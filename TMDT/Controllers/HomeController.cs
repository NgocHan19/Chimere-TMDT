using TMDT.Models;
using TMDT.Models.ViewModels;
using TMDT.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace TMDT.Controllers
{
    public class HomeController : Controller
    {
		private readonly DataContext _datacontext;
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<AppUserModel> _userManager;

		public HomeController(ILogger<HomeController> logger, DataContext context, UserManager<AppUserModel> userManager)
		{
			_logger = logger;
			_datacontext = context;
			_userManager = userManager;
		}
		public async Task<IActionResult> Index(int page = 1)
		{
			int pageSize = 9;
			int totalProducts = await _datacontext.Products.CountAsync();
			var products = await _datacontext.Products
				.Include("Category")
				.Include("Brand")
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
			var brandCounts = _datacontext.Brands
				.Select(b => new
				{
					b.Name,
					b.Slug,
					ProductCount = _datacontext.Products.Count(p => p.BrandId == b.Id)
				})
				.ToList();

			var contact = _datacontext.Contacts.FirstOrDefault();

			// Thêm thông tin phân trang vào ViewBag
			ViewBag.Page = page;
			ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
			ViewBag.BrandCounts = brandCounts;
			ViewBag.Contact = contact;

			return View(products);
		}
		public IActionResult Privacy()
		{
			return View();
		}

		public async Task<IActionResult> Contact()
		{
			var contact = await _datacontext.Contacts.FirstAsync();
			return View(contact);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(int statuscode)
		{
			if (statuscode == 404)
			{
				return View("NotFound");
			}
			else
			{
				return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}
		}
		public async Task<IActionResult> AddWishList(long Id)
		{
			// Get the current user (assume you have a way to retrieve the logged-in user's Id)
			var userId = await _userManager.GetUserAsync(User);  // Modify based on how you get UserId

			// Check if the product is already in the wishlist for this user
			var existingWishlist = await _datacontext.Wishlists
				.FirstOrDefaultAsync(w => w.ProductId == Id && w.UserId == userId.Id);

			if (existingWishlist != null)
			{
				TempData["error"] = "Product is already in your wishlist.";
				return RedirectToAction("Wishlist", "Home");
			}

			// If not, add the product to the wishlist
			var newWishlist = new WishlistModel
			{
				ProductId = Id,
				UserId = userId.Id
			};

			_datacontext.Wishlists.Add(newWishlist);
			await _datacontext.SaveChangesAsync();
			TempData["success"] = "Product added to wishlist successfully.";

			return RedirectToAction("Wishlist", "Home");
		}
		public async Task<IActionResult> Wishlist()
		{
			var wishlist_product = await (from w in _datacontext.Wishlists
										  join p in _datacontext.Products on w.ProductId equals p.Id
										  join u in _datacontext.Users on w.UserId equals u.Id
										  select new { User = u, Product = p, Wishlists = w }).ToListAsync();

			return View(wishlist_product);
		}
		public async Task<IActionResult> DeleteWishlist(int Id)
		{
			WishlistModel wishlist = await _datacontext.Wishlists.FindAsync(Id);
			_datacontext.Wishlists.Remove(wishlist);
			await _datacontext.SaveChangesAsync();
			TempData["success"] = "Wishlist removed successfully";
			return RedirectToAction("Wishlist", "Home");
		}
		public async Task<IActionResult> Account()
		{
			var userId = _userManager.GetUserId(User);
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return NotFound();
			}
			var userModel = new UserModel { UserName = user.UserName, Email = user.Email };
			return View(userModel);
		}
	}
}
