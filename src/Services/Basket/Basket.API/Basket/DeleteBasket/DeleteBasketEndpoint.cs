
using Basket.API.Basket.GetBasket;

namespace Basket.API.Basket.DeleteBasket
{
    //public record class DeleteBasketRequest(string UserName);
    public record DeleteBasketResponse(bool Success);

    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var command = new DeleteBasketCommand(userName);
                var result = await sender.Send(command);
                var response = result.Adapt<DeleteBasketResponse>();

                return Results.Ok(response);
            })
            .WithName("DeleteBasketByUserName")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Basket By UserName")
            .WithDescription("Delete Basket By UserName");
        }
    }
}
