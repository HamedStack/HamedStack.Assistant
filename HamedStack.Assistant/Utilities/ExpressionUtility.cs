
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using System.Linq.Expressions;
using System.Reflection;

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides utility methods for creating and working with expressions, particularly for
/// generating getters and setters for properties, and for working with attributes and members.
/// </summary>
public static class ExpressionUtility<T>
{
    /// <summary>
    /// Creates a getter for a specified property of the given entity.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity containing the property.</typeparam>
    /// <typeparam name="TProperty">Type of the property for which the getter is being created.</typeparam>
    /// <param name="name">Name of the property.</param>
    /// <returns>A function that gets the value of the specified property from an instance of the entity.</returns>
    /// <example>
    /// This example demonstrates how to create and use a getter for the "Name" and "BirthDate" properties of an "Employee" class:
    /// <code>
    /// var getterNameProperty = ExpressionUtils.CreateGetter&lt;Employee, string&gt;("Name");
    /// var getterBirthDateProperty = ExpressionUtils.CreateGetter&lt;Employee, DateTime&gt;("BirthDate");
    /// var name = getterNameProperty(emp1);
    /// var birthDate = getterBirthDateProperty(emp1);
    /// </code>
    /// </example>
    public static Func<TEntity, TProperty> CreateGetter<TEntity, TProperty>(string name) where TEntity : class
    {
        var instance = Expression.Parameter(typeof(TEntity), "instance");

        var body = Expression.Property(instance, name);

        return Expression.Lambda<Func<TEntity, TProperty>>(body, instance).Compile();
    }

    /// <summary>
    /// Creates a setter for a specified property of the given entity.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity containing the property.</typeparam>
    /// <typeparam name="TProperty">Type of the property for which the setter is being created.</typeparam>
    /// <param name="name">Name of the property.</param>
    /// <returns>An action that sets the value of the specified property on an instance of the entity.</returns>
    /// <example>
    /// This example demonstrates how to create and use a setter for the "Name" and "BirthDate" properties of an "Employee" class:
    /// <code>
    /// var setterNameProperty = ExpressionUtils.CreateSetter&lt;Employee, string&gt;("Name");
    /// var setterBirthDateProperty = ExpressionUtils.CreateSetter&lt;Employee, DateTime&gt;("BirthDate");
    /// setterNameProperty(emp, "John");
    /// setterBirthDateProperty(emp, new DateTime(1990, 6, 5));
    /// </code>
    /// </example>
    public static Action<TEntity, TProperty> CreateSetter<TEntity, TProperty>(string name) where TEntity : class
    {
        var instance = Expression.Parameter(typeof(TEntity), "instance");
        var propertyValue = Expression.Parameter(typeof(TProperty), "propertyValue");

        var body = Expression.Assign(Expression.Property(instance, name), propertyValue);

        return Expression.Lambda<Action<TEntity, TProperty>>(body, instance, propertyValue).Compile();
    }

    /// <summary>
    /// Retrieves a custom attribute from the specified selector.
    /// </summary>
    /// <typeparam name="TAttribute">Type of the attribute being retrieved.</typeparam>
    /// <param name="selector">Expression selector for the attribute.</param>
    /// <returns>The custom attribute of the specified type, or null if it doesn't exist.</returns>
    public static TAttribute? GetCustomAttribute<TAttribute>(
        Expression<Func<T, TAttribute>> selector) where TAttribute : Attribute
    {
        Expression body = selector;
        if (body is LambdaExpression expression) body = expression.Body;
        return body.NodeType switch
        {
            ExpressionType.MemberAccess => ((PropertyInfo)((MemberExpression)body).Member)
                .GetCustomAttribute<TAttribute>(),
            _ => throw new InvalidOperationException()
        };
    }

