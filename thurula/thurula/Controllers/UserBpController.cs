using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/bp_tracker")]
[ApiController]
public class UserBpController : ControllerBase
{
    private readonly IUserBpService _userBpService;

    public UserBpController(IUserBpService userBpService)
    {
        _userBpService = userBpService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserBp>))]
    public ActionResult<IEnumerable<UserBp>> Get() =>
        Ok(_userBpService.Get());

    [HttpGet("{id}", Name = "GetBp")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserBp))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserBp> GetBpRecord(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        try
        {
            var uwr = _userBpService.Get(id);
            if (uwr is not null) return Ok(uwr);
            return NotFound();
        } catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpGet("user/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<List<UserBp>> GetBpRecords(string id, string startDate="", string endDate="")
    {
        try
        {
            // Parse the startDate and endDate strings to DateTime objects
            DateTime? startDateTime = null;
            DateTime? endDateTime = null;

            if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var parsedStartDate))
            {
                startDateTime = parsedStartDate;
            }

            if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var parsedEndDate))
            {
                endDateTime = parsedEndDate;
            }
            //if startDate and endDate are both empty, then return all records for the specified user
            if (startDate == "" && endDate == "")
            {
                startDateTime = DateTime.MinValue;
                endDateTime = DateTime.MaxValue;
            }

            // Call the GetByUser function with the provided parameters
            List<UserBp> exerciseRecords = _userBpService.GetByUser(id, startDateTime, endDateTime);

            if (exerciseRecords.Count > 0)
            {
                return Ok(exerciseRecords);
            } else
            {
                return NotFound("No weight drinking found for the specified user and date range.");
            }
        } catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserBp))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserBp> CreateBpRecord([FromBody] UserBp uwr)
    {
        try
        {
            _userBpService.Create(uwr);
            return CreatedAtRoute("GetBp", new { id = uwr.Id }, uwr);
        } catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateBpRecord(string id, [FromBody] UserBp uwr)
    {
        if (uwr == null)
        {
            return BadRequest();
        }

        if (uwr.Id != id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _userBpService.Update(id, uwr);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteBpRecord(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        var uwr = _userBpService.Get(id);
        if (uwr == null)
        {
            return NotFound();
        }

        _userBpService.Remove(id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PatchBpRecord(string id, [FromBody] JsonPatchDocument<UserBp> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var uwr = _userBpService.Get(id);
        if (uwr == null)
        {
            return NotFound();
        }

        patchDoc.ApplyTo(uwr, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _userBpService.Update(id, uwr);
        return NoContent();
    }

    [HttpGet("user/{id}/current")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserBp> GetCurrentUserBp(string id)
    {
        try
        {
            UserBp userBp = _userBpService.GetCurrentUserBp(id);
            if (userBp is not null) return Ok(userBp);
            return NotFound();
        } catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
}