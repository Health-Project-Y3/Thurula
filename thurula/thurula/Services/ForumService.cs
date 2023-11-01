using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class ForumService : IForumService
{
    private readonly IMongoCollection<ForumQuestion> _forumQuestions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ForumService(IAtlasDbSettings settings, IMongoClient client, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        var database = client.GetDatabase(settings.DatabaseName);
        _forumQuestions = database.GetCollection<ForumQuestion>("forum_messages");
    }

    public ForumQuestion GetQuestion(string id)
    {
        return _forumQuestions.Find(question => question.Id == id).FirstOrDefault();
    }

    /// <summary> Gets all questions in the db. Inadvisable to use this method. </summary>
    public List<ForumQuestion> GetQuestions()
    {
        return _forumQuestions.Find(question => true).ToList();
    }

    public List<ForumQuestion> GetQuestionsByAuthor(string authorId, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;
        var filter = Builders<ForumQuestion>.Filter.Eq(q => q.AuthorId, authorId);
        var sort = Builders<ForumQuestion>.Sort.Descending(q => q.Date);

        var questions = _forumQuestions
            .Find(filter)
            .Sort(sort)
            .Skip(skip)
            .Limit(pageSize)
            .ToList();
        return questions;
    }

    public List<ForumQuestion> GetQuestionsByKeywords(List<string> keywords, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;
        var filter = Builders<ForumQuestion>.Filter.AnyIn(q => q.Keywords, keywords);
        var sort = Builders<ForumQuestion>.Sort.Descending(q => q.Date);

        var questions = _forumQuestions
            .Find(filter)
            .Sort(sort)
            .Skip(skip)
            .Limit(pageSize)
            .ToList();
        return questions;
    }

    public List<ForumQuestion> GetQuestionsBetweenDates(DateTime start, DateTime end, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;
        var filter = Builders<ForumQuestion>.Filter.Gte(q => q.Date, start) &
                     Builders<ForumQuestion>.Filter.Lte(q => q.Date, end);
        var sort = Builders<ForumQuestion>.Sort.Descending(q => q.Date);

        var questions = _forumQuestions
            .Find(filter)
            .Sort(sort)
            .Skip(skip)
            .Limit(pageSize)
            .ToList();
        return questions;
    }

    public List<ForumQuestion> GetRecentQuestions(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        // Define a filter to fetch only visible questions
        var filter = Builders<ForumQuestion>.Filter.Eq(q => q.Visible, true);

        // Sort by date (or any other sorting criteria you prefer)
        var sort = Builders<ForumQuestion>.Sort.Descending(q => q.Date);

        // Perform the find operation with skip, limit, filter, and sort
        var questions = _forumQuestions
            .Find(filter)
            .Sort(sort)
            .Skip(skip)
            .Limit(pageSize)
            .ToList();
        return questions;
    }

    public List<ForumQuestion> SearchQuestions(string query, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        // Define a filter to search for questions containing the query string in the Question, Description or Keywords fields
        var filter = Builders<ForumQuestion>.Filter.Regex(q => q.Question, new BsonRegularExpression(query, "i"))
                     | Builders<ForumQuestion>.Filter.Regex(q => q.Description, new BsonRegularExpression(query, "i"));

        // Sort by date (or any other sorting criteria you prefer)
        var sort = Builders<ForumQuestion>.Sort.Descending(q => q.Date);

        // Perform the find operation with skip, limit, filter, and sort
        var questions = _forumQuestions
            .Find(filter)
            .Sort(sort)
            .Skip(skip)
            .Limit(pageSize)
            .ToList();

        return questions;
    }

    [Authorize]
    public ForumQuestion AddQuestion(ForumQuestion question)
    {
        if (question.Description == "") throw new Exception("Description cannot be empty");
        if (question.Question == "") throw new Exception("Question cannot be empty");
        _forumQuestions.InsertOne(question);
        return question;
    }

    [Authorize]
    public void DeleteQuestion(string id)
    {
        _forumQuestions.DeleteOne(question => question.Id == id);
    }

    [Authorize]
    public ForumAnswer AddAnswer(string questionId, ForumAnswer answer)
    {
        answer.Id = ObjectId.GenerateNewId().ToString();
        answer.QuestionId = questionId;

        var question = GetQuestion(questionId);
        question.Answers.Add(answer);
        _forumQuestions.ReplaceOne(q => q.Id == questionId, question);
        return answer;
    }

    [Authorize]
    public void DeleteAnswer(string questionId, string answerId)
    {
        var question = GetQuestion(questionId);
        question.Answers.RemoveAll(answer => answer.Id == answerId);
        _forumQuestions.ReplaceOne(q => q.Id == questionId, question);
    }

    [Authorize]
    public void UpvoteQuestion(string questionId)
    {
        var question = GetQuestion(questionId);
        question.Upvotes++;
        question.Upvoters.Add(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)??"");
        _forumQuestions.ReplaceOne(q => q.Id == questionId, question);
    }

    [Authorize]
    public void UndoUpvoteQuestion(string questionId)
    {
        var question = GetQuestion(questionId);
        question.Upvotes--;
        question.Upvoters.Remove(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)??"");
        _forumQuestions.ReplaceOne(q => q.Id == questionId, question);
    }

    [Authorize]
    public void DownvoteQuestion(string questionId)
    {
        var question = GetQuestion(questionId);
        question.Downvotes++;
        question.Downvoters.Add(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)??"");
        _forumQuestions.ReplaceOne(q => q.Id == questionId, question);
    }

    [Authorize]
    public void UndoDownvoteQuestion(string questionId)
    {
        var question = GetQuestion(questionId);
        question.Downvotes--;
        question.Downvoters.Remove(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)??"");
        _forumQuestions.ReplaceOne(q => q.Id == questionId, question);
    }

    [Authorize]
    public void UpvoteAnswer(string questionId, string answerId)
    {
        var question = GetQuestion(questionId);
        var answer = question.Answers.Find(answer => answer.Id == answerId);
        if (answer != null)
        {
            answer.Upvotes++;
            answer.Upvoters.Add(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
        }

        _forumQuestions.ReplaceOne(q => q.Id == questionId, question);
    }

    [Authorize]
    public void UndoUpvoteAnswer(string questionId, string answerId)
    {
        var question = GetQuestion(questionId);
        var answer = question.Answers.Find(answer => answer.Id == answerId);
        if (answer != null)
        {
            answer.Upvotes--;
            answer.Upvoters.Remove(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
        }
        _forumQuestions.ReplaceOne(q => q.Id == questionId, question);
    }

    [Authorize]
    public void DownvoteAnswer(string questionId, string answerId)
    {
        var question = GetQuestion(questionId);
        var answer = question.Answers.Find(answer => answer.Id == answerId);
        if (answer != null)
        {
            answer.Downvotes++;
            answer.Downvoters.Add(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
        }
        _forumQuestions.ReplaceOne(q => q.Id == questionId, question);
    }

    [Authorize]
    public void UndoDownvoteAnswer(string questionId, string answerId)
    {
        var question = GetQuestion(questionId);
        var answer = question.Answers.Find(answer => answer.Id == answerId);
        if (answer != null)
        {
            answer.Downvotes--;
            answer.Downvoters.Remove(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
        }
        _forumQuestions.ReplaceOne(q => q.Id == questionId, question);
    }

    [Authorize]
    public void AcceptAnswer(string questionId, string answerId)
    {
        var question = GetQuestion(questionId);
        question.Answers.ForEach(answer => answer.Accepted = answer.Id == answerId);
        _forumQuestions.ReplaceOne(q => q.Id == questionId, question);
    }
}