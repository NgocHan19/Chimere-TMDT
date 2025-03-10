using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMDT.Areas.Admin.Repository.Validation;

namespace TMDT.Models
{
	public class ProductModel
    {
		[Key]
		public long Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Name is required")]
		public string Name { get; set; }
		public string Slug { get; set; }
		[Required, MinLength(4, ErrorMessage = "Description is required")]
		public string Description { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue)]
		[Column(TypeName = "decimal(8, 2)")]
		public decimal Price { get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage = "Choose one Brand")]
		public int BrandId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Choose one Category")]
        public int Quantity { get; set; }
        public int Sold { get; set; }
		public BrandModel Brand { get; set; }
		public string Image { get; set; }
        public string? Component { get; set; }
        public string? Inspiration { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int Capacity { get; set; }
        public RatingModel Ratings { get; set; }

		[NotMapped]
		[FileExtension]
		public IFormFile? ImageUpload { get; set; }

		public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }

        // Mối quan hệ với SubCategory (Danh mục con)
        public int? SubCategoryId { get; set; }  // Có thể là null nếu không có danh mục con
        [ForeignKey("SubCategoryId")]
        public CategoryModel SubCategory { get; set; } // Danh mục con
    }
}
