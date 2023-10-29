using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SPSApps.Models.Register
{
    public class User : BaseModel<int>
    {
        public string Name { get; set; } = "Unknown";
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

    }
}
