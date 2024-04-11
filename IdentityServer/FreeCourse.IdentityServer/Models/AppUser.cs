using Microsoft.AspNetCore.Identity;

namespace FreeCourse.IdentityServer.Models
{
    public class AppUser:IdentityUser
    {
        public string City{ get; set; }
    }
}
