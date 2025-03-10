using TMDT.Models;
using TMDT.Models.Momo;

namespace TMDT.Service.Momo
{
	public interface IMomoService
	{
		Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfoModel model);
		MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
	}
}
