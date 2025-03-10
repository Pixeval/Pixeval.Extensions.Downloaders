// Copyright (c) Pixeval.Extensions.Downloaders.Aria2.
// Licensed under the GPL v3 License.

using System.Text.Json.Serialization;

namespace Pixeval.Extensions.Downloaders.Aria2.Downloaders.Network;

[JsonSerializable(typeof(Aria2Request))]
[JsonSerializable(typeof(IEnumerable<object>))]
[JsonSerializable(typeof(Dictionary<string, string>))]

[JsonSerializable(typeof(ResultCode<FileDownloadTell>))]
[JsonSerializable(typeof(ResultCode<string>))]
[JsonSerializable(typeof(ResultCode<object>))]
[JsonSerializable(typeof(ResultCode<List<FileDownloadTell>>))]
[JsonSerializable(typeof(ResultCode<List<string>>))]
[JsonSerializable(typeof(ResultCode<List<object>>))]
public partial class Aria2RequestContext : JsonSerializerContext;
