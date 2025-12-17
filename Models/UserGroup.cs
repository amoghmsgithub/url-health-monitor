using UrlHealthMonitor.Models;

namespace UrlHealthMonitor.Models
{
    public class UserGroup
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
