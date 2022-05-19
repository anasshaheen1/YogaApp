namespace YogaApp.Services;

public class AuthMessageSenderOptions
{
    public string? SendGridKey { get; set; }
    public string? FromAddress { get; set; }
    public string? FromName { get; set; }

    public AuthMessageSenderOptions() { }

    public AuthMessageSenderOptions(string? SendGridKey, string? FromAddress, string? FromName)
    {
        this.SendGridKey = SendGridKey;
        this.FromAddress = FromAddress;
        this.FromName = FromName;
        
    }
}