using Eco.Core.Plugins;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Shared.Localization;
using jcdcdev.Eco.Core.Extensions;
using jcdcdev.Eco.Core.Models;

namespace jcdcdev.Eco.Core;

public abstract class PluginBase : PluginBase<EmptyConfig> { }

public abstract class PluginBase<TConfig> :
    IModKitPlugin,
    IInitializablePlugin,
    IConfigurablePlugin,
    IDisplayablePlugin,
    IThreadedPlugin where TConfig : new()
{
    protected bool Active;
    private string ModName => this.GetModName();
    private string ConfigFileName => this.GetModName().EnsureEndsWith(".eco");
    private string ModVersion => this.GetModVersion();
    public static TConfig Config => ConfigBase<TConfig>.Config;

    public IPluginConfig PluginConfig => ConfigBase<TConfig>.PluginConfig;

    public object? GetEditObject() => Config;

    public ThreadSafeAction<object, string>? ParamChanged { get; set; }

    public void OnEditObjectChanged(object o, string param) => ConfigBase<TConfig>.OnConfigEntryChanged(o, param, ConfigFileName);

    public string GetDisplayText() => GetStatus();

    public void Initialize(TimedTask timer)
    {
        Active = true;
        ConfigBase<TConfig>.Initialize(ModName);
        InitializeMod(timer);
    }

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