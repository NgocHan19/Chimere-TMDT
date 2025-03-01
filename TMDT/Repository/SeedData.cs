using TMDT.Models;
using Microsoft.EntityFrameworkCore;

namespace TMDT.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if (!_context.Products.Any()) 
			{ 
				CategoryModel Macbook = new CategoryModel { Name= "Macbook", Slug = "Macbook", Description = "This is Macbook", Status = 1 };
				CategoryModel Laptop = new CategoryModel { Name = "Laptop", Slug = "Laptop", Description = "This is Laptop", Status = 1 };
				BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "This is Apple", Status = 1 };
				BrandModel samsung = new BrandModel { Name = "Samsung", Slug = "samsung", Description = "This is Samsung", Status = 1 };
				_context.Products.AddRange(
					new ProductModel { Name = "Macbook X", Slug = "macbook x", Description = "This is Macbook", Image = "1.jpg", Category = Macbook, Price = 1299, Brand = apple },
					new ProductModel { Name = "Laptop X", Slug = "laptop x", Description = "This is Laptop", Image = "1.jpg", Category = Laptop, Price = 1199, Brand = samsung }
				);
				_context.SaveChanges();
			}
		}
	}
}
