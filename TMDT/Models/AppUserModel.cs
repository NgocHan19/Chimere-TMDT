using Microsoft.AspNetCore.Identity;

namespace TMDT.Models
{
    public class AppUserModel : IdentityUser
    {
        public string UserName { get; set; } 
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Occupation { get; set; }
        public string RoleId { get; set; }
        public string Token { get; set; }
    }
}
