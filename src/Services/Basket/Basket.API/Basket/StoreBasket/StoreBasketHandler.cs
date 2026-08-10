using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;
public record StoreBasketCommand(ShoppingCart ShoppingCart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart).NotNull().WithMessage("Shopping cart cannot be null.");
        RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("User name cannot be empty.");
        RuleFor(x => x.ShoppingCart.Items).NotEmpty().WithMessage("Shopping cart items cannot be empty.");
    }
}
public class StoreBasketCommandHandler(
        IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountClient)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart shoppingCart = command.ShoppingCart;

        await DeductDiscount(shoppingCart, cancellationToken);

        await repository.StoreBasketAsync(shoppingCart, cancellationToken);

        return new(shoppingCart.UserName);
    }

    public async Task DeductDiscount(ShoppingCart shoppingCart, CancellationToken cancellationToken)
    {
        foreach (var item in shoppingCart.Items)
        {
            var discountRequest = new GetDiscountRequest { ProductName = item.ProductName };
            var discountResponse = await discountClient.GetDiscountAsync(discountRequest, cancellationToken: cancellationToken);

            if (discountResponse is null)
                continue;

            item.Price -= discountResponse.Amount;
        }
    }
}
