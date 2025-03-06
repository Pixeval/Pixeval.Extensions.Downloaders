using System.Runtime.InteropServices.Marshalling;
using FluentIcons.Common;
using Pixeval.Extensions.Downloaders.Aria2.Downloaders;
using Pixeval.Extensions.Downloaders.Aria2.Strings;
using Pixeval.Extensions.SDK.Settings;

namespace Pixeval.Extensions.Downloaders.Aria2.Settings;

[GeneratedComClass]
public partial class SecretSettingsExtension : StringSettingsExtensionBase
{
    public override string DefaultValue => "";

    public override void OnValueChanged(string value)
    {
        Aria2ImageDownloaderExtension.Aria2Client.Secret = value;
    }

    public override string Placeholder => Resource.SecretSettingsPlaceholder;

    public override Symbol Icon => Symbol.Key;

    public override string Label => Resource.SecretSettingsLabel;

    public override string Description => Resource.SecretSettingsDescription;

    public override string Token => "Secret";
}
