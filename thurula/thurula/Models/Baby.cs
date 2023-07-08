using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class Baby
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [Required]
    [MaxLength(30)]
    [BsonElement("fname")]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [MaxLength(30)]
    [BsonElement("lname")]
    public string LastName { get; set; } = string.Empty;

    
    
}