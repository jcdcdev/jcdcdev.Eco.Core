using Eco.Core.Plugins;

namespace jcdcdev.Eco.Core;

public abstract class ConfigBase<TConfig> where TConfig : new()
{
    public static TConfig Config => _config!.Config;
    public static PluginConfig<TConfig> PluginConfig => _config!;

    private static PluginConfig<TConfig>? _config;

    public static void Initialize(string name)
    {
        _config = new PluginConfig<TConfig>(name);
        _config.SaveAsAsync(name).GetAwaiter().GetResult();
    }

    public static void OnConfigEntryChanged(object o, string name)
    {
    }
}