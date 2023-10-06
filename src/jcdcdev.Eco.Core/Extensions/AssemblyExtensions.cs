using System.Reflection;

namespace jcdcdev.Eco.Core.Extensions;

public static class AssemblyExtensions
{
    private static AssemblyName GetAssemblyName(Type type) => Assembly.GetAssembly(type)?.GetName() ?? throw new InvalidOperationException();

    public static string SemVer(this Type type)
    {
        var version = GetAssemblyName(type).Version ?? new Version(0, 1, 0);
        return $"{version.Major}.{version.Minor}.{version.Build}";
    }
    
    public static string AssemblyName(this Type type) => GetAssemblyName(type).Name ?? throw new Exception("Unable to determine mod name");
    
}