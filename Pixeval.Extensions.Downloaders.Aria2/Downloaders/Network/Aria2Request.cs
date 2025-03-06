// Copyright (c) Pixeval.Extensions.Downloaders.Aria2.
// Licensed under the GPL v3 License.

using System.Text.Json.Serialization;

namespace Pixeval.Extensions.Downloaders.Aria2.Downloaders.Network;

public record Aria2Request(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("jsonrpc")] string JsonRpc,
    [property: JsonPropertyName("method")] string Method,
    [property: JsonPropertyName("params")] List<object> Parameters);

[JsonSerializable(typeof(Aria2Request))]
[JsonSerializable(typeof(IEnumerable<object>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
public partial class Aria2RequestContext : JsonSerializerContext;
