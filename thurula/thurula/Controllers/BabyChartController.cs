using Microsoft.AspNetCore.Mvc;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/babychart")]
[ApiController]
public class BabyChartController : ControllerBase
{
    private readonly IBabyLengthChartService _babyLengthChartService;
    private readonly IBabyWeightChartService _babyWeightChartService;

    public class TempPoint
    {
        public string Id { get; set; }
        public int Month { get; set; }
        public double Value { get; set; }
    }

    public BabyChartController(IBabyLengthChartService babyLengthChartService,
        IBabyWeightChartService babyWeightChartService)
    {
        _babyLengthChartService = babyLengthChartService;
        _babyWeightChartService = babyWeightChartService;
    }

    [HttpPost("length/add")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult AddLength([FromBody] TempPoint temp)
    {
        try
        {
            _babyLengthChartService.AddLength(temp.Id, temp.Month, temp.Value);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("length/delete")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult DeleteLength([FromBody] TempPoint temp)
    {
        try
        {
            _babyLengthChartService.DeleteLength(temp.Id, temp.Month);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("length/edit")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult EditLength([FromBody] TempPoint temp)
    {
        try
        {
            _babyLengthChartService.EditLength(temp.Id, temp.Month, temp.Value);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("length/get")]
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

    [HttpGet("length/getreference")]
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
    public ActionResult AddWeight([FromBody] TempPoint temp)
    {
        try
        {
            _babyWeightChartService.AddWeight(temp.Id, temp.Month, temp.Value);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("weight/delete")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult DeleteWeight([FromBody] TempPoint temp)
    {
        try
        {
            _babyWeightChartService.DeleteWeight(temp.Id, temp.Month);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("weight/edit")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult EditWeight([FromBody] TempPoint temp)
    {
        try
        {
            _babyWeightChartService.EditWeight(temp.Id, temp.Month, temp.Value);
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("weight/get")]
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

    [HttpGet("weight/getreference")]
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