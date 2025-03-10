// Copyright (c) Pixeval.Extensions.Downloaders.Aria2.
// Licensed under the GPL v3 License.

using System.Text.Json.Serialization;

namespace Pixeval.Extensions.Downloaders.Aria2.Downloaders.Network;

public record AriaUri
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = "";

    [JsonPropertyName("uri")]
    public string Uri { get; set; } = "";
}
