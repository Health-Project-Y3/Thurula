using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class BabyFeeding
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

    [BsonElement("feedingType")]
    public string FeedingType { get; set; } = string.Empty;

    [BsonElement("feedingAmount")]
    public FeedingAmount FeedingAmount { get; set; } = new FeedingAmount();

    [BsonElement("feedingNotes")]
    public string FeedingNotes { get; set; } = string.Empty;

    [BsonElement("feedingMood")]
    public int FeedingMood { get; set; } = -1;

    [BsonElement("loggedBy")]
    public string LoggedBy { get; set; } = string.Empty;

}

public class FeedingAmount
{
    [BsonElement("value")]
    public int Value { get; set; } = 0;

    [BsonElement("unit")]
    public string Unit { get; set; } = string.Empty;
}
