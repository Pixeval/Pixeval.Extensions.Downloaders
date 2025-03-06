using System.Runtime.InteropServices.Marshalling;
using FluentIcons.Common;
using Pixeval.Extensions.Downloaders.Aria2.Downloaders;
using Pixeval.Extensions.Downloaders.Aria2.Strings;
using Pixeval.Extensions.SDK.Settings;

namespace Pixeval.Extensions.Downloaders.Aria2.Settings;

[GeneratedComClass]
public partial class EndPointSettingsExtension : StringSettingsExtensionBase
{
    public override string DefaultValue => "http://localhost:6800";

    public override void OnValueChanged(string value)
    {
        Aria2ImageDownloaderExtension.Aria2Client.EndPoint = value;
    }

    public override string Placeholder => Resource.EndPointSettingsPlaceholder;

    public override Symbol Icon => Symbol.PointScan;

    public override string Label => Resource.EndPointSettingsLabel;

    public override string Description => Resource.EndPointSettingsDescription;

    public override string Token => "EndPoint";
}
