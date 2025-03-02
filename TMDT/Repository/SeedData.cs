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
                // Tạo danh mục cha
                var collection = new CategoryModel { Name = "COLLECTION", Slug = "collection", Description = "Bộ sưu tập nước hoa", Status = 1 };
                var scent = new CategoryModel { Name = "SCENT", Slug = "scent", Description = "Các mùi hương nước hoa", Status = 1 };

                var gucci = new BrandModel { Name = "Gucci", Slug = "gucci", Description = "Thương hiệu Gucci" };
                var dior = new BrandModel { Name = "Dior", Slug = "dior", Description = "Thương hiệu Dior" };

                var summer = new CategoryModel { Name = "Summer", Slug = "summer", Description = "Mùa hè", Status = 1, ParentCategory = collection };
                var winter = new CategoryModel { Name = "Winter", Slug = "winter", Description = "Mùa đông", Status = 1, ParentCategory = collection };
                var woody = new CategoryModel { Name = "Woody", Slug = "woody", Description = "Mùi hương gỗ", Status = 1, ParentCategory = scent };

                // Thêm danh mục vào database
                _context.Categories.AddRange(collection, scent, summer, winter, woody);
                _context.SaveChanges();

                _context.Products.AddRange(
                    new ProductModel
                    {
                        Name = "Gucci Bloom",
                        Slug = "gucci-bloom",
                        Description = "Hương hoa nhài đầy quyến rũ.",
                        Image = "gucci-bloom.jpg",
                        Category = collection,
                        SubCategory = summer, 
                        Price = 140,
                        Brand = gucci, 
                        Quantity = 100,
                        Sold = 20,
                        Component = "Hoa nhài, Hoa huệ, Rễ diên vĩ",
                        Inspiration = "Mùa hè tràn đầy hương hoa",
                        Capacity = 100
                    },
                    new ProductModel
                    {
                        Name = "Dior Fahrenheit",
                        Slug = "dior-fahrenheit",
                        Description = "Hương gỗ ấm áp cho mùa đông.",
                        Image = "dior-fahrenheit.jpg",
                        Category = collection,
                        SubCategory = winter, 
                        Price = 160,
                        Brand = dior, 
                        Quantity = 80,
                        Sold = 35,
                        Component = "Hoa oải hương, Gỗ đàn hương, Hổ phách",
                        Inspiration = "Sự mạnh mẽ và ấm áp trong mùa đông",
                        Capacity = 100
                    },
                    new ProductModel
                    {
                        Name = "Chanel Coco Noir",
                        Slug = "coco-noir",
                        Description = "Hương gỗ trầm bí ẩn và quyến rũ.",
                        Image = "coco-noir.jpg",
                        Category = scent,
                        SubCategory = woody, 
                        Price = 180,
                        Brand = dior, 
                        Quantity = 90,
                        Sold = 28,
                        Component = "Gỗ đàn hương, Xạ hương, Hoắc hương",
                        Inspiration = "Vẻ đẹp huyền bí của đêm tối",
                        Capacity = 100
                    },
                    new ProductModel
                    {
                        Name = "YSL La Nuit De L'Homme",
                        Slug = "ysl-la-nuit",
                        Description = "Hương cay nồng và gỗ trầm đầy cuốn hút.",
                        Image = "ysl-la-nuit.jpg",
                        Category = scent, 
                        SubCategory = woody, 
                        Price = 150,
                        Brand = dior, 
                        Quantity = 95,
                        Sold = 30,
                        Component = "Hạt tiêu đen, Gỗ tuyết tùng, Hoa oải hương",
                        Inspiration = "Sự cuốn hút của người đàn ông hiện đại",
                        Capacity = 100
                    },
                    new ProductModel
                    {
                        Name = "Versace Eros",
                        Slug = "versace-eros",
                        Description = "Hương bạc hà tươi mát, đầy năng lượng.",
                        Image = "versace-eros.jpg",
                        Category = scent, 
                        SubCategory = woody, 
                        Price = 130,
                        Brand = gucci,
                        Quantity = 110,
                        Sold = 25,
                        Component = "Bạc hà, Vani, Gỗ tuyết tùng",
                        Inspiration = "Sự quyến rũ và mạnh mẽ",
                        Capacity = 100
                    }
                );

                _context.SaveChanges();
            }
        }
    }
}
