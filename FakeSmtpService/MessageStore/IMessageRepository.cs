using FakeSmtpService.Models;
using netDumbster.smtp;

namespace FakeSmtpService.MessageStore
{
    public interface IMessageRepository
    {
        IDictionary<int, Email> Get();

        void Insert(SmtpMessage email);

        void Delete(Email email);

        void Truncate();
    }

    internal class InMemoryMessageRepository : IMessageRepository
    {
        private readonly Dictionary<int, Email> _emails;

        public InMemoryMessageRepository()
        {
            _emails = new Dictionary<int, Email>();
        }

        public IDictionary<int, Email> Get()
        {
            return _emails;
        }

        public void Insert(SmtpMessage email)
        {
            var id = GetId();
            _emails.Add(id, new Email(email,id));
        }

        public void Delete(Email email)
        {
            _emails.Remove(email.Id);
        }

        public void Truncate()
        {
            _emails.Clear();
        }

        private int GetId()
        {
            if (_emails.Any())
            {
                return _emails.Keys.Max() + 1;
            }

            return 0;
        }
    }
}