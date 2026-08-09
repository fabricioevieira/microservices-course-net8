using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext db, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await db.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon is null)
            return new CouponModel { ProductName = "No Discount", Amount = 0, Description = string.Empty };

        logger.LogInformation($"Discount is retrieved for product: {request.ProductName}, Amount: {coupon.Amount}");

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        db.Coupons.Add(coupon);
        await db.SaveChangesAsync();

        logger.LogInformation($"Discount is successfully created for product: {request.Coupon.ProductName}, Amount: {request.Coupon.Amount}");

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        db.Coupons.Update(coupon);
        await db.SaveChangesAsync();

        logger.LogInformation($"Discount is updated for product: {request.Coupon.ProductName}, Amount: {request.Coupon.Amount}");

        return request.Coupon;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleted = await db.Coupons.Where(x => x.ProductName == request.ProductName).ExecuteDeleteAsync();

        if (deleted == 0)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount not found for product: {request.ProductName}"));

        logger.LogInformation($"Discount is deleted for product: {request.ProductName}");
        return new DeleteDiscountResponse { Success = true };
    }
}
