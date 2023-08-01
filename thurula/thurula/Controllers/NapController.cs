using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/naps")]
[ApiController]
public class NapController : ControllerBase
{
    private readonly INapService _napService;

    public NapController(INapService napService)
    {
        _napService = napService;
    }

    [HttpGet("getnap", Name = "GetNap")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NapTimes))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<NapTimes> GetNap(string napId)
    {
        try
        {
            var nap = _napService.Get(napId);
            return Ok(nap);
        } catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("getbabynaps", Name = "GetBabyNaps")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NapTimes>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<NapTimes>> Get(string id)
    {
        try
        {
            var napTimes = _napService.GetBabyNaps(id);
            return Ok(napTimes);
        } catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpPost("create", Name = "CreateNap")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NapTimes))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<NapTimes> CreateNap([FromBody] NapTimes napTime)
    {
        try
        {
            var np = _napService.Create(napTime);
            return CreatedAtRoute("GetNap", new { id = np.Id }, np);
        } catch (Exception)
        {
            return BadRequest();
        }
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NapTimes))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("updatenap", Name = "UpdateNap")]
    public ActionResult<NapTimes> UpdateNap([FromBody] NapTimes napIn)
    {
        try
        {
            _napService.Update(napIn);
            return CreatedAtRoute("GetNap", new { id = napIn.Id }, napIn);
        } catch (Exception)
        {
            return BadRequest();
        }
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("deletenap", Name = "DeleteNap")]
    public ActionResult DeleteNap([FromBody] string napId)
    {
        try
        {
            _napService.Remove(_napService.Get(napId));
            return NoContent();
        } catch (Exception)
        {
            return BadRequest();
        }
    }


    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NapTimes))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPatch("patchnap", Name = "PatchNap")]
    public ActionResult<NapTimes> PatchNap(string id, [FromBody] JsonPatchDocument<NapTimes> patchDoc)
    {
        try
        {
            var nap = _napService.Get(id);
            patchDoc.ApplyTo(nap, ModelState);
            _napService.Update(nap);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(nap);
        } catch (Exception)
        {
            return NotFound();
        }
    }


    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NapTimes))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("startnap", Name = "StartNap")]
    public ActionResult<NapTimes> StartNap([FromBody] string babyId)
    {
        try
        {
            var nap = _napService.StartNap(babyId);
            return CreatedAtRoute("GetNap", new { id = nap.Id }, nap);
        } catch (Exception)
        {
            return BadRequest();
        }
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NapTimes))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("endnap", Name = "EndNap")]
    public ActionResult<NapTimes> EndNap([FromBody] string napId)
    {
        try
        {
            var nap = _napService.EndNap(napId);
            return CreatedAtRoute("GetNap", new { id = nap.Id }, nap);
        } catch (Exception)
        {
            return BadRequest();
        }
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NapTimes))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("ratenap", Name = "RateNap")]
    public ActionResult<NapTimes> RateNap([FromBody] NapTimes napIn)
    {
        try
        {
            var nap = _napService.RateNap(napIn.Id, napIn.SleepQuality);
            return CreatedAtRoute("GetNap", new { id = nap.Id }, nap);
        } catch (Exception)
        {
            return BadRequest();
        }
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NapTimes))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("addsleepnotes", Name = "AddSleepNotes")]
    public ActionResult<NapTimes> AddSleepNotes([FromBody] NapTimes napIn)
    {
        try
        {
            var nap = _napService.AddSleepNotes(napIn.Id, napIn.SleepNotes);
            return CreatedAtRoute("GetNap", new { id = nap.Id }, nap);
        } catch (Exception)
        {
            return BadRequest();
        }
    }
}