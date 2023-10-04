using Eco.Core.Plugins;

namespace jcdcdev.Eco.Core;

public abstract class ConfigBase<TConfig> where TConfig : new()
{
    private static PluginConfig<TConfig>? _config;
    public static TConfig Config => PluginConfig.Config;
    public static PluginConfig<TConfig> PluginConfig => _config ?? throw new InvalidOperationException("Plugin has not been initialized.");

    public static void Initialize(string name) => _config = new PluginConfig<TConfig>(name);

    public static void OnConfigEntryChanged(object o, string name, string configFileName) => _config?.BuildConfigProperties();
}