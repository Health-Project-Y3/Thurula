using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/exercise")]
[ApiController]
public class UserExerciseController : ControllerBase
{
    private readonly IUserExerciseService _userExerciseService;

    public UserExerciseController(IUserExerciseService userExerciseService)
    {
        _userExerciseService = userExerciseService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserExercise>))]
    public ActionResult<IEnumerable<UserExercise>> Get() =>
        Ok(_userExerciseService.Get());

    [HttpGet("{id}", Name = "GetExercise")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserExercise))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserExercise> GetExerciseRecord(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        try
        {
            var uex = _userExerciseService.Get(id);
            if (uex is not null) return Ok(uex);
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
    public ActionResult<List<UserExercise>> GetExerciseRecords(string id, string startDate="", string endDate="")
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
            List<UserExercise> exerciseRecords = _userExerciseService.GetByUser(id, startDateTime, endDateTime);

            if (exerciseRecords.Count > 0)
            {
                return Ok(exerciseRecords);
            } else
            {
                return NotFound("No exercise records found for the specified user and date range.");
            }
        } catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserExercise))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserExercise> CreateExerciseRecord([FromBody] UserExercise uex)
    {
        try
        {
            _userExerciseService.Create(uex);
            return CreatedAtRoute("GetExercise", new { id = uex.Id }, uex);
        } catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateExerciseRecord(string id, [FromBody] UserExercise uex)
    {
        if (uex == null)
        {
            return BadRequest();
        }

        if (uex.Id != id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _userExerciseService.Update(id, uex);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteExerciseRecord(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        var uex = _userExerciseService.Get(id);
        if (uex == null)
        {
            return NotFound();
        }

        _userExerciseService.Remove(id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PatchExerciseRecord(string id, [FromBody] JsonPatchDocument<UserExercise> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var uex = _userExerciseService.Get(id);
        if (uex == null)
        {
            return NotFound();
        }

        patchDoc.ApplyTo(uex, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _userExerciseService.Update(id, uex);
        return NoContent();
    }
}