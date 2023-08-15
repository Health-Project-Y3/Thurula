using Microsoft.AspNetCore.Mvc;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/vaccines")]
[ApiController]
public class VaccineController : ControllerBase
{
    private readonly IVaccineAppointmentService _vaccineAppointmentService;
    private readonly IBabyService _babyService;

    public VaccineController(IVaccineAppointmentService vaccineAppointmentService, IBabyService babyService)
    {
        _vaccineAppointmentService = vaccineAppointmentService;
        _babyService = babyService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VaccineAppointments))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<VaccineAppointments> Get(string id)
    {
        try
        {
            var vaccine = _vaccineAppointmentService.Get(id);
            return Ok(vaccine);
        } catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("due/{babyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VaccineAppointments>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<VaccineAppointments>> GetDueVaccines(string babyId)
    {
        try
        {
            var vaccines = _babyService.GetDueVaccines(babyId);
            return Ok(vaccines);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("completed/{babyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VaccineAppointments>))]
    public ActionResult<IEnumerable<VaccineAppointments>> GetCompletedVaccines(string babyId)
    {
        try
        {
            var vaccines = _babyService.GetCompletedVaccines(babyId);
            return Ok(vaccines);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("complete/{babyId}/{vaccineId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VaccineAppointments>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public void CompleteVaccine(string babyId, string vaccineId)
    {
        try
        {
            _babyService.MarkVaccineAppointment(babyId, vaccineId, true);
        } catch (Exception ex)
        {
            BadRequest(ex.Message);
        }
    }

    [HttpPut("undo/{babyId}/{vaccineId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VaccineAppointments>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public void UndoVaccine(string babyId, string vaccineId)
    {
        try
        {
            _babyService.MarkVaccineAppointment(babyId, vaccineId, false);
        } catch (Exception ex)
        {
            BadRequest(ex.Message);
        }
    }
}