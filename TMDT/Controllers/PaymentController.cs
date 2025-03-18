using Microsoft.AspNetCore.Mvc;
using TMDT.Models;
using TMDT.Models.VNPay;
using TMDT.Service.Momo;
using TMDT.Service.VNPay;

namespace TMDT.Controllers
{
    public class PaymentController : Controller
    {
        private IMomoService _momoService;
		private readonly IVNPayService _vnPayService;
		public PaymentController(IMomoService momoService, IVNPayService vnPayService)
		{
			_momoService = momoService;
			_vnPayService = vnPayService;
		}

		[HttpPost]
		public async Task<IActionResult> CreatePaymentMomo(OrderInfoModel model)
		{
			var response = await _momoService.CreatePaymentMomo(model);
			return Redirect(response.PayUrl);
		}

		[HttpGet]
		public IActionResult PaymentCallBackMomo()
		{
			var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
			return View(response);
		}

		[HttpPost]
		public IActionResult CreatePaymentUrlVNpay(PaymentInfoModel model)
		{
			var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
			return Redirect(url);
		}
	}
}
