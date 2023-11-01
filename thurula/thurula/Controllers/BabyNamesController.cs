using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BabyNamesController : ControllerBase
{
    private readonly IBabyNameService _babyNameService;
    private readonly IUserService _userService;

    public BabyNamesController(IUserService userService, IBabyNameService babyNameService)
    {
        _userService = userService;
        _babyNameService = babyNameService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BabyNames>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<BabyNames>> GetBabyNames()
    {
        try
        {
            return Ok(_babyNameService.Get());
        } catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BabyNames))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<BabyNames> GetBabyName(string id)
    {
        try
        {
            var babyName = _babyNameService.Get(id);
            return Ok(babyName);
        } catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("gender/{gender}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BabyNames>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<BabyNames>> GetGenderNames(string gender)
    {
        try
        {
            return Ok(_babyNameService.GetByGender(gender));
        } catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("favourites/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BabyNames>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<BabyNames>> GetFavourites(string id)
    {
        try
        {
            return Ok(_userService.GetFavouriteNames(id));
        } catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("favourites/add/{userid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BabyNames>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<List<BabyNames>> AddFavourite(string userid, [FromBody] string nameid)
    {
        try
        {
            _userService.AddFavouriteName(userid, nameid);
            return Ok();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("favourites/remove/{userid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BabyNames>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<List<BabyNames>> RemoveFavourite(string userid, [FromBody] string nameid)
    {
        try
        {
            _userService.RemoveFavouriteName(userid, nameid);
            return Ok();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}