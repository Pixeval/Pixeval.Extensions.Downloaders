// Copyright (c) Pixeval.Extensions.Downloaders.Aria2.
// Licensed under the GPL v3 License.

using System.Text.Json.Serialization;

namespace Pixeval.Extensions.Downloaders.Aria2.Downloaders.Network;

/// <summary>
/// 多方法请求组合
/// </summary>
public record MultiCallRequest(
    [property: JsonPropertyName("methodName")] string MethodName,
    [property: JsonPropertyName("params")] IEnumerable<object> Parameters);
