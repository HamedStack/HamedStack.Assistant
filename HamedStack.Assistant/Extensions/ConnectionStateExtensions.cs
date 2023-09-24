﻿// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

using System.Data;
using HamedStack.Assistant.Extensions.ArrayExtended;

namespace HamedStack.Assistant.Extensions.ConnectionStateExtended;

public static class ConnectionStateExtensions
{
    public static bool In(this ConnectionState @this, params ConnectionState[] values)
    {
        return values.IndexOf(@this) != -1;
    }

    public static bool NotIn(this ConnectionState @this, params ConnectionState[] values)
    {
        return values.IndexOf(@this) == -1;
    }
}