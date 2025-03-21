﻿using TMDT.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TMDT.Repository
{
	public class DataContext : IdentityDbContext<AppUserModel>
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) 
		{ 
		
		}
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<RatingModel> Ratings { get; set; }
		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
		public DbSet<ContactModel> Contacts { get; set; }
		public DbSet<WishlistModel> Wishlists { get; set; }
		public DbSet<ProductQuantityModel> ProductQuantities { get; set; }
		public DbSet<ShippingModel> Shippings { get; set; }
		public DbSet<MomoInfoModel> MomoInfos { get; set; }
		public DbSet<VNPayInfoModel> VNPayInfos { get; set; }

		public DbSet<TMDT.Models.UserModel> UserModel { get; set; }
	}
}
