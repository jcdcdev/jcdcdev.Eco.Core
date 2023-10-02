using System.Globalization;
using Eco.Shared.Localization;
using jcdcdev.Eco.Core.Controllers;
using jcdcdev.Eco.Core.Models;

namespace jcdcdev.Eco.Core;

internal class CorePlugin : PluginBase<CoreConfig>
{
    protected override void RunMod()
    {
        while (Active)
        {
            StoreController.Update();
            Thread.Sleep(500);
        }
    }
    
    protected override void BuildStatusText(LocStringBuilder sb)
    {
        sb.AppendLineNTStr($"Updated :{StoreController.Data.Updated.ToString(CultureInfo.InvariantCulture)}");
        sb.AppendLineNTStr($"Store Count :{StoreController.Data.Stores.Count}");
        base.BuildStatusText(sb);
    }
}