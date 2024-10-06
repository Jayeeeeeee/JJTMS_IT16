using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JJTMS_IT16.Models
{
    public class EditUserRoleModel
    {
        [Required] public required string UserId { get; set; }
        [Required] public required string Email { get; set; }
        [Required] public required IEnumerable<IdentityRole> Roles { get; set; }
        public required string SelectedRole { get; set; }
    }
}
