using System.Runtime.InteropServices.Marshalling;
using FluentIcons.Common;
using Pixeval.Extensions.Downloaders.Aria2.Downloaders;
using Pixeval.Extensions.Downloaders.Aria2.Strings;
using Pixeval.Extensions.SDK.Settings;

namespace Pixeval.Extensions.Downloaders.Aria2.Settings;

[GeneratedComClass]
public partial class ReferrerSettingsExtension : StringSettingsExtensionBase
{
    public override string DefaultValue => "https://www.pixiv.net";

    public override void OnValueChanged(string value)
    {
        Aria2ImageDownloaderExtension.Aria2Client.Referrer = value;
    }

    public override string Placeholder => Resource.ReferrerSettingsPlaceholder;

    public override Symbol Icon => Symbol.SwipeUp;

    public override string Label => Resource.ReferrerSettingsLabel;

    public override string Description => Resource.ReferrerSettingsDescription;

    public override string Token => "Referrer";
}
