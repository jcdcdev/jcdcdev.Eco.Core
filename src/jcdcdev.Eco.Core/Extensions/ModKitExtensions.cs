using System.Reflection;

namespace jcdcdev.Eco.Core.Extensions;

public static class ModKitExtensions
{
    private static AssemblyName AssemblyName(Type type) => Assembly.GetAssembly(type)?.GetName() ?? throw new InvalidOperationException();

    public static string GetModVersion<TConfig>(this PluginBase<TConfig> plugin) where TConfig : new()
    {
        var version = AssemblyName(plugin.GetType()).Version ?? new Version(0, 1, 0);
        return $"{version.Major}.{version.Minor}.{version.Build}";
    }

    public static string GetModName<TConfig>(this PluginBase<TConfig> plugin) where TConfig : new() =>
        AssemblyName(plugin.GetType()).Name ?? throw new Exception("Unable to determine mod name");
}