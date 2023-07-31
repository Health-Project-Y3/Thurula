using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class BabyNaps
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("babyId")]
    public string BabyId { get; set; } = string.Empty;

    [BsonElement("startTime")]
    public DateTime StartTime { get; set; } = DateTime.MinValue;

    [BsonElement("endTime")]
    public DateTime EndTime { get; set; } = DateTime.MinValue;

    [BsonElement("sleepNotes")]
    public string SleepNotes { get; set; } = string.Empty;

    [BsonElement("sleepQuality")]
    public int SleepQuality { get; set; } = -1;

    [BsonElement("loggedBy")]
    public string LoggedBy { get; set; } = string.Empty;
}
