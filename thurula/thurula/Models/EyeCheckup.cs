using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class EyeCheckup
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("babyId")]
    public string BabyId { get; set; } = string.Empty;

    [BsonElement("checkeddate")] 
    public DateTime CheckedDate { get; set; }

    [BsonElement("score")] 
    public int Score { get; set; }

}