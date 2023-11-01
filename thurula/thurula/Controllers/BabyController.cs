using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Authorize]
[Route("api/baby")]
[ApiController]
public class BabyController : ControllerBase
{
    private readonly IBabyService _babyService;

    public BabyController(IBabyService babyService)
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
        } catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Baby))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Baby> CreateBaby([FromBody] Baby baby)
    {
        try
        {
            _babyService.Create(baby);
            return CreatedAtRoute("GetBaby", new { id = baby.Id }, baby);
        } catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PatchBaby(string id, [FromBody] JsonPatchDocument<Baby> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var baby = _babyService.Get(id);
        if (baby == null)
        {
            return NotFound();
        }

        patchDoc.ApplyTo(baby, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _babyService.Update(id, baby);
        return NoContent();
    }
}