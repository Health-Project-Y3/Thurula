using Microsoft.AspNetCore.Mvc;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/babychart")]
[ApiController]
public class BabyChartApiController : ControllerBase
{
    private readonly IBabyLengthChartService _babyLengthChartService;
    private readonly IBabyWeightChartService _babyWeightChartService;

    public BabyChartApiController(IBabyLengthChartService babyLengthChartService, IBabyWeightChartService babyWeightChartService)
    {
        _babyLengthChartService = babyLengthChartService;
        _babyWeightChartService = babyWeightChartService;
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

    [HttpPost("weight/add")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult AddWeight(string id, int month, double weight)
    {
        try
        {
            _babyWeightChartService.AddWeight(id, month, weight);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("weight/delete")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult DeleteWeight(string id, int month)
    {
        try
        {
            _babyWeightChartService.DeleteWeight(id, month);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("weight/edit")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult EditWeight(string id, int month, double weight)
    {
        try
        {
            _babyWeightChartService.EditWeight(id, month, weight);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("weight/get")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetWeights(string id)
    {
        try
        {
            var weights = _babyWeightChartService.GetWeight(id);
            return Ok(weights);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("weight/getreference")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetReferenceWeights(string gender, int percentile)
    {
        try
        {
            var weights = _babyWeightChartService.GetWeightReferenceData(gender, percentile);
            return Ok(weights);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}