using System.ComponentModel.DataAnnotations;

namespace VEBuserAPI
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required][EmailAddress]
        public string? Email { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
