// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable CommentTypo

using System.Collections.Concurrent;
using System.Reflection;

namespace HamedStack.Assistant.Implementations.Internals;

/// <summary>
/// Represents the base class for object-to-object copying operations. 
/// Provides functionalities to map and copy properties between objects of different types.
/// </summary>
internal abstract class ObjectCopyBase
{
    /// <summary>
    /// Caches property information for different types for optimized access.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> Cache = new();

    /// <summary>
    /// Copies the properties from the source object to the target object.
    /// </summary>
    /// <param name="source">The source object from which to copy properties.</param>
    /// <param name="target">The target object to which properties will be copied.</param>
    internal abstract void Copy(object source, object target);

    /// <summary>
    /// Maps the properties of the source type to the target type.
    /// </summary>
    /// <param name="source">The source type to map from.</param>
    /// <param name="target">The target type to map to.</param>
    internal abstract void MapTypes(Type source, Type target);

    /// <summary>
    /// Generates a unique key for the combination of source and target types.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>A unique string key for the combination of source and target types.</returns>
    protected static string GetMapKey(Type sourceType, Type targetType)
    {
        var keyName = "Copy_";
        keyName += sourceType.FullName?.Replace(".", "_").Replace("+", "_");
        keyName += "_";
        keyName += targetType.FullName?.Replace(".", "_").Replace("+", "_");

        return keyName;
    }

    /// <summary>
    /// Retrieves a collection of <see cref="PropertyMap"/> objects that represent matching properties between the source and target types.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>An IEnumerable of <see cref="PropertyMap"/> representing matching properties between the two types.</returns>
    protected static IEnumerable<PropertyMap> GetMatchingProperties
        (Type sourceType, Type targetType)
    {
        var sourceProperties = Cache.GetOrAdd(sourceType, sourceType.GetProperties());
        var targetProperties = Cache.GetOrAdd(targetType, targetType.GetProperties());

        return (from s in sourceProperties
                from t in targetProperties
                where s.Name == t.Name &&
                      s.CanRead &&
                      t.CanWrite &&
                      s.PropertyType == t.PropertyType
                select new PropertyMap
                {
                    SourceProperty = s,
                    DestinationProperty = t
                }).ToList();
    }
}