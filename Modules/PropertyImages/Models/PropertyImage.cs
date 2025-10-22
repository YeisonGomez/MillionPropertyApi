using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MillionPropertyApi.Modules.PropertyImages.Models;

public class PropertyImage
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? IdPropertyImage { get; set; }

    [BsonElement("idProperty")]
    public string IdProperty { get; set; } = string.Empty;

    [BsonElement("file")]
    public string File { get; set; } = string.Empty;

    [BsonElement("enabled")]
    public bool Enabled { get; set; } = true;
}
