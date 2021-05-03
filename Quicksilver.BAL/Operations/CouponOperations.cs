using System;
using System.Collections.Generic;
using System.Text;
using Quicksilver.DAL.Repositories;
using Quicksilver.DAL.DTOs;

namespace Quicksilver.BAL.Operations
{
    public class CouponOperations
    {
        private readonly CouponRepository couponRepository = new CouponRepository();

        public List<CouponDto> GetAllCoupons()
        {
            return couponRepository.GetAllCoupons();
        }

        public CouponDto GetCouponSingle(int id)
        {
            return couponRepository.GetCoupon(id);
        }

        public int CreateCoupon(CouponDto couponDto)
        {
            couponDto.DateCreated = DateTime.Today;
            return couponRepository.CreateCoupon(couponDto);
        }

        public void UpdateCoupon(CouponDto couponDto)
        {
            couponRepository.UpdateCoupon(couponDto);
        }

        public void DeleteCoupon(int id)
        {
            couponRepository.DeleteCoupon(id);
        }
    }
}
