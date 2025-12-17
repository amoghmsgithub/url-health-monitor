using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UrlHealthMonitor.Data;
using System.Net.Http;
using System.Net;
using System.Net.Mail;

namespace UrlHealthMonitor.Services
{
    public class UrlHealthCheckerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient = new HttpClient();

        public UrlHealthCheckerService(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var urls = await db.MonitoredUrls.ToListAsync(stoppingToken);

                foreach (var url in urls)
                {
                    var elapsed = DateTime.UtcNow - url.LastUpdated;

                    if (elapsed.TotalSeconds >= url.IntervalSeconds)
                    {
                        try
                        {
                            var response = await _httpClient.GetAsync(url.Url, stoppingToken);

                            if (response.IsSuccessStatusCode)
                            {
                                url.Health = "Healthy";
                                url.DownSince = null;
                            }
                            else
                            {
                                if (url.DownSince == null)
                                {
                                    url.DownSince = DateTime.UtcNow;
                                    SendDownEmail(url);
                                }
                                url.Health = "Down";
                            }
                        }
                        catch
                        {
                            if (url.DownSince == null)
                            {
                                url.DownSince = DateTime.UtcNow;
                                SendDownEmail(url);
                            }
                            url.Health = "Down";
                        }

                        url.LastUpdated = DateTime.UtcNow;
                    }
                }

                await db.SaveChangesAsync(stoppingToken);
                await Task.Delay(5000, stoppingToken);
            }
        }

        // ================= EMAIL ALERT =================
        private void SendDownEmail(Models.MonitoredUrl url)
        {
            var smtp = _configuration.GetSection("SmtpSettings");

            var client = new SmtpClient(smtp["Host"], int.Parse(smtp["Port"]))
            {
                Credentials = new NetworkCredential(
                    smtp["Username"],
                    smtp["Password"]
                ),
                EnableSsl = bool.Parse(smtp["EnableSsl"])
            };

            var mail = new MailMessage
            {
                From = new MailAddress(smtp["From"]),
                Subject = $"🚨 URL DOWN ALERT: {url.Name}",
                Body = $"The monitored URL is DOWN.\n\n" +
                       $"Name: {url.Name}\n" +
                       $"URL: {url.Url}\n" +
                       $"Time: {DateTime.UtcNow} UTC",
                IsBodyHtml = false
            };

            mail.To.Add(smtp["To"]);
            client.Send(mail);
        }
    }
}



