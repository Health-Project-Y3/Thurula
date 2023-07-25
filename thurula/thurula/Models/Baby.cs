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

    [BsonElement("ownerIds")]
    public List<string> Owners { get; set; } = new List<string>();

    [BsonElement("lengths")]
    public List<double> Lengths { get; set; } = new List<double>(Enumerable.Repeat(-1.0, 24));

    [BsonElement("weights")]
    public List<double> Weights { get; set; } = new List<double>(Enumerable.Repeat(-1.0, 24));

}