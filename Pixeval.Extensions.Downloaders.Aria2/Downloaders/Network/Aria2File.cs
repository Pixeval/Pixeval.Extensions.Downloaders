// Copyright (c) Pixeval.Extensions.Downloaders.Aria2.
// Licensed under the GPL v3 License.

using System.Text.Json.Serialization;

namespace Pixeval.Extensions.Downloaders.Aria2.Downloaders.Network;

public record Aria2File
{
    [JsonPropertyName("completedLength")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public long CompletedLength { get; set; }

    [JsonPropertyName("index")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public int Index { get; set; } 

    [JsonPropertyName("length")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public long Length { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; } = "";

    [JsonPropertyName("selected")]
    public string Selected { get; set; } = "";

    [JsonPropertyName("uris")]
    public List<AriaUri> Uris { get; set; } = [];
}
