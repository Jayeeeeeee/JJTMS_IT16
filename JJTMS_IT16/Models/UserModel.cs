using Microsoft.AspNetCore.Identity;

namespace JJTMS_IT16.Models
{
    public class UserModel : IdentityUser
    {
        // Additional properties for user can go here
        public string FullName { get; set; }
    }
}
