using Eco.Core.Plugins;
using jcdcdev.Eco.Core.Extensions;
using jcdcdev.Eco.Core.Models;

namespace jcdcdev.Eco.Core;

public abstract class ConfigBase : ConfigBase<EmptyConfig>
{
}

public abstract class ConfigBase<TConfig> where TConfig : new()
{
    public static TConfig Config => _config!.Config;
    public static PluginConfig<TConfig> PluginConfig => _config!;
    private static string FileName => ModKitExtensions.Name;

    private static PluginConfig<TConfig>? _config;

    public static void Initialize()
    {
        _config = new PluginConfig<TConfig>(ModKitExtensions.Name);
        _config.SaveAsAsync(FileName).GetAwaiter().GetResult();
    }

    public static void OnConfigEntryChanged(object o, string name)
    {
    }
}