using System.IO;

namespace FakeSmtp.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MemoryStream ContentStream { get; set; }
        public int Capacity { get; set; }
        public string Size { get; set; }

        public void SetSize(int capacity)
        {
            Capacity = capacity;

            if (capacity < 1024)
                Size = capacity + " B";
            else if (capacity < 1024 * 1024)
                Size = (capacity / 1024) + " КB";
            else
                Size = (capacity / (1024 * 1024)) + " MB";
        }

    }
}