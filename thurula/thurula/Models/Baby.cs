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

    [MaxLength(30)] [BsonElement("lname")] public string LastName { get; set; } = string.Empty;

    [BsonElement("birthdate")] public DateTime BirthDate { get; set; }

    [BsonElement("ownerIds")] public HashSet<string> Owners { get; set; } = new HashSet<string>();

    [BsonElement("lengths")] public List<double> Lengths { get; set; } = new List<double>(Enumerable.Repeat(-1.0, 24));

    [BsonElement("weights")] public List<double> Weights { get; set; } = new List<double>(Enumerable.Repeat(-1.0, 24));

    [BsonElement("completedvaccines")] public HashSet<string> CompletedVaccines { get; set; } = new HashSet<string>();

    [BsonElement("duevaccines")] public HashSet<string> DueVaccines { get; set; } = new HashSet<string>();
}