using Microsoft.AspNetCore.Identity;

namespace AnimeList.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<FanSubMember> FanSubs { get; set; } = [];
    }
}
