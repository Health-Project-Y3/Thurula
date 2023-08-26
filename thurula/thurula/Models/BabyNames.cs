using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class BabyNames
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    [BsonElement("meaning")]
    public string Meaning { get; set; } = string.Empty;
    [BsonElement("gender")]
    public string Gender { get; set; } = string.Empty;
}