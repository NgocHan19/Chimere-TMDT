using System.ComponentModel.DataAnnotations;

namespace TMDT.Models.ViewModels
{
	public class ProductDetailsViewModel
	{
		public ProductModel ProductDetails { get; set; }
        public List<IGrouping<int, GroupedProduct>> RelatedProductsGrouped { get; set; }

		[Required(ErrorMessage = "Comment is required")]
		public string Comment { get; set; }
		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Email is required")]
		public string Email { get; set; }
	}
}
