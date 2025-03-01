﻿using TMDT.Models;
using TMDT.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TMDT.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
	{
		private readonly DataContext _dataContext;
		public OrderController(DataContext context)
		{
			_dataContext = context;
		}
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Orders.OrderByDescending(o => o.CreatedDate).ToListAsync());
        }

        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
            ViewBag.Order = order;

            var DetailsOrder = await _dataContext.OrderDetails.Include(o=>o.Product).Where(o=>o.OrderCode==ordercode).ToListAsync();
            return View(DetailsOrder);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(string ordercode, int status)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;

            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Order status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updateing the order status.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string ordercode)
        {
            // Tìm đơn hàng theo ordercode
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);

            if (order == null)
            {
                return NotFound();  // Trả về lỗi nếu không tìm thấy order
            }

            try
            {
                // Xóa tất cả OrderDetails liên quan (nếu cần thiết)
                var orderDetails = await _dataContext.OrderDetails.Where(od => od.OrderCode == ordercode).ToListAsync();

                if (orderDetails.Any())
                {
                    _dataContext.OrderDetails.RemoveRange(orderDetails);
                }

                // Xóa Order
                _dataContext.Orders.Remove(order);
                await _dataContext.SaveChangesAsync(); // Lưu thay đổi vào DB

                TempData["success"] = "Order deleted successfully!"; // Thông báo thành công
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và thông báo cho người dùng
                ModelState.AddModelError("", "An error occurred while deleting the order: " + ex.Message);
                return RedirectToAction("Index");
            }
            // Chuyển hướng về trang Index sau khi xóa thành công
            return RedirectToAction("Index");
        }
    }
}
