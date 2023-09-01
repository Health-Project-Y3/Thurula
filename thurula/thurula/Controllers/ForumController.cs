using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using thurula.Hubs;

namespace thurula.Controllers;

[ApiController]
[Route("[controller]")]
public class ForumController : ControllerBase
{
    private readonly IHubContext<ForumHub, IChatClient> _hubContext;

    public ForumController(IHubContext<ForumHub, IChatClient> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("broadcast")]
    public async Task<IActionResult> BroadcastMessage([FromBody] string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            await _hubContext.Clients.All.ReceiveMessage(message);
            return NoContent();
        }
        else
        {
            return BadRequest("Message cannot be empty.");
        }
    }
    
}