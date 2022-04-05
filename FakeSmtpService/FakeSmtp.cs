using FakeSmtpService.MessageStore;
using FakeSmtpService.Models;
using netDumbster.smtp;
using Configuration = FakeSmtpService.InternalModels.Configuration;

namespace FakeSmtpService;

public class FakeSmtp : IFakeSmtp
{
    private SimpleSmtpServer _smtpServer;

    private bool _isSmtpServerOn;

    private int _maximumLimit;

    private List<Email> _receivedEmails => _messageRepository.Get().Values.ToList();

    private readonly IMessageRepository _messageRepository;

    private readonly Configuration _configuration;

    private readonly bool _runInContainer;
    
    public FakeSmtp(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
        _runInContainer = IsRunInContainer();
        _configuration = new Configuration(25,1000);
    }

    private static bool IsRunInContainer()
    {
        var environmentVariable = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
        var result = environmentVariable != null && bool.Parse(environmentVariable);
        return result;
        
    }

    public void StartSmtpServer()
    {
        var limit = _configuration.ItemLimit;
        if (_receivedEmails.Count > limit)
        {
            _receivedEmails.RemoveRange(limit - 1, _receivedEmails.Count - limit);
        }

        _smtpServer = SimpleSmtpServer.Start(_configuration.PortNumber);
        _isSmtpServerOn = true;
        _maximumLimit = limit;

        _smtpServer.MessageReceived += SmtpServer_MessageReceived;
    }

    public void StopSmtpServer()
    {
        _smtpServer.MessageReceived -= SmtpServer_MessageReceived;
        _smtpServer.ClearReceivedEmail();
        _smtpServer.Stop();
        _isSmtpServerOn = false;
    }

    public void ClearItems()
    {
        _messageRepository.Truncate();
    }

    public List<Email> GetReceivedEmails()
    {
        return _messageRepository.Get().Values.ToList();
    }

    public List<Email> GetReceivedEmails(int pageSize, int pageNumber)
    {
        return _messageRepository.Get().Values.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
    }

    public Email GetEmailById(int id, bool withoutRawData = false)
    {
        var email = _messageRepository.Get();
        if (email.ContainsKey(id))
            return email[id];

        return null;
    }

    public int GetPortNumber()
    {
        return _configuration.PortNumber;
    }

    public bool IsSmtpServerOn()
    {
        return _isSmtpServerOn;
    }

    public int GetMaximumLimit()
    {
        return _configuration.ItemLimit;
    }

    public bool RunInContainer()
    {
        return _runInContainer;
    }

    public string GetRawDataById(int id)
    {
        var email = _messageRepository.Get();
        if (email.ContainsKey(id))
            return email[id].RawData;

        return null;
    }

    // public static string GetRawDataById(List<Email> emails, int id )
    // {
    //     var count = emails.Count();
    //
    //     if (0 < count && 0 < id && id <= count)
    //     {
    //         return emails[count - id].RawData;
    //     }
    //
    //     return null;
    // }

    public byte[] GetAttachmentBytesById(int emailId, int attachmentId)
    {
        var email = GetEmailById(emailId, true);

        if (email != null)
        {
            var attacment = email.Attachments.FirstOrDefault(a => a.Id == attachmentId);

            if (attacment != null)
            {
                return attacment.ContentStream.ToArray();
            }
        }

        return null;
    }
    
    private void SmtpServer_MessageReceived(object sender, MessageReceivedArgs e)
    {
        if (_receivedEmails.Count == _maximumLimit)
        {
            _receivedEmails.RemoveAt(_receivedEmails.Count - 1);
        }

        var newEmailId = (_receivedEmails.Count == 0) ? 1 : _receivedEmails[0].Id + 1;

        _messageRepository.Insert(e.Message);
        _smtpServer.ClearReceivedEmail();
        //_messageHubContext.Clients.All.newMessage(ReceivedEmails[0]);
    }
}