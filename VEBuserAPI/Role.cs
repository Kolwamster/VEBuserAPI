using System.ComponentModel.DataAnnotations;

namespace VEBuserAPI
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
