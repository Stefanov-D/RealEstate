namespace RealEstate.Application.Models.Admin
{
    public class AdminUserViewModel
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
    }
}
