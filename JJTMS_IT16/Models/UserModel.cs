using Microsoft.AspNetCore.Identity;

namespace JJTMS_IT16.Models
{
    public class UserModel : IdentityUser
    {
        // Additional properties for user can go here
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Role { get; set; }
    }
}
