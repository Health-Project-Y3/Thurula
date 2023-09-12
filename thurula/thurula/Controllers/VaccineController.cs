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
    private readonly IUserService _userService;

    public VaccineController(IVaccineAppointmentService vaccineAppointmentService, IBabyService babyService, IUserService userService)
    {
        _vaccineAppointmentService = vaccineAppointmentService;
        _babyService = babyService;
        _userService = userService;
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

    [HttpGet("baby/due/{babyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VaccineAppointments>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<VaccineAppointments>> GetDueBabyVaccines(string babyId)
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

    [HttpGet("baby/completed/{babyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VaccineAppointments>))]
    public ActionResult<IEnumerable<VaccineAppointments>> GetCompletedBabyVaccines(string babyId)
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

    [HttpPut("baby/complete/{babyId}/{vaccineId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VaccineAppointments>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public void CompleteBabyVaccine(string babyId, string vaccineId)
    {
        try
        {
            _babyService.MarkVaccineAppointment(babyId, vaccineId, true);
        } catch (Exception ex)
        {
            BadRequest(ex.Message);
        }
    }

    [HttpPut("baby/undo/{babyId}/{vaccineId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VaccineAppointments>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public void UndoBabyVaccine(string babyId, string vaccineId)
    {
        try
        {
            _babyService.MarkVaccineAppointment(babyId, vaccineId, false);
        } catch (Exception ex)
        {
            BadRequest(ex.Message);
        }
    }

    [HttpGet("mom/due/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VaccineAppointments>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<VaccineAppointments>> GetDueMomVaccines(string userId)
    {
        try
        {
            var vaccines = _userService.GetDueVaccines(userId);
            return Ok(vaccines);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("mom/completed/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VaccineAppointments>))]
    public ActionResult<IEnumerable<VaccineAppointments>> GetCompletedMomVaccines(string userId)
    {
        try
        {
            var vaccines = _userService.GetCompletedVaccines(userId);
            return Ok(vaccines);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("mom/complete/{userId}/{vaccineId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VaccineAppointments>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public void CompleteMomVaccine(string userId, string vaccineId)
    {
        try
        {
            _userService.MarkVaccineAppointment(userId, vaccineId, true);
        } catch (Exception ex)
        {
            BadRequest(ex.Message);
        }
    }

    [HttpPut("mom/undo/{userId}/{vaccineId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VaccineAppointments>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public void UndoMomVaccine(string userId, string vaccineId)
    {
        try
        {
            _userService.MarkVaccineAppointment(userId, vaccineId, false);
        } catch (Exception ex)
        {
            BadRequest(ex.Message);
        }
    }
}