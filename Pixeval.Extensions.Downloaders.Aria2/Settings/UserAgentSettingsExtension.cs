using System.Runtime.InteropServices.Marshalling;
using FluentIcons.Common;
using Pixeval.Extensions.Downloaders.Aria2.Downloaders;
using Pixeval.Extensions.Downloaders.Aria2.Strings;
using Pixeval.Extensions.SDK.Settings;

namespace Pixeval.Extensions.Downloaders.Aria2.Settings;

[GeneratedComClass]
public partial class UserAgentSettingsExtension : StringSettingsExtensionBase
{
    public override string DefaultValue => "PixivIOSApp/5.8.7 Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36 Edg/133.0.0.0";

    public override void OnValueChanged(string value)
    {
        Aria2ImageDownloaderExtension.Aria2Client.UserAgent = value;
    }

    public override string Placeholder => Resource.UserAgentSettingsPlaceholder;

    public override Symbol Icon => Symbol.Person;

    public override string Label => Resource.UserAgentSettingsLabel;

    public override string Description => Resource.UserAgentSettingsDescription;

    public override string Token => "UserAgent";
}
