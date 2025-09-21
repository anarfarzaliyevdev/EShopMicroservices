using Catalog.API.Products.GetProductByCategory;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductRequest(
        Guid Id
    );

    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndpoint:ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products", async ([FromBody] DeleteProductRequest deleteProductRequest, ISender sender) =>
            {
                var command = deleteProductRequest.Adapt<DeleteProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<DeleteProductResponse>();

                return Results.Ok(response);
            })
            .WithName("DeleteProduct")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product");
        }
    }
}
