using Dicount.Grpc.Data;
using Dicount.Grpc.Models;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Dicount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext,
                                 ILogger<DiscountService> logger):
                 DiscountProtoService.DiscountProtoServiceBase
    {

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons
                                        .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if (coupon == null)
            {
                coupon = new Models.Coupon
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No Discount Desc"
                };
            }

            logger.LogInformation("Discount is retrieved for ProductName : {ProductName}, Amount : {Amount}",
                                  coupon.ProductName, coupon.Amount);

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if(coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
            }
            
            await dbContext.Coupons.AddAsync(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully created");

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
            }

            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully updated");

            return coupon.Adapt<CouponModel>(); 
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons
                                        .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Discount not found"));
            }

            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully removed");

            return new DeleteDiscountResponse { Success = true };
        }
    }
}
