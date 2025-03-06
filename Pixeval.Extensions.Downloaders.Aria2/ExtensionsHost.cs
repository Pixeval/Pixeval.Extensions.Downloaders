using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Pixeval.Extensions.Common;
using Pixeval.Extensions.Downloaders.Aria2.Downloaders;
using Pixeval.Extensions.Downloaders.Aria2.Settings;
using Pixeval.Extensions.Downloaders.Aria2.Strings;
using Pixeval.Extensions.SDK;

namespace Pixeval.Extensions.Downloaders.Aria2;

[GeneratedComClass]
public partial class ExtensionsHost : ExtensionsHostBase
{
    public override string ExtensionName => Resource.ExtensionHostName;

    public override string AuthorName => "扑克";

    public override string ExtensionLink => "https://github.com/Pixeval/Pixeval.Extensions.Downloaders";

    public override string HelpLink => "https://github.com/Poker-sang";

    public override string Description => Resource.ExtensionHostDescription;

    public override byte[]? Icon
    {
        get
        {
            var stream = typeof(ExtensionsHost).Assembly.GetManifestResourceStream("logo");
            if (stream is null)
                return null;
            var array = new byte[stream.Length];
            _ = stream.Read(array);
            return array;
        }
    }

    public override string Version => "1.0.0";

    public static ExtensionsHost Current { get; } = new();

    [UnmanagedCallersOnly(EntryPoint = nameof(DllGetExtensionsHost))]
    private static unsafe int DllGetExtensionsHost(void** ppv) => DllGetExtensionsHost(ppv, Current);

    public override IExtension[] Extensions { get; } =
    [
        new Aria2ImageDownloaderExtension(),
        new EndPointSettingsExtension(),
        new SecretSettingsExtension(),
        new UserAgentSettingsExtension(),
        new ReferrerSettingsExtension()
    ];
}
