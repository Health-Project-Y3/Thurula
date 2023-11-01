using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class UserWeight
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [BsonElement("user_id")] public string UserId { get; set; } = string.Empty;
    [BsonElement("date")] public DateTime Date { get; set; }
    //weight in kg
    [BsonElement("weight")] public double Weight { get; set; } = 0;
}