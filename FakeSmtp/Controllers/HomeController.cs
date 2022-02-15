using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using FakeSmtp.Models;
using FakeSmtp.Repositories;
using Microsoft.AspNetCore.Http;

namespace FakeSmtp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Settings()
        {
            ViewBag.Port = Startup.SmtpServer.Configuration.Port;
            ViewBag.IsSmtpServerOn = Startup.IsSmtpServerOn;
            ViewBag.MaximumLimit = Startup.MaximumLimit;
            ViewBag.EmailCount = Startup.ReceivedEmails.Count;
            ViewBag.ServerName = Environment.GetEnvironmentVariable("COMPUTERNAME");

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
            MessageRepository.Start(port, limit);
            return RedirectToAction("Settings");
        }

        public IActionResult Stop()
        {
            MessageRepository.Stop();
            return RedirectToAction("Settings");
        }

        public IActionResult Clear()
        {
            MessageRepository.Clear();
            return RedirectToAction("Settings");
        }

        public IActionResult Download(int id, int attachmentId)
        {
            var email = MessageRepository.GetEmailById(id);

            var attacment = email.Attachments.First(a => a.Id == attachmentId);

            byte[] fileBytes = attacment.ContentStream.ToArray();
            string fileName = attacment.Name;

            return File(fileBytes, MediaTypeNames.Application.Octet, fileName);
        }

        public IActionResult Message(int id)
        {
            var model = MessageRepository.GetEmailById(id);

            return View(model);
        }

        public IActionResult TestEmail()
        {
            MessageRepository.SendTestEmail();

            var emailId = Startup.ReceivedEmails.Count();

            return RedirectToAction("Message", new { id = emailId });
        }

        public IActionResult TestEmailPlus()
        {
            MessageRepository.SendTestEmailPlus();

            var emailId = Startup.ReceivedEmails.Count();

            return RedirectToAction("Message", new { id = emailId });
        }

        public IActionResult TestEmail500()
        {
            for (var i = 0; i < 500; i++)
            {
                MessageRepository.SendTestEmail();
            }

            return RedirectToAction("Messages", new { pageNumber = 1 });
        }

        private List<Email> GetPagedEmails(int? pageSize, int? pageNumber)
        {
            var session = HttpContext.Session;

            var sessionPageSize = (pageSize ?? session.GetInt32("PageSize") ?? 10);
            var currentPageNumber = (pageNumber ?? session.GetInt32("PageNumber") ?? 1);

            if (sessionPageSize == 0 || Startup.ReceivedEmails.Count() < sessionPageSize * (currentPageNumber - 1))
            {
                currentPageNumber = 1;
            }

            session.SetInt32("PageSize",sessionPageSize);
            session.SetInt32("PageNumber", currentPageNumber);

            ViewBag.PageSize = sessionPageSize;
            ViewBag.PageNumber = currentPageNumber;

            sessionPageSize = (sessionPageSize == 0) ? int.MaxValue : sessionPageSize;

            ViewBag.PageAnchors = MessageRepository.GetPageAnchors(Startup.ReceivedEmails.Count(), sessionPageSize, currentPageNumber);

            var model = MessageRepository.GetReceivedEmails(sessionPageSize, currentPageNumber);

            ViewBag.TotalCount = Startup.ReceivedEmails.Count();
            ViewBag.OnPageCount = model.Count;

            return model;
        }
    }
}
