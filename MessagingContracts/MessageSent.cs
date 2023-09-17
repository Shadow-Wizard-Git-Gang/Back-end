namespace MessagingContracts
{
    public record MessageSent(string From, string To, string Header, string Body);
}