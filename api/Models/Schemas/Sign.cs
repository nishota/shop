
using api.Models.Settings;

namespace api.Models.Schemas
{
    public class Sign
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = RoleName.Customer;

        public bool HasData => Email != null && Password != null && Role != null;
    }
}
