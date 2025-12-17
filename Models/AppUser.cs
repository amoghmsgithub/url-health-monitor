namespace UrlHealthMonitor.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
