// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using HamedStack.Assistant.Extensions.AssemblyExtended;
using System.Reflection;

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

    public static IEnumerable<Assembly> FindAssembliesOfInterface(Type interfaceType)
    {
        var appAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        var assemblies = new HashSet<Assembly>();

        foreach (var assembly in appAssemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract) continue;

                var interfaces = type.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (@interface == interfaceType ||
                        (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == interfaceType))
                    {
                        assemblies.Add(assembly);
                        break;
                    }
                }
            }
        }

        return assemblies;
    }

    public static IEnumerable<Type> FindImplementationsOfInterface(Type interfaceType)
    {
        var appAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        var implementations = new HashSet<Type>();

        foreach (var assembly in appAssemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract) continue;

                var interfaces = type.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (@interface == interfaceType ||
                        (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == interfaceType))
                    {
                        implementations.Add(type);
                        break;
                    }
                }
            }
        }

        return implementations;
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
}