// Copyright (c) Pixeval.Extensions.Downloaders.Aria2.
// Licensed under the GPL v3 License.

using System.Text.Json.Serialization;

namespace Pixeval.Extensions.Downloaders.Aria2.Downloaders.Network;

public record FileDownloadTell
{
    [JsonPropertyName("bitfield")]
    public string Bitfield { get; set; } = "";

    [JsonPropertyName("bittorrent")]
    public object? Bittorrent { get; set; }

    [JsonPropertyName("completedLength")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public long CompletedLength { get; set; }

    [JsonPropertyName("connections")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public int Connections { get; set; }

    [JsonPropertyName("dir")]
    public string Dir { get; set; } = "";

    [JsonPropertyName("downloadSpeed")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public long DownloadSpeed { get; set; }

    [JsonPropertyName("files")]
    public List<Aria2File> Files { get; set; } = [];

    [JsonPropertyName("gid")]
    public string Gid { get; set; } = "";

    [JsonPropertyName("infoHash")]
    public string? InfoHash { get; set; }

    [JsonPropertyName("numPieces")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public int NumPieces { get; set; }

    [JsonPropertyName("numSeeders")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public int NumSeeders { get; set; }

    [JsonPropertyName("pieceLength")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public long PieceLength { get; set; }

    [JsonPropertyName("seeder")]
    public string? Seeder { get; set; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter<TellState>))]
    public TellState Status { get; set; }

    [JsonPropertyName("totalLength")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public long TotalLength { get; set; }

    [JsonPropertyName("uploadLength")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public long UploadLength { get; set; }

    [JsonPropertyName("uploadSpeed")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public long UploadSpeed { get; set; }
}
