using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/checklist")]
[ApiController]
public class ChecklistApiController : ControllerBase
{
    private readonly IChecklistService _checklistService;

    public ChecklistApiController(IChecklistService checklistService)
    {
        _checklistService = checklistService;
    }
    
    [HttpGet, Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Checklist>))]
    public ActionResult<IEnumerable<Checklist>> Get() =>
        Ok(_checklistService.Get());

    [HttpGet("{id}", Name = "GetChecklist")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Checklist))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Checklist> GetChecklist(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }
        try
        {
            var checklist = _checklistService.Get(id);
            return Ok(checklist);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Checklist))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Checklist> CreateChecklist([FromBody] Checklist checklist)
    {
        _checklistService.Create(checklist);
        return CreatedAtRoute("GetChecklist", new {id = checklist.Id}, checklist);
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
