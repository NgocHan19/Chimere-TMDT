using TMDT.Models.VNPay;
namespace TMDT.Service.VNPay
{
	public interface IVNPayService
	{
		string CreatePaymentUrl(PaymentInfoModel model, HttpContext context);
		PaymentResponseModel PaymentExecute(IQueryCollection collections);
	}
}
