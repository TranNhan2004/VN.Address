using System.Text.Json.Serialization;

namespace VN.Address;

internal class ProvinceModel
{
    [JsonPropertyName("name")]
    public required string Name { get; set; } 

    [JsonPropertyName("wards")]
    public required List<WardModel> Wards { get; set; }
}

internal class WardModel
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}