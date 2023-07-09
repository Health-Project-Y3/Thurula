using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/baby")]
[ApiController]
public class BabyApiController : ControllerBase
{
    private readonly IBabyService _babyService;

    public BabyApiController(IBabyService babyService)
    {
        _babyService = babyService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Baby>))]
    public ActionResult<IEnumerable<Baby>> Get() =>
        Ok(_babyService.Get());

    [HttpGet("{id}", Name = "GetBaby")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Baby))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Baby> GetBaby(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }
        try
        {
            var baby = _babyService.Get(id);
            return Ok(baby);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Baby))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Baby> CreateBaby([FromBody] Baby baby)
    {
        _babyService.Create(baby);
        return CreatedAtRoute("GetBaby", new {id = baby.Id}, baby);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    public IActionResult UpdateBaby(string id, [FromBody] Baby baby)
    {
        if (baby == null)
        {
            return BadRequest();
        }

        if (baby.Id != id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _babyService.Update(id, baby);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteBaby(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        var baby = _babyService.Get(id);
        if (baby == null)
        {
            return NotFound();
        }

        _babyService.Remove(baby);
        return NoContent();
    }
    
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    public IActionResult PatchBaby(string id, [FromBody] Baby baby)
    {
        if (baby == null)
        {
            return BadRequest();
        }
        if (baby.Id != id)
        {
            return BadRequest();
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _babyService.Update(id, baby);
        return NoContent();
    }
    
}
