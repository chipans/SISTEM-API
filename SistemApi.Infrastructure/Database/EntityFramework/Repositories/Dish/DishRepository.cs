using Microsoft.EntityFrameworkCore;
using SistemApi.Domain.Models.Dish;
using SistemApi.Domain.Repositories;
using SistemApi.Infrastructure.Database.EntityFramework.Context;
using SistemApi.Infrastructure.Database.EntityFramework.Entities.Dish;

namespace SistemApi.Infrastructure.Database.EntityFramework.Repositories.Dish;

public class DishRepository : IDishRepository
{
    private readonly SistemApiDbContext _context;

    public DishRepository(SistemApiDbContext context)
    {
        _context = context;
    }

    public async Task<List<DishModel>> GetAllAsync()
    {
        var entities = await _context.Dish.AsNoTracking().ToListAsync();
        return entities.Select(ToModel).ToList();
    }

    public async Task<DishModel?> GetByIdAsync(int id)
    {
        var entity = await _context.Dish.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        return entity is null ? null : ToModel(entity);
    }

    public async Task<DishModel> CreateAsync(DishModel dish)
    {
        var entity = new DishEntity
        {
            Name = dish.Name,
            Description = dish.Description,
            Price = dish.Price,
            IsActive = dish.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Dish.AddAsync(entity);
        await _context.SaveChangesAsync();

        return ToModel(entity);
    }

    public async Task<DishModel?> UpdateAsync(DishModel dish)
    {
        var entity = await _context.Dish.FirstOrDefaultAsync(d => d.Id == dish.Id);
        if (entity is null) return null;

        entity.Name = dish.Name;
        entity.Description = dish.Description;
        entity.Price = dish.Price;
        entity.IsActive = dish.IsActive;

        await _context.SaveChangesAsync();
        return ToModel(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Dish.FirstOrDefaultAsync(d => d.Id == id);
        if (entity is null) return false;

        _context.Dish.Remove(entity);
        var affected = await _context.SaveChangesAsync();
        return affected > 0;
    }

    public Task<bool> ExistsByIdAsync(int id) => _context.Dish.AnyAsync(d => d.Id == id);

    private static DishModel ToModel(DishEntity entity) =>
        new(entity.Id, entity.Name, entity.Description, entity.Price, entity.IsActive, entity.CreatedAt);
}
