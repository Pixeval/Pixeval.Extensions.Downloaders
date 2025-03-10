// Copyright (c) Pixeval.Extensions.Downloaders.Aria2.
// Licensed under the GPL v3 License.

using System.Runtime.InteropServices.Marshalling;
using Pixeval.Extensions.Common;
using Pixeval.Extensions.SDK.Downloaders;

namespace Pixeval.Extensions.Downloaders.Aria2.Downloaders;

[GeneratedComClass]
public partial class Aria2ImageDownloaderExtension : DownloaderExtensionBase
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

    public override void Download(IProgressNotifier notifier, string uri, string destination)
    {
        var name = Path.GetFileName(destination);
        var directory = Path.GetDirectoryName(destination);
        _ = Aria2Client.DownloadAsync(notifier, uri, directory!, name);
    }
}
