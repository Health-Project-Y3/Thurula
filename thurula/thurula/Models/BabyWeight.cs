using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class BabyWeight
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("gender")]
    public string Gender { get; set; } = string.Empty;
    [BsonElement("percentile")]
    public int Percentile { get; set; } = 0;
    [BsonElement("months")]
    public List<double> Weights { get; set; } = new List<double>();

}