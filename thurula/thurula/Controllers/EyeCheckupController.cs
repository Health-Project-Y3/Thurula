using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/eyecheckup")]
[ApiController]
public class EyeCheckupApiController : ControllerBase
{
    private readonly IEyeCheckupService _eyecheckupService;

    public EyeCheckupApiController(IEyeCheckupService eyecheckupService)
    {
        _eyecheckupService = eyecheckupService;
    }

    [HttpGet, Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EyeCheckup>))]
    public ActionResult<IEnumerable<EyeCheckup>> Get() =>
        Ok(_eyecheckupService.Get());

    [HttpGet("{id}", Name = "GetEyeCheckup")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EyeCheckup))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<EyeCheckup> GetEyeCheckup(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }
        try
        {
            var eyecheckup = _eyecheckupService.GetEyeCheckup(id);
            return Ok(eyecheckup);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    
    [HttpPost(Name = "CreateEyeCheckup")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EyeCheckup))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<EyeCheckup> CreateEyeCheckup([FromBody] EyeCheckup eyecheckup)
    {
        var check = _eyecheckupService.Create(eyecheckup);
        return CreatedAtRoute("GetEyeCheckup", new { id = check.Id }, check);
    }

    // [HttpPut("{id}")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    // public IActionResult UpdateBaby(string id, [FromBody] Baby baby)
    // {
    //     if (baby == null)
    //     {
    //         return BadRequest();
    //     }

    //     if (baby.Id != id)
    //     {
    //         return BadRequest();
    //     }

    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState);
    //     }

    //     _babyService.Update(id, baby);
    //     return NoContent();
    // }

    // [HttpDelete("{id}")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // public IActionResult DeleteBaby(string id)
    // {
    //     if (string.IsNullOrEmpty(id))
    //     {
    //         return BadRequest();
    //     }

    //     var baby = _babyService.Get(id);
    //     if (baby == null)
    //     {
    //         return NotFound();
    //     }

    //     _babyService.Remove(baby);
    //     return NoContent();
    // }

    // [HttpPatch("{id}")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    // public IActionResult PatchBaby(string id, [FromBody] Baby baby)
    // {
    //     if (baby == null)
    //     {
    //         return BadRequest();
    //     }
    //     if (baby.Id != id)
    //     {
    //         return BadRequest();
    //     }
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState);
    //     }

    //     _babyService.Update(id, baby);
    //     return NoContent();
    // }

}
