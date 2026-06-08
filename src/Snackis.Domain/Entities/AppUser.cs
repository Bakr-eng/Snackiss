using Microsoft.AspNetCore.Identity;


namespace Snackis.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }

        public string? ProfilePictureUrl { get; set; }
    }
}
