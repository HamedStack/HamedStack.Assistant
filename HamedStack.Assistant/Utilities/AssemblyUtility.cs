// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using HamedStack.Assistant.Extensions.AssemblyExtended;
using System.Reflection;
using System.Runtime.InteropServices;

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides utility methods for operations related to assemblies.
/// </summary>
public static class AssemblyUtility
{
    public static IEnumerable<Assembly> AppDomainContains(params Type[] types)
    {
        return AppDomain.CurrentDomain.GetAssemblies().Where(a => a.Contains(types));
    }

    /// <summary>
    /// Retrieves the entry assembly and all its referenced assemblies.
    /// </summary>
    /// <returns>
    /// A collection of assemblies, including the entry assembly and all its referenced assemblies.
    /// If there's no entry assembly, null is returned.
    /// </returns>
    public static IEnumerable<Assembly>? GetEntryAssemblyWithReferences()
    {
        var listOfAssemblies = new List<Assembly>();
        var mainAsm = Assembly.GetEntryAssembly();

        if (mainAsm == null) return null;

        listOfAssemblies.Add(mainAsm);
        listOfAssemblies.AddRange(mainAsm.GetReferencedAssemblies().Select(Assembly.Load));
        return listOfAssemblies;
    }

    public static string GetRuntimeAssemblyPath(string assemblyName)
    {
        if (string.IsNullOrWhiteSpace(assemblyName))
            throw new ArgumentException($"{nameof(assemblyName)} cannot be null or whitespace.", nameof(assemblyName));
        return Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), assemblyName);
    }

    public static string GetRuntimeDirectory()
    {
        return RuntimeEnvironment.GetRuntimeDirectory();
    }

    public static string GetRuntimeDirectory(out IEnumerable<string> assemblies)
    {
        var path = RuntimeEnvironment.GetRuntimeDirectory();
        assemblies = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories);
        return path;
    }

    public static IEnumerable<Assembly> GetRuntimeDirectoryAssemblies()
    {
        var assemblies = new List<Assembly>();
        var path = RuntimeEnvironment.GetRuntimeDirectory();
        var allDllFiles = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories);
        foreach (var dllFile in allDllFiles)
        {
            try
            {
                var assemblyName = AssemblyName.GetAssemblyName(dllFile);
                var loadedAssembly = Assembly.Load(assemblyName);
                assemblies.Add(loadedAssembly);
            }
            catch
            {
                // ignored
            }
        }
        return assemblies;
    }

    internal static IEnumerable<Assembly> GetAllAppDomainAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

        var assemblyDir = AppDomain.CurrentDomain.BaseDirectory;

        var allDllFiles = Directory.GetFiles(assemblyDir, "*.dll", SearchOption.AllDirectories);

        foreach (var dllFile in allDllFiles)
        {
            try
            {
                var assemblyName = AssemblyName.GetAssemblyName(dllFile);

                if (assemblies.Any(a => a.FullName == assemblyName.FullName)) continue;

                var loadedAssembly = Assembly.Load(assemblyName);
                assemblies.Add(loadedAssembly);
            }
            catch
            {
                // ignored
            }
        }

        return assemblies;
    }
}