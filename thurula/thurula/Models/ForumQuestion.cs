using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class ForumQuestion
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("question")]
    public string Question { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("author_id")]
    public string AuthorId { get; set; } =  string.Empty;

    [BsonElement("author_fname")]
    public string AuthorFirstName { get; set; } =  string.Empty;

    [BsonElement("author_lname")]
    public string AuthorLastName { get; set; } =  string.Empty;

    [BsonElement("date")]
    public DateTime Date { get; set; } = DateTime.Now;

    [BsonElement("keywords")]
    public List<string> Keywords { get; set; } = new();

    [BsonElement("visible")]
    public bool Visible { get; set; } = true;

    [BsonElement("upvotes")]
    public int Upvotes { get; set; } = 0;

    [BsonElement("downvotes")]
    public int Downvotes { get; set; } = 0;

    [BsonElement("answers")]
    public List<ForumAnswer> Answers { get; set; } = new();

    [BsonElement("upvoters")]
    public List<string> Upvoters { get; set; } = new();

    [BsonElement("downvoters")]
    public List<string> Downvoters { get; set; } = new();

}

public class ForumAnswer
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("question_id")]
    public string QuestionId { get; set; } = string.Empty;

    [BsonElement("answer")]
    public string Answer { get; set; } = string.Empty;

    [BsonElement("author_id")]
    public string Author { get; set; } =  string.Empty;

    [BsonElement("author_fname")]
    public string AuthorFirstName { get; set; } =  string.Empty;

    [BsonElement("author_lname")]
    public string AuthorLastName { get; set; } =  string.Empty;

    [BsonElement("date")]
    public DateTime Date { get; set; } = DateTime.Now;

    [BsonElement("accepted")]
    public bool Accepted { get; set; } = false;

    [BsonElement("upvotes")]
    public int Upvotes { get; set; } = 0;

    [BsonElement("downvotes")]
    public int Downvotes { get; set; } = 0;

    [BsonElement("upvoters")]
    public List<string> Upvoters { get; set; } = new();

    [BsonElement("downvoters")]
    public List<string> Downvoters { get; set; } = new();

}