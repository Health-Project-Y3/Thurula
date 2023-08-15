using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class DiaperTimes
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [BsonElement("babyId")]
    public string BabyId { get; set; } = string.Empty;

    [BsonElement("time")]
    public DateTime Time { get; set; } = DateTime.MinValue;

    [BsonElement("diaperNotes")]
    public string DiaperNotes { get; set; } = string.Empty;

    [BsonElement("diaperType")]
    public string DiaperType { get; set; } = string.Empty;

    [BsonElement("loggedBy")]
    public string LoggedBy { get; set; } = string.Empty;

}