    /// <summary>
    /// Retrieves the getter for a specific property by its name.
    /// </summary>
    /// <typeparam name="TObject">Type of the object containing the property.</typeparam>
    /// <typeparam name="TProperty">Type of the property.</typeparam>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>A function to get the property value from an object instance.</returns>
    public static Func<TObject, TProperty> GetProperty<TObject, TProperty>(string propertyName)
    {
        var paramExpression = Expression.Parameter(typeof(TObject), "value");

        Expression propertyGetterExpression = Expression.Property(paramExpression, propertyName);

        return Expression.Lambda<Func<TObject, TProperty>>(propertyGetterExpression, paramExpression).Compile();
    }

    /// <summary>
    /// Retrieves the property information for a given expression.
    /// </summary>
    /// <param name="property">Expression pointing to the property.</param>
    /// <returns>Information about the specified property.</returns>
    public static PropertyInfo GetProperty(Expression<Func<T, object>> property)
    {
        LambdaExpression lambda = property;
        MemberExpression memberExpression;

        if (lambda.Body is UnaryExpression expression)
            memberExpression = (MemberExpression)expression.Operand;
        else
            memberExpression = (MemberExpression)lambda.Body;

        return (PropertyInfo)memberExpression.Member;
    }

    /// <summary>
    /// Retrieves the name of the property from the provided expression.
    /// </summary>
    /// <param name="property">Expression pointing to the property.</param>
    /// <returns>Name of the specified property.</returns>
    public static string GetPropertyName(Expression<Func<T, object>> property)
    {
        return GetProperty(property).Name;
    }

    /// <summary>
    /// Sets the value of a deep property (a property that might be nested within other properties or fields) on the given object.
    /// </summary>
    /// <typeparam name="TObject">Type of the target object.</typeparam>
    /// <param name="target">Target object on which the property value should be set.</param>
    /// <param name="propertyToSet">Expression pointing to the property to set.</param>
    /// <param name="valueToSet">Value to set on the property.</param>
    public static void SetDeepValue<TObject>(TObject? target, Expression<Func<TObject, T>> propertyToSet, T valueToSet)
    {
        var members = new List<MemberInfo>();

        var exp = propertyToSet.Body;

        // There is a chain of getters in propertyToSet, with at the beginning a
        // ParameterExpression. We put the MemberInfo of these getters in members. We don't really
        // need the ParameterExpression

        while (exp != null)
            if (exp is MemberExpression mi)
            {
                members.Add(mi.Member);
                exp = mi.Expression;
            }
            else
            {
                if (exp is not ParameterExpression)
                    // We support only a ParameterExpression at the base
                    throw new NotSupportedException();

                break;
            }

        if (members.Count == 0)
            // We need at least a getter
            throw new NotSupportedException();

        // Now we must walk the getters (excluding the last).
        object? targetObject = target;

        // We have to walk the getters from last (most inner) to second (the first one is the one we
        // have to use as a setter)
        for (var i = members.Count - 1; i >= 1; i--)
        {
            var pi = members[i] as PropertyInfo;

            if (pi != null)
            {
                targetObject = pi.GetValue(targetObject);
            }
            else
            {
                var fi = (FieldInfo)members[i];
                targetObject = fi.GetValue(targetObject);
            }
        }

        // The first one is the getter we treat as a setter
        {
            var pi = members[0] as PropertyInfo;

            if (pi != null)
            {
                pi.SetValue(targetObject, valueToSet);
            }
            else
            {
                var fi = (FieldInfo)members[0];
                fi.SetValue(targetObject, valueToSet);
            }
        }
    }

    /// <summary>
    /// Creates a setter action for a specific property by its name.
    /// </summary>
    /// <typeparam name="TObject">Type of the object containing the property.</typeparam>
    /// <typeparam name="TProperty">Type of the property.</typeparam>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>An action to set the property value on an object instance.</returns>
    public static Action<TObject, TProperty> SetProperty<TObject, TProperty>(string propertyName)
    {
        var paramExpression = Expression.Parameter(typeof(TObject));

        var paramExpression2 = Expression.Parameter(typeof(TProperty), propertyName);

        var propertyGetterExpression = Expression.Property(paramExpression, propertyName);

        return Expression.Lambda<Action<TObject, TProperty>>
                (Expression.Assign(propertyGetterExpression, paramExpression2), paramExpression, paramExpression2)
            .Compile();
    }
}