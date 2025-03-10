using TMDT.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TMDT.Repository
{
	public class DataContext : IdentityDbContext<AppUserModel>
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<BrandModel> Brands { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<AppUserModel> AppUsers { get; set; }
		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
		public DbSet<RatingModel> Ratings { get; set; }
		public DbSet<ContactModel> Contacts { get; set; }
		public DbSet<ProductQuantityModel> ProductQuantities { get; set; }
		public DbSet<WishlistModel> Wishlists { get; set; }
		public DbSet<MomoInfoModel> MomoInfos { get; set; }

		public DbSet<TMDT.Models.UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình kiểu dữ liệu decimal chính xác
			modelBuilder.Entity<ProductModel>()
				.Property(p => p.Price)
				.HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderDetails>()
                .Property(o => o.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderModel>()
                .Property(o => o.ShippingCost)
                .HasColumnType("decimal(18,2)");
        }
    }
}
