using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/weight_tracker")]
[ApiController]
public class UserWeightController : ControllerBase
{
    private readonly IUserWeightService _userWeightService;

    public UserWeightController(IUserWeightService userWeightService)
    {
        _userWeightService = userWeightService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserWeight>))]
    public ActionResult<IEnumerable<UserWeight>> Get() =>
        Ok(_userWeightService.Get());

    [HttpGet("{id}", Name = "GetWeight")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserWeight))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserWeight> GetWeightRecord(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        try
        {
            var uwr = _userWeightService.Get(id);
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
    public ActionResult<List<UserWeight>> GetWeightRecords(string id, string startDate="", string endDate="")
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

            // Call the GetByUser function with the provided parameters
            List<UserWeight> exerciseRecords = _userWeightService.GetByUser(id, startDateTime, endDateTime);

            if (exerciseRecords.Count > 0)
            {
                return Ok(exerciseRecords);
            } else
            {
                return NotFound("No weight records found for the specified user and date range.");
            }
        } catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserWeight))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserWeight> CreateWeightRecord([FromBody] UserWeight uwr)
    {
        try
        {
            _userWeightService.Create(uwr);
            return CreatedAtRoute("GetWeight", new { id = uwr.Id }, uwr);
        } catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateWeightRecord(string id, [FromBody] UserWeight uwr)
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

        _userWeightService.Update(id, uwr);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteWeightRecord(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        var uwr = _userWeightService.Get(id);
        if (uwr == null)
        {
            return NotFound();
        }

        _userWeightService.Remove(id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PatchWeightRecord(string id, [FromBody] JsonPatchDocument<UserWeight> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var uwr = _userWeightService.Get(id);
        if (uwr == null)
        {
            return NotFound();
        }

        patchDoc.ApplyTo(uwr, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _userWeightService.Update(id, uwr);
        return NoContent();
    }
}