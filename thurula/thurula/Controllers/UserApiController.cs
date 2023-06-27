using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/users")]
[ApiController]
public class UserApiController : ControllerBase
{
    private readonly IUserService _userService;

    public UserApiController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
    public ActionResult<IEnumerable<User>> Get() =>
        Ok(_userService.Get());

    [HttpGet("{id}", Name = "GetUser")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<User> GetUser(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }
        try
        {
            var user = _userService.Get(id);
            return Ok(user);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<User> CreateUser([FromBody] User user)
    {
        _userService.Create(user);
        return CreatedAtRoute("GetUser", new {id = user.Id}, user);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    public IActionResult UpdateUser(string id, [FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest();
        }

        if (user.Id != id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _userService.Update(id, user);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteUser(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        var user = _userService.Get(id);
        if (user == null)
        {
            return NotFound();
        }

        _userService.Remove(user);
        return NoContent();
    }
    
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    public IActionResult PatchUser(string id, [FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest();
        }
        if (user.Id != id)
        {
            return BadRequest();
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _userService.Update(id, user);
        return NoContent();
    }
    
}
