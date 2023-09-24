// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable UnusedType.Global

namespace HamedStack.Assistant.Implementations.Internals;

/// <summary>
/// Provides a lightweight object-to-object mapper, enabling the copying of properties from one object to another.
/// </summary>
/// <remarks>
/// This class is designed for efficient and simplistic property mapping between objects of different types. 
/// Property mappings are cached to optimize subsequent copy operations.
/// </remarks>
internal class LightObjectMapper : ObjectCopyBase
{
    /// <summary>
    /// Caches the property mappings for source and target types.
    /// </summary>
    private readonly Dictionary<string, PropertyMap[]> _maps = new();

    /// <summary>
    /// Copies the properties of the source object to the target object.
    /// </summary>
    /// <param name="source">The source object from which to copy properties.</param>
    /// <param name="target">The target object to which properties are copied.</param>
    /// <remarks>
    /// Only matching properties by name and type will be copied.
    /// If the source and target types haven't been mapped before, it will create a new mapping before performing the copy.
    /// </remarks>
    internal override void Copy(object source, object target)
    {
        var sourceType = source.GetType();
        var targetType = target.GetType();

        var key = GetMapKey(sourceType, targetType);
        if (!_maps.ContainsKey(key)) MapTypes(sourceType, targetType);

        var propMap = _maps[key];

        for (var i = 0; i < propMap.Length; i++)
        {
            var prop = propMap[i];
            var sourceValue = prop.SourceProperty?.GetValue(source, null);
            prop.DestinationProperty?.SetValue(target, sourceValue, null);
        }
    }

    /// <summary>
    /// Maps the properties of the source type to the target type.
    /// </summary>
    /// <param name="source">The source type to map.</param>
    /// <param name="target">The target type to which properties are mapped.</param>
    /// <remarks>
    /// Property mappings are cached to optimize subsequent copy operations.
    /// </remarks>
    internal override void MapTypes(Type source, Type target)
    {
        var key = GetMapKey(source, target);
        if (_maps.ContainsKey(key)) return;
        var props = GetMatchingProperties(source, target);
        _maps.Add(key, props.ToArray());
    }
}