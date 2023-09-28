using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/drinking_tracker")]
[ApiController]
public class UserDrinkingController : ControllerBase
{
    private readonly IUserDrinkingService _userDrinkingService;

    public UserDrinkingController(IUserDrinkingService userDrinkingService)
    {
        _userDrinkingService = userDrinkingService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDrinking>))]
    public ActionResult<IEnumerable<UserDrinking>> Get() =>
        Ok(_userDrinkingService.Get());

    [HttpGet("{id}", Name = "GetDrinking")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDrinking))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserDrinking> GetDrinkingRecord(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        try
        {
            var uwr = _userDrinkingService.Get(id);
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
    public ActionResult<List<UserDrinking>> GetDrinkingRecords(string id, string startDate="", string endDate="")
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
            List<UserDrinking> exerciseRecords = _userDrinkingService.GetByUser(id, startDateTime, endDateTime);

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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDrinking))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserDrinking> CreateDrinkingRecord([FromBody] UserDrinking uwr)
    {
        try
        {
            _userDrinkingService.Create(uwr);
            return CreatedAtRoute("GetDrinking", new { id = uwr.Id }, uwr);
        } catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateDrinkingRecord(string id, [FromBody] UserDrinking uwr)
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

        _userDrinkingService.Update(id, uwr);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteDrinkingRecord(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        var uwr = _userDrinkingService.Get(id);
        if (uwr == null)
        {
            return NotFound();
        }

        _userDrinkingService.Remove(id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PatchDrinkingRecord(string id, [FromBody] JsonPatchDocument<UserDrinking> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var uwr = _userDrinkingService.Get(id);
        if (uwr == null)
        {
            return NotFound();
        }

        patchDoc.ApplyTo(uwr, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _userDrinkingService.Update(id, uwr);
        return NoContent();
    }
}