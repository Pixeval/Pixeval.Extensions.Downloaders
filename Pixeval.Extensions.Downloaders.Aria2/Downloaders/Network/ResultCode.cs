// Copyright (c) Pixeval.Extensions.Downloaders.Aria2.
// Licensed under the GPL v3 License.

using System.Text.Json.Serialization;

namespace Pixeval.Extensions.Downloaders.Aria2.Downloaders.Network;

public record ResultCode<T>(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("jsonrpc")] string JsonRpc,
    [property: JsonPropertyName("result")] T Result);
