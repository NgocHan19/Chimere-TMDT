using E_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace TMDT.Repository
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<AppUserModel> AppUsers { get; set; }
	}
}
