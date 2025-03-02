using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMDT.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public string Slug { get; set; }

        public int Status { get; set; }

        // Thêm thuộc tính này để tạo quan hệ cha - con
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public CategoryModel ParentCategory { get; set; } // Danh mục cha

        public List<CategoryModel> SubCategories { get; set; } // Danh mục con
    }
}
