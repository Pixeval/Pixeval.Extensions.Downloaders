// Copyright (c) Pixeval.Extensions.Downloaders.Aria2.
// Licensed under the GPL v3 License.

using System.Runtime.InteropServices.Marshalling;
using Pixeval.Extensions.SDK.Downloaders;

namespace Pixeval.Extensions.Downloaders.Aria2.Downloaders;

[GeneratedComClass]
public partial class Aria2ImageDownloaderExtension : ImageDownloaderExtensionBase
{
    public static Aria2Client Aria2Client { get; private set; } = null!;

    public override void OnExtensionLoaded()
    {
        Aria2Client = new();
    }

    public override void OnExtensionUnloaded()
    {
        Aria2Client.Dispose();
    }

    public override Task<string?> DownloadAsync(string uri, string destination)
    {
        var name = Path.GetFileName(destination);
        var directory = Path.GetDirectoryName(destination);
        return Aria2Client.DownloadAsync(uri, directory!, name);
    }
}
