using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quicksilver.BAL.Operations;
using Quicksilver.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quicksilver.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Coupon : Controller
    {
        private readonly CouponOperations couponOperations = new CouponOperations();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetCoupons()
        {
            var list = couponOperations.GetAllCoupons();
            return Ok(list);
        }

        [HttpGet]
        public IActionResult GetCoupon(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invaid coupon id");
            }
            return Ok(couponOperations.GetCouponSingle(id));
        }

        [HttpPost]
        public IActionResult CreateCoupon(CouponDto couponDto)
        {
            if (couponDto.Code == null || couponDto.Discount == 0 || couponDto.DateExpired == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            couponDto.Id = couponOperations.CreateCoupon(couponDto);
            return CreatedAtAction("GetCoupon/" + couponDto.Id, couponDto);
        }

        [HttpPut]
        public IActionResult UpdateCoupon(CouponDto couponDto)
        {
            if (couponDto.Id == 0 || couponDto.Code == null || couponDto.Discount == 0 || couponDto.DateExpired == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            couponOperations.UpdateCoupon(couponDto);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteCoupon(int Id)
        {
            if (Id==0||!ModelState.IsValid)
            {
                return BadRequest("Invlaid Data");
            }
            couponOperations.DeleteCoupon(Id);
            return Ok();
        }

    }
}
