using System.ComponentModel.DataAnnotations;
namespace TMDT.Models.ViewModel
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Please Type Your Email")]
		public string Email { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Please Type Your Password")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
