namespace SistemApi.Domain.Models.Dish;

public class DishModel
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public DishModel(int id, string name, string? description, decimal price, bool isActive, DateTime createdAt = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del plato es requerido.", nameof(name));

        if (price < 0)
            throw new ArgumentException("El precio no puede ser negativo.", nameof(price));

        Id = id;
        Name = name;
        Description = description;
        Price = price;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public void UpdateDetails(string name, string? description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del plato es requerido.", nameof(name));

        if (price < 0)
            throw new ArgumentException("El precio no puede ser negativo.", nameof(price));

        Name = name;
        Description = description;
        Price = price;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
