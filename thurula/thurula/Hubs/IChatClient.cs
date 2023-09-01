namespace thurula.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(string message);
}