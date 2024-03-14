namespace CityInfo.Services;

public interface IMailService
{
    void Send(string subject, string body);
}

public class LocalMailService(IConfiguration config) : IMailService
{
    private readonly string _mailTo = config["mailSettings:mailToAddress"] ?? "to@mail.io";
    private readonly string _mailFrom = config["mailSettings:mailFromAddress"] ?? "from@mail.io";

    public void Send(string subject, string body)
    {
        Console.WriteLine($"Mailing with {nameof(LocalMailService)} from {_mailFrom} to {_mailTo}.");
        Console.WriteLine($"Subject:{subject}");
        Console.WriteLine($"Body: {body}");
    }
}

public class CloudMailService : IMailService
{
    private const string _mailTo = "admin@mycompany.com";
    private const string _mailFrom = "customer@overthere.com";

    public void Send(string subject, string body)
    {
        Console.WriteLine($"Mailing with {nameof(CloudMailService)} from {_mailFrom} to {_mailTo}.");
        Console.WriteLine($"Subject:{subject}");
        Console.WriteLine($"Body: {body}");
    }
}
