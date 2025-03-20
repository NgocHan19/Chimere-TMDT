
﻿using TMDT.Areas.Admin.Repository;
using TMDT.Models;
using TMDT.Models.ViewModel;
using TMDT.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TMDT.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;
		public readonly DataContext _dataContext;
		public readonly IEmailSender _emailSender;

		public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager, DataContext dataContext, IEmailSender emailSender)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_dataContext = dataContext;
			_emailSender = emailSender;
		}

		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);
				if (result.Succeeded)
				{
					return Redirect(loginVM.ReturnUrl ?? "/");
				}
				ModelState.AddModelError("", "Invalid Username or Password");
			}
			return View(loginVM);
		}

		public IActionResult ForgotPassword()
		{
			return View();
		}

		public async Task<IActionResult> SendEmailForgotPassword(AppUserModel user)
		{
			var checkMail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

			if (checkMail == null)
			{
				TempData["error"] = "Email not found.";
				return RedirectToAction("ForgotPassword", "Account");
			}
			else
			{
				string token = Guid.NewGuid().ToString();
				// Update token for user
				checkMail.Token = token;
				_dataContext.Update(checkMail);
				await _dataContext.SaveChangesAsync();

				// Send reset email
				var receiver = checkMail.Email;
				var subject = "Change password for user " + checkMail.Email;
				var message = $"Click on this link to change your password: <a href='{Request.Scheme}://{Request.Host}/Account/NewPassword?email={checkMail.Email}&token={token}'>Change Password</a>";

				await _emailSender.SendEmailAsync(receiver, subject, message);
			}

			TempData["success"] = "An email has been sent to your registered email address with password reset instructions.";
			return RedirectToAction("ForgotPassword", "Account");
		}

		public async Task<IActionResult> NewPassword(AppUserModel user, string token)
		{
			var checkUser = await _userManager.Users
				.Where(u => u.Email == user.Email)
				.Where(u => u.Token == user.Token)
				.FirstOrDefaultAsync();

			if (checkUser != null)
			{
				ViewBag.Email = checkUser.Email;
				ViewBag.Token = token;
			}
			else
			{
				TempData["error"] = "Email not found or token is incorrect.";
				return RedirectToAction("ForgotPassword", "Account");
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateNewPassword(AppUserModel user)
		{
			var checkUser = await _userManager.Users
				.Where(u => u.Email == user.Email)
				.Where(u => u.Token == user.Token)
				.FirstOrDefaultAsync();

			if (checkUser != null)
			{
				string newToken = Guid.NewGuid().ToString();
				var passwordHasher = new PasswordHasher<AppUserModel>();
				var passwordHash = passwordHasher.HashPassword(checkUser, user.PasswordHash);

				checkUser.PasswordHash = passwordHash;
				checkUser.Token = newToken;

				var result = await _userManager.UpdateAsync(checkUser);

				if (result.Succeeded)
				{
					TempData["success"] = "Password updated successfully!";
					return RedirectToAction("Login", "Account");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
					TempData["error"] = "Password update failed.";
					return RedirectToAction("ForgotPassword", "Account");
				}
			}
			else
			{
				TempData["error"] = "Email not found or token is incorrect.";
				return RedirectToAction("ForgotPassword", "Account");
			}
		}

		public IActionResult Create()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if(ModelState.IsValid)
			{
				//Kiểm tra xem email đã tồn tại chưa
				var checkEmail = await _userManager.FindByEmailAsync(user.Email);
				if(checkEmail != null)
				{
					ModelState.AddModelError("Email", "Email already exists.");
					return View(user);
				}

				//Kiểm tra xem sđt đã tồn tại chưa
				var checkPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber);
				if (checkPhoneNumber != null)
				{
					ModelState.AddModelError("PhoneNumber", "Phone number already exists.");
					return View(user);
				}
				
				//Tạo user mới
				AppUserModel newUser = new AppUserModel
				{
					UserName = user.UserName,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber
				};
				IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
				if (result.Succeeded)
				{
					var addRole = await _userManager.AddToRoleAsync(newUser, "User");
					if (addRole.Succeeded)
					{
						TempData["success"] = "Account created successfully!";
						return RedirectToAction("Login", "Account");
					}
					else
					{
						foreach (IdentityError error in addRole.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}
					}
				}
				else
				{
					foreach (IdentityError error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}
			return View(user);
        }

        public async Task<IActionResult> Logout(string returnUrl = "/")
		{
			await _signInManager.SignOutAsync();
			return Redirect(returnUrl);
		}

        public async Task<IActionResult> Portal()
        {
            // Step 1: Get the current user's email
            var email = User.FindFirstValue(ClaimTypes.Email);
            // Step 2: Ensure email is not null
            if (string.IsNullOrEmpty(email))
            {
                return NotFound("User is not logged in or no email found.");
            }
            // Step 3: Get the current user using the UserManager
            var currentUser = await _userManager.FindByEmailAsync(email);
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }
            // Step 4: Return the view with the model
            return View(currentUser);
        }

        public async Task<IActionResult> PersonalOrder()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return NotFound("User is not logged in or no email found.");
            }
            var check = _dataContext.Orders
                        .Where(d => d.UserName == email)
                        .OrderBy(c => c.CreatedDate);
            return View(await check.OrderByDescending(p => p.CreatedDate).ToListAsync());
        }

        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
            ViewBag.Order = order;
            var DetailsOrder = await _dataContext.OrderDetails
        .Include(o => o.Product)
        .Where(o => o.OrderCode == ordercode)
        .ToListAsync();
            return View(DetailsOrder);
        }

		public async Task<IActionResult> CancelOrder(string orderCode)
		{
			// Lấy Email của người dùng hiện tại từ Claims
			var email = User.FindFirstValue(ClaimTypes.Email);
			if (string.IsNullOrEmpty(email))
			{
				TempData["error"] = "Unable to identify the user. Please log in again.";
				return RedirectToAction("Login", "Account"); // Nếu không đăng nhập, chuyển hướng về Login
			}

			// Kiểm tra đơn hàng có tồn tại và thuộc về người dùng hiện tại không
			var order = await _dataContext.Orders
				.FirstOrDefaultAsync(o => o.OrderCode == orderCode && o.UserName == email);

			if (order == null)
			{
				TempData["error"] = "Order not found or you do not have permission to cancel this order.";
				return RedirectToAction("PersonalOrder"); // Chuyển hướng về danh sách đơn hàng cá nhân
			}
			// Cập nhật trạng thái đơn hàng thành "Đã hủy"
			order.Status = 6;

			_dataContext.Orders.Update(order);
			await _dataContext.SaveChangesAsync();

			TempData["success"] = "Order has been canceled successfully.";
			return RedirectToAction("PersonalOrder"); // Chuyển hướng về danh sách đơn hàng cá nhân
		}
		[HttpPost]
        public async Task<IActionResult> ConfirmDelivery(string orderCode)
        {
            // Lấy email của người dùng từ Claims
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                TempData["error"] = "Unable to identify the user. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            // Tìm đơn hàng theo mã và người dùng
            var order = await _dataContext.Orders
                .FirstOrDefaultAsync(o => o.OrderCode == orderCode && o.UserName == email);

            if (order == null)
            {
                TempData["error"] = "Order not found or you do not have permission to confirm this delivery.";
                return RedirectToAction("PersonalOrder"); // Chuyển hướng về danh sách đơn hàng cá nhân
            }

            // Kiểm tra xem đơn hàng có ở trạng thái "Hàng đã giao tới nơi" (4) không
            if (order.Status == 4)
            {
                order.Status = 5;  // Cập nhật trạng thái thành "Đơn hàng đã hoàn thành"
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Order has been confirmed as received.";
            }
            else
            {
                TempData["error"] = "This order cannot be confirmed at this stage.";
            }
            return RedirectToAction("PersonalOrder"); // Chuyển hướng về danh sách đơn hàng cá nhân
        }
    }
}
