using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;
[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [BsonElement("username")] public string Username { get; set; } = string.Empty;
    [BsonElement("password")] public string PasswordHash { get; set; } = string.Empty;
    [BsonElement("fname")] public string FirstName { get; set; } = string.Empty;
    [BsonElement("lname")] public string LastName { get; set; } = string.Empty;
    [BsonElement("gender")] public string Gender { get; set; } = string.Empty;
    [BsonElement("email")] public string Email { get; set; } = string.Empty;
    [BsonElement("phone")] public string Phone { get; set; } = string.Empty;
    [BsonElement("pregnant")] public bool Pregnant { get; set; } = false;
    [BsonElement("conception_date")] public DateTime ConceptionDate { get; set; } = DateTime.MinValue;
    [BsonElement("babyIds")] public List<string> Babies { get; set; } = new();
    [BsonElement("dueDate")] public DateTime DueDate { get; set; } = DateTime.MinValue;
    [BsonElement("favouriteNames")] public List<string> FavouriteNames { get; set; } = new();
    [BsonElement("completedvaccines")] public HashSet<string> CompletedVaccines { get; set; } = new HashSet<string>();
    [BsonElement("duevaccines")] public HashSet<string> DueVaccines { get; set; } = new HashSet<string>();
    //in centimeters
    [BsonElement("height")] public double Height { get; set; }
    //in kilograms
    [BsonElement("weight")] public double Weight { get; set; }
    //in kilograms
    [BsonElement("prepregnancy_weight")] public double PreWeight { get; set; }
}