using Eco.Core.Plugins;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Server;
using Eco.Shared.Localization;
using Eco.WebServer.Web.Controllers;
using jcdcdev.Eco.Core.Extensions;
using jcdcdev.Eco.Core.Models;
using PluginManager = Eco.Core.PluginManager;

namespace jcdcdev.Eco.Core;

public abstract class PluginBase : PluginBase<EmptyConfig> { }

public abstract class PluginBase<TConfig> :
    IModKitPlugin,
    IInitializablePlugin,
    IConfigurablePlugin,
    IDisplayablePlugin,
    IThreadedPlugin where TConfig : new()
{
    protected PluginBase()
    {
        ModName = GetType().AssemblyName();
        ModVersion = GetType().SemVer();
        _pluginConfig = new PluginConfig<TConfig>(ModName);
        PluginConfig = _pluginConfig;
        Config = _pluginConfig.Config;
    }

    public string ModVersion { get; }

    public TConfig Config { get; }

    public string ModName { get; }

    protected bool Active;
    private readonly PluginConfig<TConfig> _pluginConfig;
    public IPluginConfig PluginConfig { get; }

    public object? GetEditObject() => Config;

    public ThreadSafeAction<object, string>? ParamChanged { get; set; }

    public void OnEditObjectChanged(object o, string param)
    {
        _pluginConfig.BuildConfigProperties();
        OnConfigChanged(param);
    }

    protected virtual void OnConfigChanged(string propertyChanged) { }

    public string GetDisplayText() => GetStatus();

    public void Initialize(TimedTask timer)
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

    public string GetCategory() => Localizer.DoStr("Mods");

    public async Task ShutdownAsync()
    {
        Active = false;
        await ShutdownMod();
    }

    public void Run()
    {
        var builder = ColorLogBuilder.Create();
        builder.Append($"{ModName}", ConsoleColor.DarkCyan);
        builder.Append(" - ");
        builder.Append($"{ModVersion}", ConsoleColor.DarkYellow);
        Logger.Write(builder);
        RunMod();
    }

    protected virtual void BuildStatusText(LocStringBuilder sb) { }

    protected virtual Task ShutdownMod() => Task.CompletedTask;

    public override string ToString() => ModName;

    protected virtual void RunMod() { }

    protected virtual void InitializeMod(TimedTask timer) { }
}