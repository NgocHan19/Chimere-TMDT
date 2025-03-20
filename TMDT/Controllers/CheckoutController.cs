using Microsoft.AspNetCore.Mvc;
using TMDT.Models.Momo;
using TMDT.Models;
using System.Security.Claims;
using TMDT.Service.Momo;
using Microsoft.AspNetCore.Identity.UI.Services;
using TMDT.Repository;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TMDT.Service.VNPay;

namespace TMDT.Controllers
{
    public class CheckoutController : Controller
    {
		private readonly DataContext _dataContext;
		public readonly IEmailSender _emailSender;
		private static readonly HttpClient client = new HttpClient();
		private IMomoService _momoService;
		private readonly IVNPayService _vnPayService;

		public CheckoutController(IEmailSender emailSender, DataContext dataContext, IMomoService momoService, IVNPayService vnPayService)
		{
			_dataContext = dataContext;
			_emailSender = emailSender;
			_momoService = momoService;
			_vnPayService = vnPayService;
		}

		public IActionResult Index()
        {
            return View();
        }

		public async Task<IActionResult> Checkout(string PaymentMethod, string PaymentId)
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if(userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var orderCode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
				var shippingPriceCookie = Request.Cookies["shippingPrice"];
				int shippingPrice = 0;
				if (shippingPriceCookie != null)
				{
					var shippingPriceJson = shippingPriceCookie;
					shippingPrice = JsonConvert.DeserializeObject<int>(shippingPriceJson);
				}
				else
				{
					shippingPrice = 0;
				}
				orderItem.ShippingCost = shippingPrice;
				orderItem.UserName = userEmail;
				orderItem.PaymentMethod = PaymentMethod + " " + PaymentId;
				if (PaymentMethod != "VNPay" || PaymentMethod != "Momo")
				{
					orderItem.PaymentMethod = "COD";
				}
				else if (PaymentMethod == "VNPay")
				{
					orderItem.PaymentMethod = "VNPay" + " " + PaymentId;
				}
				else
				{
					orderItem.PaymentMethod = "Momo" + " " + PaymentMethod;
				}
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;
				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();
				//Tạo Order Detail
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
				foreach(var cart in cartItems)
				{
					var orderDetail = new OrderDetails();
					orderDetail.UserName = userEmail;
					orderDetail.OrderCode = orderCode;
					orderDetail.ProductId = cart.ProductId;
					orderDetail.Price = cart.Price;
					orderDetail.Quantity = cart.Quantity;
					//Update product quantity
					var product = await _dataContext.Products.Where(p => p.Id == cart.ProductId).FirstAsync();
					product.Quantity -= cart.Quantity;
					product.Sold += cart.Quantity;
					_dataContext.Update(product);
					//++Update product quantity
					_dataContext.Add(orderDetail);
					_dataContext.SaveChanges();
				}
				HttpContext.Session.Remove("cart");
				//Send mail
				var reciever = userEmail;
				var subject = "Đặt hàng thành công!";
				var message = "Đặt hàng thành công, chúc bạn có trải nghiệm dịch vụ vui vẻ!";
				await _emailSender.SendEmailAsync(reciever, subject, message);
				TempData["success"] = "Đơn hàng đã được tạo, vui lòng chờ chúng tôi duyệt đơn hàng nhé!";
			}
			return RedirectToAction("History", "Account");
		}


		[HttpGet]
        public async Task<IActionResult> PaymentCallBackMomo(MomoInfoModel model)
		{
			var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            var requestQuery = HttpContext.Request.Query;
            if(requestQuery["resultCode"] != "0")
			{
				var newMomoInsert = new MomoInfoModel
				{
					OrderId = requestQuery["orderId"],
					FullName = User.FindFirstValue(ClaimTypes.Email),
					Amount = int.Parse(requestQuery["amount"]),
					OrderInfo = requestQuery["orderInfo"],
					DatePaid = DateTime.Now
				};
				_dataContext.Add(newMomoInsert);
				await _dataContext.SaveChangesAsync();
				var PaymentMethod = "Momo";
				await Checkout(requestQuery["orderId"], PaymentMethod );
			}
			else
			{
				TempData["success"] = "Đã hủy giao dịch";
				return RedirectToAction("Index", "Cart");
			}
			return View(response);
		}

		[HttpGet]
		public async Task<IActionResult> PaymentCallbackVNpay()
		{
			var response = _vnPayService.PaymentExecute(Request.Query);
			if (response.VNPayResponseCode == "00")
			{
				var newVNPayInsert = new VNPayInfoModel
				{
					OrderId = response.VNPayResponseCode,
					PaymentMethod = response.VNPayResponseCode,
					OrderDescription = response.VNPayResponseCode,
					TransactionId = response.VNPayResponseCode,
					PaymentId = response.VNPayResponseCode,
					DateCreate = DateTime.Now
				};
				_dataContext.Add(newVNPayInsert);
				await _dataContext.SaveChangesAsync();
				//Tiiến hành đặt hàng khi thanh toán VNPay thành công
				var paymentMethod = response.PaymentMethod;
				var paymentId = response.PaymentId;
				await Checkout(paymentMethod, paymentId);
			}
			else
			{
				TempData["success"] = "Đã hủy giao dịch";
				return RedirectToAction("Index", "Cart");
			}
			//return Json(response);
			return View(response);
		}
	}
}
