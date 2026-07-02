using System.Net;
using SistemApi.Domain.Commom;
using SistemApi.Domain.Models.Dish;
using SistemApi.Domain.Repositories;

namespace SistemApi.Application.Services.Dish;

public class DishService
{
    private readonly IDishRepository _repository;

    public DishService(IDishRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<DishModel>>> GetAllAsync()
    {
        var dishes = await _repository.GetAllAsync();
        return Result<List<DishModel>>.Success(dishes);
    }

    public async Task<Result<DishModel>> GetByIdAsync(int id)
    {
        var dish = await _repository.GetByIdAsync(id);
        if (dish is null)
            return Result<DishModel>.Failure(["El plato no fue encontrado."], HttpStatusCode.NotFound);

        return Result<DishModel>.Success(dish);
    }

    public async Task<Result<DishModel>> CreateAsync(DishModel dish)
    {
        var created = await _repository.CreateAsync(dish);
        return Result<DishModel>.Success(created, HttpStatusCode.Created);
    }

    public async Task<Result<DishModel>> UpdateAsync(int id, DishModel dish)
    {
        if (!await _repository.ExistsByIdAsync(id))
            return Result<DishModel>.Failure(["El plato no fue encontrado."], HttpStatusCode.NotFound);

        var updated = await _repository.UpdateAsync(dish);
        return Result<DishModel>.Success(updated!);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        if (!await _repository.ExistsByIdAsync(id))
            return Result<bool>.Failure(["El plato no fue encontrado."], HttpStatusCode.NotFound);

        var deleted = await _repository.DeleteAsync(id);
        return Result<bool>.Success(deleted);
    }
}
