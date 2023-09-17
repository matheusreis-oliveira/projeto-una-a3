using CatalogApi.Context;
using CatalogApi.Models;
using Microsoft.AspNetCore.Identity;

public static class Seeder
{
    public static async Task Initialize(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        var user = new IdentityUser
        {
            UserName = "teste@teste.com",
            Email = "teste@teste.com",
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(user, "Test@123");
        if (!result.Succeeded)
        {
            throw new Exception("Failed to create test user");
        }

        var category = new Category
        {
            Name = "Eletrônicos",
            ImageUrl = "https://example.com/images/eletronicos.jpg",
        };

        var products = new List<Product>
        {
            new Product
            {
                Name = "Smartphone",
                Description = "Smartphone com tela de 6.5 polegadas",
                Price = 1000.00m,
                ImageUrl = "https://example.com/images/smartphone.jpg",
                Stock = 100,
                RegisterDate = DateTime.Now,
                Category = category
            },
            new Product
            {
                Name = "Notebook",
                Description = "Notebook com processador de última geração",
                Price = 2000.00m,
                ImageUrl = "https://example.com/images/notebook.jpg",
                Stock = 50,
                RegisterDate = DateTime.Now,
                Category = category
            }
        };

        await context.Category.AddAsync(category);
        await context.Product.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}
