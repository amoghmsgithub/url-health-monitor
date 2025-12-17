using Microsoft.AspNetCore.Mvc;
using UrlHealthMonitor.Data;
using UrlHealthMonitor.Models;

namespace UrlHealthMonitor.Controllers
{
    public class MonitoredUrlsController : Controller
    {
        private readonly AppDbContext _context;

        public MonitoredUrlsController(AppDbContext context)
        {
            _context = context;
        }

        // ===================== INDEX =====================
        // GET: /MonitoredUrls
        public IActionResult Index()
        {
            var urls = _context.MonitoredUrls.ToList();
            return View(urls);
        }

        // ===================== CREATE =====================
        // GET: /MonitoredUrls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /MonitoredUrls/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MonitoredUrl monitoredUrl)
        {
            if (ModelState.IsValid)
            {
                monitoredUrl.Health = "Unknown";
                monitoredUrl.LastUpdated = DateTime.UtcNow;

                _context.MonitoredUrls.Add(monitoredUrl);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(monitoredUrl);
        }

        // ===================== EDIT =====================
        // GET: /MonitoredUrls/Edit/5
        public IActionResult Edit(int id)
        {
            var url = _context.MonitoredUrls.Find(id);
            if (url == null)
            {
                return NotFound();
            }

            return View(url);
        }

        // POST: /MonitoredUrls/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MonitoredUrl monitoredUrl)
        {
            if (ModelState.IsValid)
            {
                _context.MonitoredUrls.Update(monitoredUrl);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(monitoredUrl);
        }

        // ===================== DELETE =====================
        // GET: /MonitoredUrls/Delete/5
        public IActionResult Delete(int id)
        {
            var url = _context.MonitoredUrls.Find(id);
            if (url == null)
            {
                return NotFound();
            }

            return View(url);
        }

        // POST: /MonitoredUrls/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var url = _context.MonitoredUrls.Find(id);
            if (url != null)
            {
                _context.MonitoredUrls.Remove(url);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

