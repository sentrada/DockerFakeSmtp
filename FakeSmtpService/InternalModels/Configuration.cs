namespace FakeSmtpService.InternalModels;

internal class Configuration
{
    internal int PortNumber;

    internal int ItemLimit;

    public Configuration(int portNumber, int itemLimit)
    {
        PortNumber = portNumber;
        ItemLimit = itemLimit;
    }
}