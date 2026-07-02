using SistemApi.Api.Endpoints.Common;
using SistemApi.Application.Services.Dish;
using SistemApi.Domain.Models.Dish;

namespace SistemApi.Api.Endpoints.Dish;

public static class DishGroupEndpoint
{
    public static void MapDishGroupEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("api/dish").WithTags("Dish");

        group.MapGet("/", (DishService service) => service.GetAllAsync().ToApiResult());

        group.MapGet("/{id:int}", (int id, DishService service) => service.GetByIdAsync(id).ToApiResult());

        group.MapPost("/", (DishModel model, DishService service) => service.CreateAsync(model).ToApiResult());

        group.MapPut("/{id:int}", (int id, DishModel model, DishService service) => service.UpdateAsync(id, model).ToApiResult());

        group.MapDelete("/{id:int}", (int id, DishService service) => service.DeleteAsync(id).ToApiResult());
    }
}
