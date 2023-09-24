// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using System.Reflection;

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides utility methods for operations related to assemblies.
/// </summary>
public static class AssemblyUtility
{
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
}