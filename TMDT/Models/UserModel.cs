﻿using System.ComponentModel.DataAnnotations;

namespace TMDT.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Type Your Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Type Your Email"), EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Please Type Your Password")]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
	}
}