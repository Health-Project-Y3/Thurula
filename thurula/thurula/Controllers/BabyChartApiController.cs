using Microsoft.AspNetCore.Mvc;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/babychart")]
[ApiController]
public class BabyChartApiController : ControllerBase
{
    private readonly IBabyChartService _babyChartService;

    public BabyChartApiController(IBabyChartService babyChartService)
    {
        _babyChartService = babyChartService;
    }

    [HttpPost("length/add")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult AddLength(string id, int month, double length)
    {
        try
        {
            _babyChartService.AddLength(id, month, length);
            return NoContent();
        }
        catch (Exception ex)
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
            _babyChartService.DeleteLength(id, month);
            return NoContent();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPost("length/edit")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult EditLength(string id, int month, double length)
    {
        try
        {
            _babyChartService.EditLength(id, month, length);
            return NoContent();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}