using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class UserExercise
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [BsonElement("user_id")] public string UserId { get; set; } = string.Empty;
    [BsonElement("date")] public DateTime Date { get; set; }
    [BsonElement("minutes_exercised")] public int MinutesExercised { get; set; } = 0;
}