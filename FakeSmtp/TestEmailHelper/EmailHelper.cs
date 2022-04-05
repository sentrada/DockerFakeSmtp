using System.Net.Mail;
using System.Reflection;
using System.Text;
using FakeSmtpService;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FakeSmtp.TestEmailHelper;

//TODO: It is a temporary solution
public class EmailHelper
{
    private static Func<IFakeSmtp> _fakeSmtp;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        var fakeSmtp = serviceProvider.GetService<IFakeSmtp>();
        if (fakeSmtp == null)
            throw new NullReferenceException();
        _fakeSmtp = () => fakeSmtp;
    }

    public static bool Status => _fakeSmtp().IsSmtpServerOn();

    public static List<PageAnchor> GetPageAnchors(int count, int pageSize, int currentPageNumber)
    {
        var pageAnchors = new List<PageAnchor>();

        var pageCount = (pageSize == 0) ? 1 : (int)Math.Ceiling((decimal)count / pageSize);


        if (pageCount == 0)
        {
            pageAnchors.Add(new PageAnchor { PageNumber = 1, PageLabel = "1" });
        }

        else if (pageCount < 8)
        {
            for (var i = 1; i <= pageCount; i++)
            {
                pageAnchors.Add(new PageAnchor { PageNumber = i, PageLabel = i.ToString() });
            }
        }
        else
        {
            if (currentPageNumber <= 4)
            {
                for (var i = 1; i <= 5; i++)
                {
                    pageAnchors.Add(new PageAnchor { PageNumber = i, PageLabel = i.ToString() });
                }

                pageAnchors.Add(new PageAnchor { PageNumber = 6, PageLabel = "..." });
                pageAnchors.Add(new PageAnchor { PageNumber = pageCount, PageLabel = "" + pageCount });
            }
            else if (pageCount - currentPageNumber <= 3)
            {
                pageAnchors.Add(new PageAnchor { PageNumber = 1, PageLabel = "1" });
                pageAnchors.Add(new PageAnchor { PageNumber = pageCount - 5, PageLabel = "..." });
                for (var i = pageCount - 4; i <= pageCount; i++)
                {
                    pageAnchors.Add(new PageAnchor { PageNumber = i, PageLabel = "" + i });
                }
            }
            else
            {
                pageAnchors.Add(new PageAnchor { PageNumber = 1, PageLabel = "1" });

                pageAnchors.Add(new PageAnchor { PageNumber = currentPageNumber - 2, PageLabel = "..." });
                pageAnchors.Add(new PageAnchor
                    { PageNumber = currentPageNumber - 1, PageLabel = "" + (currentPageNumber - 1) });
                pageAnchors.Add(new PageAnchor { PageNumber = currentPageNumber, PageLabel = "" + currentPageNumber });
                pageAnchors.Add(new PageAnchor
                    { PageNumber = currentPageNumber + 1, PageLabel = "" + (currentPageNumber + 1) });
                pageAnchors.Add(new PageAnchor { PageNumber = currentPageNumber + 2, PageLabel = "..." });

                pageAnchors.Add(new PageAnchor { PageNumber = pageCount, PageLabel = "" + pageCount });
            }
        }

        return pageAnchors;
    }


    #region [ Test Messages ]

    public static void SendTestEmail()
    {
        var email = CreateTestEmail();

        using (var sc = new SmtpClient { Host = "localhost", Port = _fakeSmtp().GetPortNumber() })
        {
            sc.Send(email);
        }
    }

    public static void SendTestEmailPlus()
    {
        string asmPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)
            .Replace("file:\\", "");

        var email = CreateTestEmail();

        email.Attachments.Add(new System.Net.Mail.Attachment(asmPath + @"\Content\minimal.pdf"));

        using (var sc = new SmtpClient { Host = "localhost", Port = _fakeSmtp().GetPortNumber() })
        {
            sc.Send(email);
        }
    }

    private static MailMessage CreateTestEmail()
    {
        var email = new MailMessage();

        email.BodyEncoding = Encoding.UTF8;

        email.From = new MailAddress("from@mail.com", "Notification from mail.com");

        email.To.Add("to1@mail.com");
        email.To.Add("to2@mail.com");

        email.CC.Add("cc1@mail.com");
        email.CC.Add("cc2@mail.com");

        email.Bcc.Add("bcc1@mail.com");
        email.Bcc.Add("bcc2@mail.com");

        email.Subject = "This is \"Lorem ipsum\" test email";
        email.Body =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce ut pretium velit, at hendrerit mauris. Nam sit amet purus ac diam luctus facilisis.\nNullam sagittis vestibulum orci, pulvinar placerat turpis mollis ut. Phasellus fermentum tempor magna, venenatis congue sem interdum eget.\nUt commodo pellentesque fermentum. Praesent suscipit et augue sit amet pulvinar.\nEtiam augue mi, feugiat nec lobortis sit amet, condimentum a nibh.\nInteger blandit lacus sed lectus venenatis, sit amet consequat felis rutrum.";

        email.IsBodyHtml = false;
        email.Priority = MailPriority.Normal;

        return email;
    }

    #endregion
}

public class PageAnchor
{
    public int PageNumber { get; set; }
    public string PageLabel { get; set; }
}