using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models.ViewModel
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Please Type Your Username")]
		public string UserName { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Please Type Your Password")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
