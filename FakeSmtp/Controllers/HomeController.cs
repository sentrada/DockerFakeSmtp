using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using FakeSmtp.TestEmailHelper;
using FakeSmtpService;
using FakeSmtpService.Models;

namespace FakeSmtp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFakeSmtp _fakeSmtp;

        public HomeController(IFakeSmtp fakeSmtp)
        {
            _fakeSmtp = fakeSmtp;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Settings()
        {
            ViewBag.Port = _fakeSmtp.GetPortNumber();
            ViewBag.IsSmtpServerOn = _fakeSmtp.IsSmtpServerOn();
            ViewBag.MaximumLimit = _fakeSmtp.GetMaximumLimit();
            ViewBag.EmailCount = _fakeSmtp.GetReceivedEmails().Count;
            ViewBag.ServerName = Dns.GetHostName();
            ViewBag.RunInContainer = _fakeSmtp.RunInContainer(); 

            return View();
        }

        public IActionResult Messages(int? pageSize, int? pageNumber)
        {
            var model = GetPagedEmails(pageSize, pageNumber);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Headers(int? pageSize, int? pageNumber)
        {
            var model = GetPagedEmails(pageSize, pageNumber);

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Start(int? port, int? limit)
        {
            _fakeSmtp.StartSmtpServer();
            return RedirectToAction("Settings");
        }

        public IActionResult Stop()
        {
            _fakeSmtp.StartSmtpServer();
            return RedirectToAction("Settings");
        }

        public IActionResult Clear()
        {
            _fakeSmtp.ClearItems();
            return RedirectToAction("Settings");
        }

        public IActionResult Download(int id, int attachmentId)
        {
            var email = _fakeSmtp.GetEmailById(id);

            var attacment = email.Attachments.First(a => a.Id == attachmentId);

            byte[] fileBytes = attacment.ContentStream.ToArray();
            string fileName = attacment.Name;

            return File(fileBytes, MediaTypeNames.Application.Octet, fileName);
        }

        public IActionResult Message(int id)
        {
            var model = _fakeSmtp.GetEmailById(id);

            return View(model);
        }

        public IActionResult TestEmail()
        {
            EmailHelper.SendTestEmail();

            var emailId = _fakeSmtp.GetReceivedEmails().Count();

            return RedirectToAction("Message", new { id = emailId });
        }

        public IActionResult TestEmailPlus()
        {
            EmailHelper.SendTestEmailPlus();

            var emailId = _fakeSmtp.GetReceivedEmails().Count();

            return RedirectToAction("Message", new { id = emailId });
        }

        public IActionResult TestEmail500()
        {
            for (var i = 0; i < 500; i++)
            {
                EmailHelper.SendTestEmail();
            }

            return RedirectToAction("Messages", new { pageNumber = 1 });
        }

        private List<Email> GetPagedEmails(int? pageSize, int? pageNumber)
        {
            var session = HttpContext.Session;

            var sessionPageSize = (pageSize ?? session.GetInt32("PageSize") ?? 10);
            var currentPageNumber = (pageNumber ?? session.GetInt32("PageNumber") ?? 1);

            if (sessionPageSize == 0 || _fakeSmtp.GetReceivedEmails().Count() < sessionPageSize * (currentPageNumber - 1))
            {
                currentPageNumber = 1;
            }

            session.SetInt32("PageSize",sessionPageSize);
            session.SetInt32("PageNumber", currentPageNumber);

            ViewBag.PageSize = sessionPageSize;
            ViewBag.PageNumber = currentPageNumber;

            sessionPageSize = (sessionPageSize == 0) ? int.MaxValue : sessionPageSize;

            ViewBag.PageAnchors = EmailHelper.GetPageAnchors(_fakeSmtp.GetReceivedEmails().Count(), sessionPageSize, currentPageNumber);

            var model = _fakeSmtp.GetReceivedEmails(sessionPageSize, currentPageNumber);

            ViewBag.TotalCount = _fakeSmtp.GetReceivedEmails().Count();
            ViewBag.OnPageCount = model.Count;

            return model;
        }
    }
}
