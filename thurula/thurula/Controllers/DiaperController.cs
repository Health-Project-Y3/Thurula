using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/diapers")]
[ApiController]
public class DiaperController : ControllerBase
{
    private readonly IDiaperService _diaperService;

    public DiaperController(IDiaperService diaperService)
    {
        _diaperService = diaperService;
    }

    [HttpGet("{id}", Name = "GetDiaper")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DiaperTimes))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DiaperTimes> GetDiaper(string id)
    {
        try
        {
            var diaper = _diaperService.Get(id);
            return Ok(diaper);
        } catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("baby/{id}", Name = "GetBabyDiapers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DiaperTimes>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<DiaperTimes>> Get(string id)
    {
        try
        {
            var diaperTimes = _diaperService.GetBabyDiapers(id);
            return Ok(diaperTimes);
        } catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost(Name = "CreateDiaper")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DiaperTimes))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<DiaperTimes> CreateDiaper([FromBody] DiaperTimes diaperTime)
    {
        try
        {
            var dp = _diaperService.Create(diaperTime);
            return CreatedAtRoute("GetDiaper", new { id = dp.Id }, dp);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DiaperTimes))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}", Name = "UpdateDiaper")]
    public ActionResult<DiaperTimes> UpdateDiaper(string id, [FromBody] DiaperTimes diaperIn)
    {
        try
        {
            _diaperService.Update(id, diaperIn);
            return CreatedAtRoute("GetDiaper", new { id }, diaperIn);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("{id}", Name = "DeleteDiaper")]
    public ActionResult DeleteDiaper(string id)
    {
        try
        {
            _diaperService.Remove(id);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DiaperTimes))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPatch("{id}", Name = "PatchDiaper")]
    public ActionResult<DiaperTimes> PatchDiaper(string id, [FromBody] JsonPatchDocument<DiaperTimes> patchDoc)
    {
        try
        {
            var diaper = _diaperService.Get(id);
            if (diaper == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(diaper, ModelState);
            _diaperService.Update(id, diaper);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(diaper);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}