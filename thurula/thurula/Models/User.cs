using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [BsonElement("username")]
    public string Username { get; set; } = string.Empty;
    [BsonElement("password")]
    public string PasswordHash { get; set; } = string.Empty;
    
    [BsonElement("fname")]
    public string FirstName { get; set; } = string.Empty;
    [BsonElement("lname")]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [BsonElement("gender")]
    public string Gender { get; set; } = string.Empty;
    
    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;
    
    [BsonElement("phone")]
    public string Phone { get; set; } = string.Empty;
    
    [BsonElement("pregnant")]
    public bool Pregnant { get; set; } = false;
    
    [BsonElement("babyIds")] 
    public List<string> BabyIds { get; set; } = new();

    [BsonElement("dueDate")] 
    public DateTime DueDate { get; set; } = DateTime.MinValue; 
    
}