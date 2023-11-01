using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Authorize]
[Route("api/feeding")]
[ApiController]
public class FeedingController : ControllerBase
{
    private readonly IFeedingService _feedingService;

    public FeedingController(IFeedingService feedingService)
    {
        _feedingService = feedingService;
    }

    [HttpGet("{id}", Name = "GetFeeding")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeedingTimes))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<FeedingTimes> GetFeeding(string id)
    {
        try
        {
            var feeding = _feedingService.Get(id);
            return Ok(feeding);
        } catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("baby/{id}", Name = "GetBabyFeedings")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FeedingTimes>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<FeedingTimes>> Get(string id)
    {
        try
        {
            var feedingTimes = _feedingService.GetBabyFeedings(id);
            return Ok(feedingTimes);
        } catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost(Name = "CreateFeeding")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FeedingTimes))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<FeedingTimes> CreateFeeding([FromBody] FeedingTimes feedingTime)
    {
        try
        {
            var fd = _feedingService.Create(feedingTime);
            return CreatedAtRoute("GetFeeding", new { id = fd.Id }, fd);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}", Name = "UpdateFeeding")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FeedingTimes))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<FeedingTimes> UpdateFeeding(string id, [FromBody] FeedingTimes feedingTime)
    {
        try
        {
            _feedingService.Update(id, feedingTime);
            return CreatedAtRoute("GetFeeding", new { id = feedingTime.Id }, feedingTime);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}", Name = "DeleteFeeding")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult DeleteFeeding(string id)
    {
        try
        {
            _feedingService.Remove(id);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}", Name = "PatchFeeding")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeedingTimes))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<FeedingTimes> PatchFeeding(string id, [FromBody] JsonPatchDocument<FeedingTimes> patchDoc)
    {
        try
        {
            var feeding = _feedingService.Get(id);
            patchDoc.ApplyTo(feeding);
            _feedingService.Update(id, feeding);
            return Ok(feeding);
        } catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

}