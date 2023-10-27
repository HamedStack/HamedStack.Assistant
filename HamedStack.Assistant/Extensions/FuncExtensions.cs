// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

using System.Linq.Expressions;
using HamedStack.Assistant.Extensions.TaskExtended;

namespace HamedStack.Assistant.Extensions.FuncExtended;

public static class FuncExtensions
{
    public static void ExecuteTimeout<T>(this Func<T> func, TimeSpan maxDelay)
    {
        var executionTask = Task.Run(() =>
        {
            func();
        });
        executionTask.ExecuteTimeout(maxDelay);
    }
    public static Func<T2, T1, TR> Swap<T1, T2, TR>(this Func<T1, T2, TR> f)
    {
        return (t2, t1) => f(t1, t2);
    }

    public static Expression<Func<T>> ToExpression<T>(this Func<T> func)
    {
        return () => func();
    }

    public static Expression<Func<T0, TR>> ToExpression<T0, TR>(this Func<T0, TR> func)
    {
        return t0 => func(t0);
    }

    public static Expression<Func<T0, T1, TR>> ToExpression<T0, T1, TR>(this Func<T0, T1, TR> func)
    {
        return (t0, t1) => func(t0, t1);
    }

    public static Expression<Func<T0, T1, T2, TR>> ToExpression<T0, T1, T2, TR>(this Func<T0, T1, T2, TR> func)
    {
        return (t0, t1, t2) => func(t0, t1, t2);
    }

    public static Expression<Func<T0, T1, T2, T3, TR>> ToExpression<T0, T1, T2, T3, TR>(
        this Func<T0, T1, T2, T3, TR> func)
    {
        return (t0, t1, t2, t3) => func(t0, t1, t2, t3);
    }

    public static Expression<Func<T0, T1, T2, T3, T4, TR>> ToExpression<T0, T1, T2, T3, T4, TR>(
        this Func<T0, T1, T2, T3, T4, TR> func)
    {
        return (t0, t1, t2, t3, t4) => func(t0, t1, t2, t3, t4);
    }

    public static Expression<Func<T0, T1, T2, T3, T4, T5, TR>> ToExpression<T0, T1, T2, T3, T4, T5, TR>(
        this Func<T0, T1, T2, T3, T4, T5, TR> func)
    {
        return (t0, t1, t2, t3, t4, t5) => func(t0, t1, t2, t3, t4, t5);
    }

    public static Expression<Func<T0, T1, T2, T3, T4, T5, T6, TR>> ToExpression<T0, T1, T2, T3, T4, T5, T6, TR>(
        this Func<T0, T1, T2, T3, T4, T5, T6, TR> func)
    {
        return (t0, t1, t2, t3, t4, t5, t6) => func(t0, t1, t2, t3, t4, t5, t6);
    }

    public static Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, TR>> ToExpression<T0, T1, T2, T3, T4, T5, T6, T7, TR>(
        this Func<T0, T1, T2, T3, T4, T5, T6, T7, TR> func)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7) => func(t0, t1, t2, t3, t4, t5, t6, t7);
    }

    public static Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TR>> ToExpression<T0, T1, T2, T3, T4, T5, T6, T7,
        T8, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TR> func)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8) => func(t0, t1, t2, t3, t4, t5, t6, t7, t8);
    }

    public static Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>> ToExpression<T0, T1, T2, T3, T4, T5, T6,
        T7, T8, T9, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) => func(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
    }

    public static Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>> ToExpression<T0, T1, T2, T3, T4, T5,
        T6, T7, T8, T9, T10, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => func(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
    }

    public static Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>> ToExpression<T0, T1, T2, T3,
        T4, T5, T6, T7, T8, T9, T10, T11, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            func(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
    }

    public static Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>> ToExpression<T0, T1, T2,
        T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
        this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            func(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
    }

    public static Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>> ToExpression<T0, T1,
        T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
        this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            func(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
    }

    public static Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>> ToExpression<T0,
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
        this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            func(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
    }

    public static Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>>
        ToExpression<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            func(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
    }
}