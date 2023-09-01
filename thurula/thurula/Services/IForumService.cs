using thurula.Models;

namespace thurula.Services;

public interface IForumService
{
    ForumQuestion GetQuestion(string id);
    List<ForumQuestion> GetQuestions();
    List<ForumQuestion> GetQuestionsByAuthor(string authorId, int page, int pageSize);
    List<ForumQuestion> GetQuestionsByKeywords(List<string> keywords, int page, int pageSize);
    List<ForumQuestion> GetQuestionsBetweenDates(DateTime start, DateTime end, int page, int pageSize);
    List<ForumQuestion> GetRecentQuestions(int page, int pageSize);
    List<ForumQuestion> SearchQuestions(string query, int page, int pageSize);

    void AddQuestion(ForumQuestion question);
    void DeleteQuestion(string id);
    void AddAnswer(string questionId, ForumAnswer answer);
    void DeleteAnswer(string questionId, string answerId);
    void UpvoteQuestion(string questionId);
    void DownvoteQuestion(string questionId);
    void UpvoteAnswer(string questionId, string answerId);
    void DownvoteAnswer(string questionId, string answerId);
    void AcceptAnswer(string questionId, string answerId);
}