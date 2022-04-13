namespace api.Models.Schemes
{
    public class Sign
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public bool HasData => Email != null && Password != null;
    }
}
