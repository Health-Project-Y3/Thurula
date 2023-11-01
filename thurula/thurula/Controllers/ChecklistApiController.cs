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

    [HttpGet]
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

    [HttpGet("newborns", Name = "GetNewbornsList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Checklist>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Checklist>> GetNewbornsList()
    {
        try
        {
            var newbornsList = _checklistService.GetAllNewborns(); 
            if (newbornsList != null && newbornsList.Any())
            {
                return Ok(newbornsList);
            }
            else
            {
                return NotFound("here");
            }
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpGet("week2", Name = "GetWeek2List")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Checklist>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Checklist>> GetWeek2List()
    {
        try
        {
            var week2List = _checklistService.GetAllWeek2(); 
            if (week2List != null && week2List.Any())
            {
                return Ok(week2List);
            }
            else
            {
                return NotFound("here");
            }
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("week3", Name = "GetWeek3List")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Checklist>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Checklist>> GetWeek3List()
    {
        try
        {
            var week3List = _checklistService.GetAllWeek3(); 
            if (week3List != null && week3List.Any())
            {
                return Ok(week3List);
            }
            else
            {
                return NotFound("here");
            }
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("month1", Name = "GetMonth1List")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Checklist>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Checklist>> GetMonth1List()
    {
        try
        {
            var month1List = _checklistService.GetAllMonth1(); 
            if (month1List != null && month1List.Any())
            {
                return Ok(month1List);
            }
            else
            {
                return NotFound("here");
            }
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

     [HttpGet("week5", Name = "GetWeek5List")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Checklist>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Checklist>> GetWeek5List()
    {
        try
        {
            var week5List = _checklistService.GetAllWeek5(); 
            if (week5List != null && week5List.Any())
            {
                return Ok(week5List);
            }
            else
            {
                return NotFound("here");
            }
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("week6", Name = "GetWeek6List")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Checklist>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Checklist>> GetWeek6List()
    {
        try
        {
            var week6List = _checklistService.GetAllWeek6(); 
            if (week6List != null && week6List.Any())
            {
                return Ok(week6List);
            }
            else
            {
                return NotFound("here");
            }
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("week7", Name = "GetWeek7List")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Checklist>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Checklist>> GetWeek7List()
    {
        try
        {
            var week7List = _checklistService.GetAllWeek7(); 
            if (week7List != null && week7List.Any())
            {
                return Ok(week7List);
            }
            else
            {
                return NotFound("here");
            }
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

     [HttpGet("month2", Name = "GetMonth2List")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Checklist>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Checklist>> GetMonth2List()
    {
        try
        {
            var month2List = _checklistService.GetAllMonth2(); 
            if (month2List != null && month2List.Any())
            {
                return Ok(month2List);
            }
            else
            {
                return NotFound("here");
            }
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

     [HttpGet("week9", Name = "GetWeek9List")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Checklist>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Checklist>> GetWeek9List()
    {
        try
        {
            var week9List = _checklistService.GetAllWeek9(); 
            if (week9List != null && week9List.Any())
            {
                return Ok(week9List);
            }
            else
            {
                return NotFound("here");
            }
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

     [HttpGet("week10", Name = "GetWeek10List")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Checklist>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Checklist>> GetWeek10List()
    {
        try
        {
            var week10List = _checklistService.GetAllWeek10(); 
            if (week10List != null && week10List.Any())
            {
                return Ok(week10List);
            }
            else
            {
                return NotFound("here");
            }
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

     [HttpGet("week11", Name = "GetWeek11List")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Checklist>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Checklist>> GetWeek11List()
    {
        try
        {
            var week11List = _checklistService.GetAllWeek11(); 
            if (week11List != null && week11List.Any())
            {
                return Ok(week11List);
            }
            else
            {
                return NotFound("here");
            }
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }



    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Checklist))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Checklist> CreateChecklist([FromBody] Checklist checklist)
    {
        _checklistService.Create(checklist);
        return CreatedAtRoute("GetChecklist", new { id = checklist.Id }, checklist);
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
