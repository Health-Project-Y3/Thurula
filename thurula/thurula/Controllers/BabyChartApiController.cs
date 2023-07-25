using Microsoft.AspNetCore.Mvc;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/babychart")]
[ApiController]
public class BabyChartApiController : ControllerBase
{
    private readonly IBabyLengthChartService _babyLengthChartService;

    public BabyChartApiController(IBabyLengthChartService babyLengthChartService)
    {
        _babyLengthChartService = babyLengthChartService;
    }

    [HttpPost("length/add")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult AddLength(string id, int month, double length)
    {
        try
        {
            _babyLengthChartService.AddLength(id, month, length);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("length/delete")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult DeleteLength(string id, int month)
    {
        try
        {
            _babyLengthChartService.DeleteLength(id, month);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("length/edit")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult EditLength(string id, int month, double length)
    {
        try
        {
            _babyLengthChartService.EditLength(id, month, length);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("length/get")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetLengths(string id)
    {
        try
        {
            var lengths = _babyLengthChartService.GetLength(id);
            return Ok(lengths);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("length/getreference")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetReferenceLengths(string gender, int percentile)
    {
        try
        {
            var lengths = _babyLengthChartService.GetLengthReferenceData(gender, percentile);
            return Ok(lengths);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}