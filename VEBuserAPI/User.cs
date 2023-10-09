using System.ComponentModel.DataAnnotations;

namespace VEBuserAPI
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Email { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
