using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Quicksilver.DAL.DTOs;
using Quicksilver.DAL.Interfaces;
using Quicksilver.DAL.Repositories;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Quicksilver.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly CommonRepository _commonRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserOperations _userOperation;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TransactionController(IConfiguration configuration, IUserOperations userOperation, IWebHostEnvironment webHostEnvironment)
        {
            _commonRepository = new CommonRepository();
            _configuration = configuration;
            _userOperation = userOperation;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public IActionResult ApplyCoupon(string code)
        {
            var coupon = _commonRepository.CanApplyCoupon(code);
            var currentDate = DateTime.Now;
            if (coupon!=null && currentDate<coupon.DateExpired && code==coupon.Code)
            {
                return Ok(coupon);
            }
            else
            {
                return BadRequest("Invalid Code");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CalculateEstimate(CalcEstimate Estimate)
        {
            if (Estimate.CourierType==0||Estimate.RecipientStationId==0||Estimate.SenderStationId==0||Estimate.Weight==0||Estimate.Volume==0 || Estimate.SenderAddress==null || Estimate.RecipientAddress==null)
            {
                return BadRequest("Invalid Details");
            }
            var rate = _commonRepository.GetRateByCourierTypeAndWeight(Estimate.Weight,Estimate.CourierType);
            if (rate==null ||rate.Rate==0)
            {
                return BadRequest("Sorry, We are not currently this weight in that delivery type.");
            }
            var distanceObject = await GetDistance(Estimate.SenderAddress,Estimate.RecipientAddress);
            if (distanceObject == null || distanceObject.rows[0].elements[0].status == "ZERO_RESULTS" || distanceObject.rows[0].elements[0].status == "UNKNOWN_ERROR")
            {
                distanceObject = await GetDistance(Estimate.SenderAddress, Estimate.RecipientAddress);
            }
            if (distanceObject==null || distanceObject.rows[0].elements[0].status=="ZERO_RESULTS")
            {
                return BadRequest("Sorry, Our system was unable to calculate the distance.");
            }

            var coupon = _commonRepository.CanApplyCoupon(Estimate.CouponCode);
            if (!string.IsNullOrEmpty(Estimate.CouponCode) && coupon==null)
            {
                return BadRequest("Invalid coupon please remove or change coupon.");
            }
            var resultEstimate = GetEstimate(coupon,distanceObject,rate);
            return Ok(resultEstimate);
        }





        private ResultEstimate GetEstimate(CouponDto coupon,DistanceMatrixAiDto distanceObject,RateDto rate)
        {
            var TaxRate = _configuration.GetValue<double>("TaxRate");
            ResultEstimate resultEstimate = new ResultEstimate();
            resultEstimate.IntialCost = ((distanceObject.rows[0].elements[0].distance.value / 1000) * (rate.Rate/10));
            resultEstimate.Tax = TaxRate;
            resultEstimate.Discount = coupon== null?0:coupon.Discount;
            resultEstimate.TaxAmount = (resultEstimate.IntialCost) * (TaxRate / 100);
            resultEstimate.DiscountAmt = coupon==null?0:(resultEstimate.IntialCost * (resultEstimate.Discount / 100));
            resultEstimate.GrandTotal = (resultEstimate.IntialCost+resultEstimate.TaxAmount) - resultEstimate.DiscountAmt;
            return resultEstimate;
        }

        private async Task<DistanceMatrixAiDto> GetDistance(string origin,string destination)
        {
            DistanceMatrixAiDto distanceMatrixAiDto = null;
            try
            {
                HttpClient client = new HttpClient();
                var url = _configuration.GetValue<string>("DistanceMatrixAi:Url");
                url = url.Replace("%origin%", origin).Replace("%destination%", destination);
                var Apiresponse = await client.GetAsync(url);
                if (Apiresponse.IsSuccessStatusCode)
                {
                    distanceMatrixAiDto = JsonConvert.DeserializeObject<DistanceMatrixAiDto>(await Apiresponse.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {
            }
            return distanceMatrixAiDto;
        }

        [HttpPost]
        public async Task<IActionResult> BookCourierByAgent(BookCourierDto bookCourierDto)
        {
            if (bookCourierDto.CourierType==0||bookCourierDto.Phone==0||string.IsNullOrEmpty(bookCourierDto.rAddress)|| string.IsNullOrEmpty(bookCourierDto.sAddress)|| bookCourierDto.sPIN==0 || bookCourierDto.rPIN==0||bookCourierDto.Volume==0||bookCourierDto.Weight==0|| bookCourierDto.Phone.ToString().Count()!=10||(bookCourierDto.Phone!=0 && (string.IsNullOrEmpty(bookCourierDto.Name)|| string.IsNullOrEmpty(bookCourierDto.Email))))
            {
                return BadRequest("Invalid Details");
            }
            CouponDto Coupon=null;
            if (!string.IsNullOrEmpty(bookCourierDto.CouponCode))
            {
                Coupon = _commonRepository.CanApplyCoupon(bookCourierDto.CouponCode);
                if (Coupon==null|| DateTime.Now > Coupon.DateExpired)
                {
                    return BadRequest("Invalid Coupon");
                }
            }
            var Rate = _commonRepository.GetRateByCourierTypeAndWeight(bookCourierDto.Weight, bookCourierDto.CourierType);
            if (Rate == null)
            {
                return BadRequest("Sorry, We are not currently delivering this weight and delivery type.");
            }
            var user = _userOperation.GetUserSingle(bookCourierDto.Phone);
            if (user==null||string.IsNullOrEmpty(user.Name))
            {
                var res = await _userOperation.CreateUser(bookCourierDto.Phone, bookCourierDto.Email, bookCourierDto.Name);
                if (res == false)
                {
                    return BadRequest("Failed creating user, Please try again");
                }
                user = _userOperation.GetUserSingle(bookCourierDto.Phone);
            }
            var distance = await GetDistance(bookCourierDto.sAddress,bookCourierDto.rAddress);
            if (distance == null || distance.rows[0].elements[0].status == "ZERO_RESULTS")
            {
                return BadRequest("Sorry, Our system was unable to calculate the distance.");
            }
            var Estimate = GetEstimate(Coupon,distance,Rate);
            var AgentId = await _userOperation.GetLoggerUser(User.Identity.Name);
            int? cid = 0;
            if (Coupon!=null)
            {
                cid = Coupon.Id;
            }
            var tid = _commonRepository.BookCourier(cid,Rate,bookCourierDto,user.Id,Estimate,2, distance.rows[0].elements[0].distance.value, AgentId);
            CourierBookedMessageDto courierBookedMessageDto = new CourierBookedMessageDto()
            {
                TotalCost = Estimate.GrandTotal,
                TrackingId = tid,
                ExpectedDeliveryDate = DateTime.Today.AddDays((distance.rows[0].elements[0].distance.value / 1000) / 200)
            };
            return Ok(courierBookedMessageDto);
        }

        public IActionResult getinvoice(string tid)
        {
            if (string.IsNullOrEmpty(tid))
            {
                return Ok("Invalid Details");
            }
            var contentType = "application/pdf";
            var name = "invoice.pdf";
            var uHtml = "<!DOCTYPE html> <html lang='en'> <head> <meta charset='utf-8'> <!--  This file has been downloaded from bootdey.com    @bootdey on twitter --> <!--  All snippets are MIT license http://bootdey.com/license --> <title>Invoice - Quicksilverpost</title> <meta name='viewport' content='width=device-width, initial-scale=1'> <link href='http://netdna.bootstrapcdn.com/bootstrap/4.1.1/css/bootstrap.min.css' rel='stylesheet'> </head> <body style='margin-top: 20px;'> <link href='https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css' rel='stylesheet'> <div class='container'> <div class='col-md-12'> <div class='invoice' style='background: #fff; padding: 20px;'> <!-- begin invoice-company --> <div class='invoice-company text-inverse f-w-600' style='font-size: 20px; margin-bottom: 20px;'> <span class='pull-right hidden-print' style='display: none;'> <a href='javascript:;' class='btn btn-sm btn-white m-b-10 p-l-5' style='color: #2d353c; background: #fff; border-color: #d9dfe3;'><i class='fa fa-file t-plus-1 text-danger fa-fw fa-lg'></i> Export as PDF</a> <a href='javascript:;' onclick='window.print()' class='btn btn-sm btn-white m-b-10 p-l-5' style='color: #2d353c; background: #fff; border-color: #d9dfe3;'><i class='fa fa-print t-plus-1 fa-fw fa-lg'></i> Print</a> </span>%%CompanyName%%</div> <!-- end invoice-company --> <!-- begin invoice-header --> <div class='invoice-header' style='margin: 0 -20px; background: #f0f3f4; padding: 20px; margin-bottom: 20px;'> <div class='invoice-from' style='display: table-cell; width: 1%; padding-right: 20px;'> <small>from</small> <address class='m-t-5 m-b-5'> <strong class='text-inverse' style='font-size: 16px; font-weight: 600;'>%%SPerson%%</strong><br>%%SStreetAddress%%<br> %%SCityZipCode%%<br> Phone: %%SPhone%%<br></address> </div> <div class='invoice-to' style='display: table-cell; width: 1%; padding-right: 20px;'> <small>to</small> <address class='m-t-5 m-b-5'> <strong class='text-inverse' style='font-size: 16px; font-weight: 600;'>%%RPerson%%</strong><br>%%RStreetAddress%%<br> %%RCityZipCode%%<br>Phone: %%RPhone%%</address> </div> <div class='invoice-date' style='display: table-cell; width: 1%; text-align: right; padding-left: 20px;'> <small>Invoice / %%MNTH%% period</small> <div class='date text-inverse m-t-5' style='font-size: 16px; font-weight: 600;'>%%DATE%%</div> <div class='invoice-detail'>%%TRNO%%<br> %%DTP%% </div> </div> </div> <!-- end invoice-header --> <!-- begin invoice-content --> <div class='invoice-content' style='margin-bottom: 20px;'> <!-- begin table-responsive --> <div class='table-responsive'> <table class='table table-invoice'> <thead> <tr> <th>DESCRIPTION</th> <th class='text-center' width='10%'>DISTANCE</th> <th class='text-center' width='10%'>WEIGHT</th> <th class='text-right' width='20%'>LINE TOTAL</th> </tr> </thead> <tbody> <tr> <td> <span class='text-inverse'>%%ItemTitle%%</span><br> <small>%%ItemDescription%%</small> </td> <td class='text-center'>%%Distance%%</td> <td class='text-center'>%%Weight%%</td> <td class='text-right'>₹ %%InitialCost%%</td> </tr> </tbody> </table> </div> <!-- end table-responsive --> <!-- begin invoice-price --> <div class='invoice-price' style='background: #f0f3f4; display: table; width: 100%;'> <div class='invoice-price-left' style='display: table-cell; padding: 20px; font-size: 20px; font-weight: 600; width: 75%; position: relative; vertical-align: middle;'> <div class='invoice-price-row' style='display: table; float: left;'> <div class='sub-price' style='display: table-cell; vertical-align: middle; padding: 0 20px;'> <small style='font-size: 12px; font-weight: 400; display: block;'>SUBTOTAL</small> <span class='text-inverse'>&#8377; %%IC%%</span> </div> <div class='sub-price' style='display: table-cell; vertical-align: middle; padding: 0 20px;'> <i class='fa fa-plus text-muted'></i> </div> <div class='sub-price' style='display: table-cell; vertical-align: middle; padding: 0 20px;'> <small style='font-size: 12px; font-weight: 400; display: block;'>TAX (%%TRT%%%)</small> <span class='text-inverse'>&#8377; %%TAMT%%</span> </div> <div class='sub-price' style='display: table-cell; vertical-align: middle; padding: 0 20px;'> <i class='fa fa-minus text-muted'></i> </div> <div class='sub-price' style='display: table-cell; vertical-align: middle; padding: 0 20px;'> <small style='font-size: 12px; font-weight: 400; display: block;'>DISCOUNT (%%DRT%%%)</small> <span class='text-inverse'>&#8377; %%DAMT%%</span> </div> </div> </div> <div class='invoice-price-right' style='display: table-cell; padding: 20px; position: relative; width: 25%; background: #2d353c; color: #fff; font-size: 28px; text-align: right; vertical-align: bottom; font-weight: 300;'> <small style='font-weight: 400; display: block; opacity: .6; position: absolute; top: 10px; left: 10px; font-size: 12px;'>TOTAL</small> <span class='f-w-600'>&#8377; %%GAMT%%</span> </div> </div> <!-- end invoice-price --> </div> <!-- end invoice-content --> <!-- begin invoice-note --> <div class='invoice-note' style='color: #999; margin-top: 80px; font-size: 85%; margin-bottom: 20px;'> * Make all cheques payable to Quicksilverpost<br> * Company does not guarantee for Fragile items<br> * If you have any questions concerning this invoice, contact [%%AName%%, %%APhone%%, %%AEmail%%] </div> <!-- end invoice-note --> <!-- begin invoice-footer --> <div class='invoice-footer' style='border-top: 1px solid #ddd; padding-top: 10px; font-size: 10px;'> <p class='text-center m-b-5 f-w-600'> THANK YOU FOR YOUR ORDER </p> <p class='text-center'> <span class='m-r-10'><i class='fa fa-fw fa-lg fa-globe'></i> quicksilverpost.ml</span> <span class='m-r-10'><i class='fa fa-fw fa-lg fa-phone-volume'></i> T:8561-957277</span> <span class='m-r-10'><i class='fa fa-fw fa-lg fa-envelope'></i> support@quicksilverpost.ml</span> </p> </div> <!-- end invoice-footer --> </div> </div> </div> <script src='http://code.jquery.com/jquery-1.10.2.min.js'></script> <script src='http://netdna.bootstrapcdn.com/bootstrap/4.1.1/js/bootstrap.min.js'></script> <script type='text/javascript'> </script> </body> </html>";

            uHtml = uHtml.Replace("%%CompanyName%%", "Quicksilverpost");
            var OrderDetails = _commonRepository.GetOrderDetails(tid);
            var tamt = OrderDetails.InitialCost * OrderDetails.Tax * 0.01;
            var damt = (OrderDetails.InitialCost+tamt) * OrderDetails.Discount * 0.01;
            var rPin = OrderDetails.recipientAddress.Substring(OrderDetails.recipientAddress.LastIndexOf(",") + 1);
            var sPin = OrderDetails.SenderAddress.Substring(OrderDetails.SenderAddress.LastIndexOf(",") + 1);
            
            OrderDetails.recipientAddress = OrderDetails.recipientAddress.Substring(0, OrderDetails.recipientAddress.LastIndexOf(",")).Trim();
            OrderDetails.SenderAddress = OrderDetails.SenderAddress.Substring(0, OrderDetails.SenderAddress.LastIndexOf(",")).Trim();
            
            var rState = OrderDetails.recipientAddress.Substring(OrderDetails.recipientAddress.LastIndexOf(",")+ 1).Trim();
            var sState = OrderDetails.SenderAddress.Substring(OrderDetails.SenderAddress.LastIndexOf(",") + 1).Trim();

            OrderDetails.recipientAddress = OrderDetails.recipientAddress.Substring(0, OrderDetails.recipientAddress.LastIndexOf(",")).Trim();
            OrderDetails.SenderAddress = OrderDetails.SenderAddress.Substring(0, OrderDetails.SenderAddress.LastIndexOf(",")).Trim();

            var rCity = OrderDetails.recipientAddress.Substring(OrderDetails.recipientAddress.LastIndexOf(",") + 1).Trim();
            var sCity = OrderDetails.SenderAddress.Substring(OrderDetails.SenderAddress.LastIndexOf(",") + 1).Trim();

            OrderDetails.recipientAddress = OrderDetails.recipientAddress.Substring(0, OrderDetails.recipientAddress.LastIndexOf(",")).Trim();
            OrderDetails.SenderAddress = OrderDetails.SenderAddress.Substring(0, OrderDetails.SenderAddress.LastIndexOf(",")).Trim();

            var rAddr = OrderDetails.recipientAddress.Trim();
            var sAddr = OrderDetails.SenderAddress.Trim();

            uHtml = uHtml.Replace("%%AName%%", OrderDetails.aName).Replace("%%APhone%%", OrderDetails.aPhone.ToString()).Replace("%%AEmail%%", OrderDetails.aEmail)
                .Replace("%%SPerson%%", OrderDetails.FullName).Replace("%%RPerson%%", OrderDetails.RecipientName)
                .Replace("%%DATE%%", OrderDetails.OrderDate.Date.ToShortDateString()).Replace("%%TRNO%%", OrderDetails.TrackingId.ToString())
                .Replace("%%DTP%%", OrderDetails.CourierType).Replace("%%Distance%%", OrderDetails.Distance.ToString())
                .Replace("%%Weight%%", OrderDetails.Weight.ToString()).Replace("%%InitialCost%%", OrderDetails.InitialCost.ToString())
                .Replace("%%MNTH%%", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(OrderDetails.OrderDate.Month))
                .Replace("%%SPhone%%", OrderDetails.PhoneNumber.ToString()).Replace("%%RPhone%%", OrderDetails.RecipientMobileNo.ToString())
                .Replace("%%IC%%", OrderDetails.InitialCost.ToString()).Replace("%%TRT%%", OrderDetails.Tax.ToString())
                .Replace("%%DRT%%", OrderDetails.Discount.ToString()).Replace("%%GAMT%%", OrderDetails.FinalCost.ToString())
                .Replace("%%TAMT%%", tamt.ToString()).Replace("%%DAMT%%", damt.ToString())
                .Replace("%%SStreetAddress%%", sAddr).Replace("%%RStreetAddress%%", rAddr)
                .Replace("%%SCityZipCode%%", sCity + "," + sState + "," + sPin)
                .Replace("%%RCityZipCode%%", rCity + "," + rState + "," + rPin);

            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            var pdfbyte = converter.ConvertHtmlString(uHtml);
            pdfbyte.Save(name);
            var path = Path.Combine(_webHostEnvironment.ContentRootPath,name);
            byte[] pdf = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);
            return File(pdf, contentType, name);
        }


        public IActionResult TrackCourier(long tid)
        {
            try
            {
                if (tid == 0)
                {
                    return BadRequest("Invalid Tracking Id");
                }
                var track = _commonRepository.GetTrackCourier(tid);
                if (track == null || string.IsNullOrEmpty(track.recipientAddress))
                {
                    return BadRequest("Invalid Tracking Id");
                }
                track.ExDeliveryDate = track.OrderDate.AddDays((track.Distance / 1000) / 200);
                var tin = track.recipientAddress.LastIndexOf(",");
                var addr = track.recipientAddress.Substring(tin+1).Trim();
                track.recipientAddress = track.recipientAddress.Substring(0, tin);

                tin = track.recipientAddress.LastIndexOf(",");
                track.recipientAddress = track.recipientAddress.Substring(0, tin);
                
                tin = track.recipientAddress.LastIndexOf(",");
                addr = track.recipientAddress.Substring(tin + 1).Trim() + ", " + addr;
                track.recipientAddress = addr.Replace("Pin:","");
                return Ok(track);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> BookCourierByUser(BookCourierDto bookCourierDto)
        {
            try
            {
                if (bookCourierDto.CourierType == 0 || string.IsNullOrEmpty(bookCourierDto.rAddress) || string.IsNullOrEmpty(bookCourierDto.sAddress) || bookCourierDto.sPIN == 0 || bookCourierDto.rPIN == 0 || bookCourierDto.Volume == 0 || bookCourierDto.Weight == 0)
                {
                    return BadRequest("Invalid Details");
                }
                CouponDto Coupon = null;
                if (!string.IsNullOrEmpty(bookCourierDto.CouponCode))
                {
                    Coupon = _commonRepository.CanApplyCoupon(bookCourierDto.CouponCode);
                    if (Coupon == null || DateTime.Now > Coupon.DateExpired)
                    {
                        return BadRequest("Invalid Coupon");
                    }
                }
                var Rate = _commonRepository.GetRateByCourierTypeAndWeight(bookCourierDto.Weight, bookCourierDto.CourierType);
                if (Rate == null)
                {
                    return BadRequest("Sorry, We are not currently delivering this weight and delivery type.");
                }
                var user = _userOperation.GetUserSingle(User.Identity.Name);
                if (user == null || string.IsNullOrEmpty(user.Name))
                {
                    return BadRequest("Failed fetching user, Please try again");
                }
                bookCourierDto.sAddress = bookCourierDto.sAddress + ", " + bookCourierDto.sCity + ", " + bookCourierDto.sState + ", Pin:" + bookCourierDto.sPIN;
                bookCourierDto.rAddress = bookCourierDto.rAddress + ", " + bookCourierDto.rCity + ", " + bookCourierDto.rState + ", Pin:" + bookCourierDto.rPIN;
                var distance = await GetDistance(bookCourierDto.sAddress, bookCourierDto.rAddress);

                if (distance == null || distance.rows[0].elements[0].status == "ZERO_RESULTS" || distance.rows[0].elements[0].status == "UNKNOWN_ERROR")
                {
                    distance = await GetDistance(bookCourierDto.rAddress, bookCourierDto.rAddress);
                }
                if (distance == null || distance.rows[0].elements[0].status == "ZERO_RESULTS")
                {
                    return BadRequest("Sorry, Our system was unable to calculate the distance.");
                }

                var Estimate = GetEstimate(Coupon, distance, Rate);
                var AgentId = User.IsInRole("Agent") || User.IsInRole("Admin") ? await _userOperation.GetLoggerUser(User.Identity.Name) : null;
                int? cid = 0;
                if (Coupon != null)
                {
                    cid = Coupon.Id;
                }
                var tid = _commonRepository.BookCourier(cid, Rate, bookCourierDto, user.Id, Estimate, 1, distance.rows[0].elements[0].distance.value, AgentId, true);
                var orderId = CreateOrder(Estimate.GrandTotal, tid);
                if (string.IsNullOrEmpty(orderId))
                {
                    _commonRepository.RevertBackOrder(tid);
                    return BadRequest("Failed creating transaction");
                }
                CourierBookedMessageDto courierBookedMessageDto = new CourierBookedMessageDto()
                {
                    TotalCost = Estimate.GrandTotal,
                    TrackingId = tid,
                    ExpectedDeliveryDate = DateTime.Today.AddDays((distance.rows[0].elements[0].distance.value / 1000) / 200),
                    OrderId = orderId,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.MobileNo.ToString(),
                    Address = bookCourierDto.sAddress
                };
                return Ok(courierBookedMessageDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SuccessOrder()
        {
            var paymentStatus = Request.Form["rzPaymentStatus"].ToString();
            if (paymentStatus!= "success")
            {
                return View("Fail");
            }
            var orderId = Request.Form["rzOrderId"].ToString();
            var paymentId = Request.Form["rzPaymentId"].ToString();
            var signature = Request.Form["rzSignature"].ToString();
            var trackingId = long.Parse(Request.Form["rzTrackingId"].ToString());

            var isRetryTransaction = Request.Form["rzIsRetryPayment"].ToString();

            var validSignature = CompareSignatures(orderId, paymentId, signature);
            ViewBag.TrackingId = trackingId;
            if (validSignature)
            {
                _commonRepository.ValidateTransaction(trackingId);
                ViewBag.Message = "Congratulations!! Your payment was successful";
                ViewBag.isRetryTransaction = isRetryTransaction;
                return View("Success");
            }
            else
            {
                return View("Fail");
            }
        }

        [HttpPost]
        public IActionResult tryagain(long tid)
        {
            var orderDetails = _commonRepository.GetOrderDetailsFromTemp(tid.ToString());
            ResultEstimate resultEstimate = new ResultEstimate();
            resultEstimate.IntialCost = orderDetails.InitialCost;
            resultEstimate.Tax = orderDetails.Tax;
            resultEstimate.Discount = orderDetails.Discount;
            resultEstimate.TaxAmount = (resultEstimate.IntialCost) * (orderDetails.Tax / 100);
            resultEstimate.DiscountAmt = orderDetails.Discount == 0 ? 0 : (resultEstimate.IntialCost * (resultEstimate.Discount / 100));
            resultEstimate.GrandTotal = (resultEstimate.IntialCost + resultEstimate.TaxAmount) - resultEstimate.DiscountAmt;

            var orderId = CreateOrder(resultEstimate.GrandTotal, tid);

            if (string.IsNullOrEmpty(orderId))
            {
                _commonRepository.RevertBackOrder(tid);
                return BadRequest("Failed creating transaction");
            }
            CourierBookedMessageDto courierBookedMessageDto = new CourierBookedMessageDto()
            {
                TotalCost = resultEstimate.GrandTotal,
                TrackingId = tid,
                ExpectedDeliveryDate = DateTime.Today.AddDays((orderDetails.Distance / 1000) / 200),
                OrderId = orderId,
                Name = orderDetails.FullName,
                Email = orderDetails.Email,
                Phone = orderDetails.PhoneNumber.ToString(),
                Address = orderDetails.SenderAddress
            };
            return View("retrytransaction",courierBookedMessageDto);
        }

        private bool CompareSignatures(string orderId, string paymentId, string razorPaySignature)
        {
            var text = orderId + "|" + paymentId;
            var secret = _configuration.GetValue<string>("RazorpayKeySecret");
            var generatedSignature = CalculateSHA256(text, secret);
            if (generatedSignature == razorPaySignature)
                return true;
            else
                return false;
        }

        private string CalculateSHA256(string text, string secret)
        {
            string result = "";
            var enc = Encoding.Default;
            byte[]
            baText2BeHashed = enc.GetBytes(text),
            baSalt = enc.GetBytes(secret);
            System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baSalt);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;
        }

        public ViewResult Capture()
        {
            return View();
        }

        public ViewResult CapturePayment(string paymentId)
        {
            var key = _configuration.GetValue<string>("RazorpayKeyID");
            var secret = _configuration.GetValue<string>("RazorpayKeySecret");
            RazorpayClient client = new RazorpayClient(key, secret);
            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);
            var amount = payment.Attributes["amount"];
            var currency = payment.Attributes["currency"];

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", amount);
            options.Add("currency", currency);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            ViewBag.Message = "Payment capatured!";
            return View("Success");
        }

        private string CreateOrder(double FinalCost, long TrackingId)
        {
            try
            {
                var key = _configuration.GetValue<string>("RazorpayKeyID");
                var secret = _configuration.GetValue<string>("RazorpayKeySecret");
                RazorpayClient client = new RazorpayClient(key, secret);

                int cost = Convert.ToInt32(FinalCost * 100);

                var Notes = new Dictionary<string, string>()
                {
                    { "TrackingId", TrackingId.ToString() }
                };

                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", cost.ToString());
                options.Add("currency", "INR");
                options.Add("notes", Notes);
                var order = client.Order.Create(options);
                string orderId = order.Attributes["id"].ToString();
                return orderId;
            }
            catch (Exception ex)
            {
                _commonRepository.RevertBackOrder(TrackingId);
                throw new Exception("Failed to create order");
            }
        }
    }
}