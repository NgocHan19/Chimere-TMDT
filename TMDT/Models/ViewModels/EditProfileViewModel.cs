﻿using System.ComponentModel.DataAnnotations;
namespace TMDT.Models.ViewModel
{
    public class EditProfileViewModel
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
