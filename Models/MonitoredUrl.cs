using System;

namespace UrlHealthMonitor.Models
{
    public class MonitoredUrl
    {
        public int Id { get; set; }   // Primary Key

        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public int IntervalSeconds { get; set; } // 30 / 40 / 50

        public string Health { get; set; } = "Unknown";

        public DateTime LastUpdated { get; set; }

        public DateTime? DownSince { get; set; }
    }
}
