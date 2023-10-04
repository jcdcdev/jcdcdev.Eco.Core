using System.Globalization;
using Eco.Shared.Localization;
using jcdcdev.Eco.Core.Models;
using jcdcdev.Eco.Core.Services;

namespace jcdcdev.Eco.Core;

public class CorePlugin : PluginBase<CoreConfig>
{
    protected override void RunMod()
    {
        while (Active)
        {
            StoreService.Update();
            Thread.Sleep(500);
        }
    }

    protected override void BuildStatusText(LocStringBuilder sb)
    {
        sb.AppendLineNTStr($"Updated: {StoreService.Data.Updated.ToString(CultureInfo.InvariantCulture)}");
        sb.AppendLineNTStr($"Store Count: {StoreService.Data.Stores.Count}");
        base.BuildStatusText(sb);
    }
}