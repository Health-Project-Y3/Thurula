using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace thurula.Models;

public class Instruction
{
    [Required]
    [MaxLength(500)]
    [BsonElement("instruction")]
    public string InstructionText { get; set; }

    [Required]
    // [MaxLength(500)]
    [BsonElement("checked")]
    public bool Checked { get; set; }
}

public class Checklist
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [Required]
    [MaxLength(500)]
    [BsonElement("period")]
    public string Period { get; set; } = string.Empty;
    [Required]
    [MaxLength(500)]
    [BsonElement("category")]
    public string Category { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    [BsonElement("instructions")]
    public List<Instruction> Instructions { get; set; }
    // public array Instructions { get; set; } = array.Empty;
    
    
}