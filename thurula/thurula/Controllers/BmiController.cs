using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Authorize]
[Route("api/bmi")]
[ApiController]
public class BmiController : ControllerBase
{
    private readonly IBmiService _bmiService;
    private readonly IUserService _userService;

    public BmiController(IUserService userService, IBmiService bmiService)
    {
        _userService = userService;
        _bmiService = bmiService;
    }

    [HttpGet("calculate/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<double> CalculateBmi(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        try
        {
            var user = _userService.Get(id);
            if (user == null) return NotFound("User not found");
            var bmi = _bmiService.CalculateBmi(user.Height, user.Weight);
            return Ok(bmi);
        } catch (Exception e)
        {
            return NotFound(e);
        }
    }

    [HttpGet("checkrange/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<string> CheckRange(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        try
        {
            var user = _userService.Get(id);
            if (user == null) return NotFound("User not found");
            var bmiClass = _bmiService.CheckRange(user);
            return Ok(bmiClass);
        } catch (Exception e)
        {
            return NotFound(e);
        }
    }
}