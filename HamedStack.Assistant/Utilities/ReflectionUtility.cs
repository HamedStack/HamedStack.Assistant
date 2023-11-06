﻿// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local☺

using System.Linq.Expressions;
using System.Reflection;

namespace HamedStack.Assistant.Utilities;

public static class ReflectionUtility
{
    public static MemberExpression? GetMemberExpression(this LambdaExpression expression, bool enforceMemberExpression)
    {
        MemberExpression? memberExpression = null;
        if (expression.Body.NodeType == ExpressionType.Convert)
        {
            var body = (UnaryExpression)expression.Body;
            memberExpression = body.Operand as MemberExpression;
        }
        else if (expression.Body.NodeType == ExpressionType.MemberAccess)
        {
            memberExpression = expression.Body as MemberExpression;
        }
        if (enforceMemberExpression && memberExpression == null)
        {
            throw new ArgumentException("Not a member access", nameof(expression));
        }
        return memberExpression;
    }

    public static TProperty? GetProperty<TClass, TProperty>(TClass instanceType, string propertyName)
                                        where TClass : class
    {
        if (propertyName == null || string.IsNullOrEmpty(propertyName))
            throw new ArgumentNullException(nameof(propertyName), "Value can not be null or empty.");

        object? obj = null;
        var type = instanceType.GetType();
        var info = type.GetTypeInfo().GetProperty(propertyName);
        if (info != null)
            obj = info.GetValue(instanceType, null);
        return (TProperty?)obj;
    }

    public static object? GetProperty(Type instanceType, string propertyName)
    {
        if (propertyName == null || string.IsNullOrEmpty(propertyName))
            throw new ArgumentNullException(nameof(propertyName), "Value can not be null or empty.");

        object? obj = null;
        var info = instanceType.GetTypeInfo().GetProperty(propertyName);
        if (info != null)
            obj = info.GetValue(instanceType, null);
        return obj;
    }

    public static PropertyInfo GetProperty<TModel>(Expression<Func<TModel, object>> expression)
    {
        var memberExpression = GetMemberExpression(expression);
        return (PropertyInfo)memberExpression.Member;
    }

    public static PropertyInfo GetProperty<TModel, T>(Expression<Func<TModel, T>> expression)
    {
        var memberExpression = GetMemberExpression(expression);
        return (PropertyInfo)memberExpression.Member;
    }

    public static PropertyInfo? GetProperty(LambdaExpression expression)
    {
        var memberExpression = GetMemberExpression(expression, true);
        return (PropertyInfo?)memberExpression?.Member;
    }

    public static bool IsMemberExpression<T>(Expression<Func<T, object>> expression)
    {
        return IsMemberExpression<T, object>(expression);
    }

    public static bool IsMemberExpression<T, TU>(Expression<Func<T, TU>> expression)
    {
        return GetMemberExpression(expression, false) != null;
    }

    public static bool MeetsSpecialGenericConstraints(Type genericArgType, Type proposedSpecificType)
    {
        var genericArgTypeInfo = genericArgType.GetTypeInfo();
        var proposedSpecificTypeInfo = proposedSpecificType.GetTypeInfo();
        var gpa = genericArgTypeInfo.GenericParameterAttributes;
        var constraints = gpa & GenericParameterAttributes.SpecialConstraintMask;
        if (constraints == GenericParameterAttributes.None)
        {
            return true;
        }
        if ((constraints & GenericParameterAttributes.ReferenceTypeConstraint) != 0
            && proposedSpecificTypeInfo.IsValueType)
        {
            return false;
        }
        if ((constraints & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0
            && !proposedSpecificTypeInfo.IsValueType)
        {
            return false;
        }
        if ((constraints & GenericParameterAttributes.DefaultConstructorConstraint) != 0
            && proposedSpecificType.GetConstructor(Type.EmptyTypes) == null)
        {
            return false;
        }
        return true;
    }

    public static void SetProperty<TClass>(TClass instanceType, string propertyName, object propertyValue)
                                where TClass : class
    {
        if (propertyName == null || string.IsNullOrEmpty(propertyName))
            throw new ArgumentNullException(nameof(propertyName), "Value can not be null or empty.");

        var type = instanceType.GetType();
        var info = type.GetTypeInfo().GetProperty(propertyName);

        if (info != null)
            info.SetValue(instanceType, Convert.ChangeType(propertyValue, info.PropertyType), null);
    }

    public static void SetProperty(Type instanceType, string propertyName, object propertyValue)
    {
        if (propertyName == null || string.IsNullOrWhiteSpace(propertyName))
            throw new ArgumentNullException(nameof(propertyName), "Value can not be null or empty.");

        var info = instanceType.GetTypeInfo().GetProperty(propertyName);

        if (info != null)
            info.SetValue(instanceType, Convert.ChangeType(propertyValue, info.PropertyType), null);
    }

    private static MemberExpression GetMemberExpression<TModel, T>(Expression<Func<TModel, T>> expression)
    {
        MemberExpression? memberExpression = null;
        if (expression.Body.NodeType == ExpressionType.Convert)
        {
            var body = (UnaryExpression)expression.Body;
            memberExpression = body.Operand as MemberExpression;
        }
        else if (expression.Body.NodeType == ExpressionType.MemberAccess)
        {
            memberExpression = expression.Body as MemberExpression;
        }
        if (memberExpression == null)
        {
            throw new ArgumentException("Not a member access", nameof(expression));
        }
        return memberExpression;
    }
}