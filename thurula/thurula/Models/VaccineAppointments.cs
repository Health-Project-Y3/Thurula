using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class VaccineAppointments
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    //When concerning mothers it should be days from conception
    [BsonElement("daysfrombirth")]
    public int DaysFromBirth { get; set; } = 0;

    [BsonElement("user_type")] public string UserType = string.Empty;

}