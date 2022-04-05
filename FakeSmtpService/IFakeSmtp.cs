using FakeSmtpService.Models;

namespace FakeSmtpService;

public interface IFakeSmtp
{
    void StartSmtpServer();

    void StopSmtpServer();

    void ClearItems();
    
    List<Email> GetReceivedEmails();

    List<Email> GetReceivedEmails(int pageSize, int pageNumber);

    Email GetEmailById(int id, bool withoutRawData = false);
    int GetPortNumber();
    bool IsSmtpServerOn();
    int GetMaximumLimit();

    bool RunInContainer();
}