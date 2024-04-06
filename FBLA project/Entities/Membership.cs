using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace FBLA_project
{
    public class Membership
    {
        [Required]
        public required string MembershipType { get; set; }
        public required Car Car { get; set; }

    }
}
