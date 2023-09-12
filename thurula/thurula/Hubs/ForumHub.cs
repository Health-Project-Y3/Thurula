using Microsoft.AspNetCore.SignalR;

namespace thurula.Hubs;

public sealed class ForumHub : Hub<IChatClient>
{
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("User connected");
        await Clients.All.ReceiveMessage($"UserConnected {Context.ConnectionId} has joined");
    }

    public async Task SendMessage(string message)
    {
        Console.WriteLine("Message received");
        await Clients.All.ReceiveMessage($"{Context.ConnectionId}: {message}");

    }

}