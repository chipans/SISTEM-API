using SistemApi.Domain.Models.Dish;

namespace SistemApi.Domain.Repositories;

public interface IDishRepository
{
    Task<List<DishModel>> GetAllAsync();
    Task<DishModel?> GetByIdAsync(int id);
    Task<DishModel> CreateAsync(DishModel dish);
    Task<DishModel?> UpdateAsync(DishModel dish);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByIdAsync(int id);
}
