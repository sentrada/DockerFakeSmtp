// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Net.Mail;
// using netDumbster.smtp;
//
// namespace FakeSmtp.Models
// {
//     public class Email
//     {
//         public int Id { get; set; }
//
//         public string From { get; set; }
//
//         public string To { get; set; }
//
//         public string Cc { get; set; }
//
//         public string Bcc { get; set; }
//
//         public DateTime SentDate { get; set; }
//
//         public string Subject { get; set; }
//
//         public string Body { get; set; }
//
//         public bool IsBodyHtml { get; set; }
//
//         public string Importance { get; set; }
//
//         public List<Attachment> Attachments { get; set; }
//
//         public string RawData { get; set; }
//
//         public Email(SmtpMessage smtpMessage, int index, bool withoutRawData = false)
//         {
//             MailMessage mailMessage = Helpers.MailMessageMimeParser.ParseMessage(new StringReader(smtpMessage.Data));
//
//             Id = index;
//
//             From = mailMessage.From.Address;
//             To = string.Join("; ", mailMessage.To);
//             Cc = string.Join("; ", mailMessage.CC);
//             Bcc = GetBcc(smtpMessage, mailMessage);
//
//             SentDate = DateTime.TryParse(mailMessage.Headers["Date"],out var sentDate) ? sentDate : DateTime.MinValue;
//
//             Subject = smtpMessage.Subject;
//
//             Body = mailMessage.Body;
//
//             RawData = withoutRawData ? "" : smtpMessage.Data;
//
//             IsBodyHtml = mailMessage.IsBodyHtml;
//
//             switch (smtpMessage.Importance)
//             {
//                 case "high":
//                     Importance = "High";
//                     break;
//                 case "low":
//                     Importance = "Low";
//                     break;
//                 default:
//                     Importance = "Normal";
//                     break;
//             }
//
//             Attachments = new List<Attachment>();
//
//             for (var i = 0; i < mailMessage.Attachments.Count; i++)
//             {
//                 var attachment = new Attachment
//                 {
//                     Id = i + 1,
//                     Name = mailMessage.Attachments[i].ContentType.Name,
//                     ContentStream = (MemoryStream)mailMessage.Attachments[i].ContentStream
//                 };
//
//                 attachment.SetSize(attachment.ContentStream.Capacity);
//
//                 Attachments.Add(attachment);
//             }
//         }
//
//         private string GetBcc(SmtpMessage smtpMessage, MailMessage mailMessage)
//         {
//             string[] toArray = mailMessage.To.Select(to => to.Address).ToArray();
//             string[] ccArray = mailMessage.CC.Select(cc => cc.Address).ToArray();
//             var bccList = new List<string>();
//
//             foreach (var to in smtpMessage.ToAddresses)
//             {
//                 if (!toArray.Contains(to.Address) && !ccArray.Contains(to.Address))
//                 {
//                     bccList.Add(to.Address);
//                 }
//             }
//
//             return (bccList.Count == 0) ? null : string.Join("; ", bccList.ToArray());
//         }
//     }
// }