using Eco.Core.Plugins;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Shared.Localization;
using jcdcdev.Eco.Core.Extensions;
using PluginManager = Eco.Core.PluginManager;

namespace jcdcdev.Eco.Core;

public abstract class PluginBase<TConfig> : PluginBase,
    IConfigurablePlugin where TConfig : new()
{
    private string ConfigFileName => ModName.EnsureEndsWith(".eco");

    public override void Initialize(TimedTask timer)
    {
        base.Initialize(timer);
        _pluginConfig.SaveAsAsync(ConfigFileName).GetAwaiter().GetResult();
    }

    public TConfig Config { get; }
    private readonly PluginConfig<TConfig> _pluginConfig;
    public object? GetEditObject() => Config;
    public IPluginConfig PluginConfig { get; }


    public ThreadSafeAction<object, string>? ParamChanged { get; set; }

    public void OnEditObjectChanged(object o, string param) => OnConfigChanged(param);

    protected virtual void OnConfigChanged(string propertyChanged) { }

    protected PluginBase()
    {
        _pluginConfig = new PluginConfig<TConfig>(ModName);
        PluginConfig = _pluginConfig;
        Config = _pluginConfig.Config;
    }
}

public abstract class PluginBase :
    IModKitPlugin,
    IInitializablePlugin,
    IDisplayablePlugin,
    IThreadedPlugin
{
    protected PluginBase()
    {
        ModName = GetType().AssemblyName();
        ModVersion = GetType().SemVer();
    }

    public string ModVersion { get; }

    public string ModName { get; }

    protected bool Active;
    
    public string GetDisplayText() => GetStatus();

    public virtual void Initialize(TimedTask timer)
    {
        Active = true;
        InitializeMod(timer);
        PluginManager.Controller.RunIfOrWhenInited(PluginsInitialized);
    }

    protected virtual void PluginsInitialized() { }

    public string GetStatus()
    {
        var sb = new LocStringBuilder();
        sb.AppendLineNTStr($"Version - {ModVersion}");
        sb.AppendLine();
        BuildStatusText(sb);

        return sb.ToString() ?? string.Empty;
    }

    public string GetCategory() => Localizer.DoStr("jcdcdev.Eco");

    public async Task ShutdownAsync()
    {
        Active = false;
        await ShutdownMod();
    }

    public void Run()
    {
        var builder = ColorLogBuilder.Create();
        builder.Append("- ", ConsoleColor.DarkGray);
        builder.Append($"({ModVersion})", ConsoleColor.DarkYellow);
        builder.Log(ModName);
        RunMod();
    }

    protected virtual void BuildStatusText(LocStringBuilder sb) { }

    protected virtual Task ShutdownMod() => Task.CompletedTask;

    public override string ToString() => ModName.Split(".").LastOrDefault() ?? ModName;

    protected virtual void RunMod() { }

    protected virtual void InitializeMod(TimedTask timer) { }
}