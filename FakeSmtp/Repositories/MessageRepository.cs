// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Net.Mail;
// using System.Reflection;
// using System.Text;
// using FakeSmtpService;
// using Email = FakeSmtpService.Models.Email;
//
// namespace FakeSmtp.Repositories
// {
// 	public class MessageRepository
// 	{
// 		private readonly IFakeSmtp _fakeSmtp;
//
// 		public MessageRepository(IFakeSmtp fakeSmtp)
// 		{
// 			_fakeSmtp = fakeSmtp;
// 		}
//
//
// 		// public List<Email> GetReceivedEmails()
// 		// {
// 		// 	return _fakeSmtp.GetReceivedEmails();
// 		// }
//
// 		// public static List<Email> GetReceivedEmails(int pageSize, int pageNumber)
// 		// {
// 		// 	return Startup.ReceivedEmails.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
// 		// }
//
// 		// public static Email GetEmailById(int id, bool withoutRawData = false )
// 		// {
// 		// 	var emails = Startup.ReceivedEmails;
// 		// 	
// 		// 	var count = emails.Count();
// 		//
// 		// 	if (0 < count && 0 < id && id <= count)
// 		// 	{
// 		// 		return emails[count - id];
// 		// 	}
// 		//
// 		// 	return null;
// 		// }
//
// 		// public static string GetRawDataById(int id)
// 		// {
// 		// 	var count = Startup.SmtpServer.ReceivedEmail.Count();
// 		//
// 		// 	if (0 < count && 0 < id && id <= count)
// 		// 	{
// 		// 		return Startup.SmtpServer.ReceivedEmail[count - id].Data;
// 		// 	}
// 		//
// 		// 	return null;
// 		// }
// 		//
// 		// public static string GetRawDataById(List<Email> emails, int id )
// 		// {
// 		// 	var count = emails.Count();
// 		//
// 		// 	if (0 < count && 0 < id && id <= count)
// 		// 	{
// 		// 		return emails[count - id].RawData;
// 		// 	}
// 		//
// 		// 	return null;
// 		// }
// 		//
// 		// public static byte[] GetAttachmentBytesById(int emailId, int attachmentId)
// 		// {
// 		// 	var email = GetEmailById(emailId, true);
// 		// 	
// 		// 	if (email != null) {
// 		// 		var attacment = email.Attachments.FirstOrDefault(a => a.Id == attachmentId);
// 		//
// 		// 		if (attacment != null)
// 		// 		{
// 		// 			return attacment.ContentStream.ToArray();
// 		// 		}
// 		// 	}
// 		//
// 		// 	return null;
// 		// }
//
// 		// public static void Start(int? port, int? limit)
// 		// {
// 		// 	if (port <= 0) port = 5000;
// 		// 	if (limit <= 0) limit = 1000;
//   //
//   //           Startup.StartSmtpServer(port ?? 5000, limit ?? 1000);
// 		// }
//   //
// 		// public static void Stop()
// 		// {
//   //           Startup.StopSmtpServer();
// 		// }
//   //
//   //
// 		// public static void Clear()
// 		// {
//   //           Startup.ReceivedEmails.Clear();
//   //           Startup.SmtpServer.ClearReceivedEmail();
// 		// }
//
// 	
// 	
// }