using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using thurula.Hubs;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[ApiController]
[Route("[controller]")]
public class ForumController : ControllerBase
{
    private readonly IHubContext<ForumHub, IChatClient> _hubContext;
    private readonly IForumService _forumService;

    public ForumController(IHubContext<ForumHub, IChatClient> hubContext, IForumService forumService)
    {
        _hubContext = hubContext;
        _forumService = forumService;
    }

    [HttpPost("broadcast")]
    public async Task<IActionResult> BroadcastMessage([FromBody] string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            await _hubContext.Clients.All.ReceiveMessage(message);
            return NoContent();
        } else
        {
            return BadRequest("Message cannot be empty.");
        }
    }

    [HttpGet("questions")]
    [ProducesResponseType((StatusCodes.Status200OK))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<ForumQuestion>> GetQuestions()
    {
        return Ok(_forumService.GetQuestions());
    }

    [HttpGet("questions/id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ForumQuestion))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ForumQuestion> GetQuestion(string id)
    {
        var question = _forumService.GetQuestion(id);
        if (question == null)
        {
            return NotFound();
        }

        return Ok(question);
    }

    [HttpGet("questions/recent")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ForumQuestion>))]
    public ActionResult<IEnumerable<ForumQuestion>> GetQuestions(int page, int pageSize)
    {
        return Ok(_forumService.GetRecentQuestions(page, pageSize));
    }

    [HttpGet("questions/search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ForumQuestion>))]
    public ActionResult<IEnumerable<ForumQuestion>> SearchQuestions(string query, int page, int pageSize)
    {
        return Ok(_forumService.SearchQuestions(query, page, pageSize));
    }

    [HttpGet("questions/author")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ForumQuestion>))]
    public ActionResult<IEnumerable<ForumQuestion>> GetQuestionsByAuthor(string authorId, int page, int pageSize)
    {
        return Ok(_forumService.GetQuestionsByAuthor(authorId, page, pageSize));
    }

    [HttpGet("questions/keywords")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ForumQuestion>))]
    public ActionResult<IEnumerable<ForumQuestion>> GetQuestionsByKeywords(int page, int pageSize,
        [FromQuery] string[] keywords)
    {
        return Ok(_forumService.GetQuestionsByKeywords(keywords.ToList(), page, pageSize));
    }

    [HttpGet("questions/dates")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ForumQuestion>))]
    public ActionResult<IEnumerable<ForumQuestion>> GetQuestionsBetweenDates(int page, int pageSize, DateTime start,
        DateTime end)
    {
        return Ok(_forumService.GetQuestionsBetweenDates(start, end, page, pageSize));
    }

    [Authorize]
    [HttpPost("questions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ForumQuestion> AddQuestion([FromBody] ForumQuestion question)
    {
        try
        {
            var q  = _forumService.AddQuestion(question);
            return Ok(q);
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpDelete("questions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult DeleteQuestion(string id)
    {
        try
        {
            _forumService.DeleteQuestion(id);
            return Ok();
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPut("questions/upvote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult UpvoteQuestion(string questionId, bool undo = false)
    {
        try
        {
            if (undo)
            {
                _forumService.UndoUpvoteQuestion(questionId);
            } else
            {
                _forumService.UpvoteQuestion(questionId);
            }

            return Ok();
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPut("questions/downvote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult DownvoteQuestion(string questionId, bool undo = false)
    {
        try
        {
            if (undo)
            {
                _forumService.UndoDownvoteQuestion(questionId);
            } else
            {
                _forumService.DownvoteQuestion(questionId);
            }

            return Ok();
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPut("questions/switchvote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult SwitchVote(string questionId, bool upvote)
    {
        try
        {
            if (upvote)
            {
                _forumService.UpvoteQuestion(questionId);
                _forumService.UndoDownvoteQuestion(questionId);
            } else
            {
                _forumService.DownvoteQuestion(questionId);
                _forumService.UndoUpvoteQuestion(questionId);
            }

            return Ok();
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPost("answers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ForumAnswer> AddAnswer(string questionId, [FromBody] ForumAnswer answer)
    {
        try
        {
            return Ok(_forumService.AddAnswer(questionId, answer));
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpDelete("answers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult DeleteAnswer(string questionId, string answerId)
    {
        try
        {
            _forumService.DeleteAnswer(questionId, answerId);
            return Ok();
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPut("answers/upvote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult UpvoteAnswer(string questionId, string answerId, bool undo = false)
    {
        try
        {
            if (undo)
            {
                _forumService.UndoUpvoteAnswer(questionId, answerId);
            } else
            {
                _forumService.UpvoteAnswer(questionId, answerId);
            }

            return Ok();
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPut("answers/downvote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult DownvoteAnswer(string questionId, string answerId, bool undo = false)
    {
        try
        {
            if (undo)
            {
                _forumService.UndoDownvoteAnswer(questionId, answerId);
            } else
            {
                _forumService.DownvoteAnswer(questionId, answerId);
            }

            return Ok();
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPut("answers/switchvote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult SwitchVote(string questionId, string answerId, bool upvote)
    {
        try
        {
            if (upvote)
            {
                _forumService.UpvoteAnswer(questionId, answerId);
                _forumService.UndoDownvoteAnswer(questionId, answerId);
            } else
            {
                _forumService.DownvoteAnswer(questionId, answerId);
                _forumService.UndoUpvoteAnswer(questionId, answerId);
            }

            return Ok();
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPut("answers/accept")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult AcceptAnswer(string questionId, string answerId)
    {
        try
        {
            _forumService.AcceptAnswer(questionId, answerId);
            return Ok();
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}