using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMDT.Models
{
	public class ContactModel
	{
		[Key]
		[Required(ErrorMessage = "Website Name is required")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Contact info is required")]
		public string Description { get; set; }
		[Required(ErrorMessage = "Map is required")]
		public string Map { get; set; }
		[Required(ErrorMessage = "Phone number is required")]
		public string Phone { get; set; }
		[Required(ErrorMessage = "Email is required")]
		public string Email { get; set; }
		public string LogoImg { get; set; }

		[NotMapped]
		[FileExtensions]
		public IFormFile? ImageUpload { get; set; }
	}
}
