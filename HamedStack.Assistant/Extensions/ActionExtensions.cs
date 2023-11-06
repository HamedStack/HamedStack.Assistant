// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

using HamedStack.Assistant.Extensions.TaskExtended;
using System.Diagnostics;
using System.Linq.Expressions;

namespace HamedStack.Assistant.Extensions.ActionExtended;

public static class ActionExtensions
{
    public static void ExecuteTimeout(this Action action, TimeSpan maxDelay)
    {
        var executionTask = Task.Run(action);
        executionTask.ExecuteTimeout(maxDelay);
    }

    public static TimeSpan GetExecutionTime(this Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        var start = new Stopwatch();
        start.Start();
        action();
        start.Stop();
        return start.Elapsed;
    }

    public static Action NeutralizeException(this Action action, Action? @finally = null)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        return () =>
        {
            try
            {
                action();
            }
            catch
            {
                // ignored
            }
            finally
            {
                @finally?.Invoke();
            }
        };
    }

    public static Action<object>? ToActionObject<T>(this Action<T>? actionT)
    {
        return actionT == null ? null : new Action<object>(o => actionT((T)o));
    }

    public static Expression<Action> ToExpression(this Action action)
    {
        return () => action();
    }

    public static Expression<Action<T0>> ToExpression<T0>(this Action<T0> action)
    {
        return t0 => action(t0);
    }

    public static Expression<Action<T0, T1>> ToExpression<T0, T1>(this Action<T0, T1> action)
    {
        return (t0, t1) => action(t0, t1);
    }

    public static Expression<Action<T0, T1, T2>> ToExpression<T0, T1, T2>(this Action<T0, T1, T2> action)
    {
        return (t0, t1, t2) => action(t0, t1, t2);
    }

    public static Expression<Action<T0, T1, T2, T3>> ToExpression<T0, T1, T2, T3>(this Action<T0, T1, T2, T3> action)
    {
        return (t0, t1, t2, t3) => action(t0, t1, t2, t3);
    }

    public static Expression<Action<T0, T1, T2, T3, T4>> ToExpression<T0, T1, T2, T3, T4>(
        this Action<T0, T1, T2, T3, T4> action)
    {
        return (t0, t1, t2, t3, t4) => action(t0, t1, t2, t3, t4);
    }

    public static Expression<Action<T0, T1, T2, T3, T4, T5>> ToExpression<T0, T1, T2, T3, T4, T5>(
        this Action<T0, T1, T2, T3, T4, T5> action)
    {
        return (t0, t1, t2, t3, t4, t5) => action(t0, t1, t2, t3, t4, t5);
    }

    public static Expression<Action<T0, T1, T2, T3, T4, T5, T6>> ToExpression<T0, T1, T2, T3, T4, T5, T6>(
        this Action<T0, T1, T2, T3, T4, T5, T6> action)
    {
        return (t0, t1, t2, t3, t4, t5, t6) => action(t0, t1, t2, t3, t4, t5, t6);
    }

    public static Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7>> ToExpression<T0, T1, T2, T3, T4, T5, T6, T7>(
        this Action<T0, T1, T2, T3, T4, T5, T6, T7> action)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7) => action(t0, t1, t2, t3, t4, t5, t6, t7);
    }

    public static Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8>> ToExpression<T0, T1, T2, T3, T4, T5, T6, T7,
        T8>(this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8) => action(t0, t1, t2, t3, t4, t5, t6, t7, t8);
    }

    public static Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>> ToExpression<T0, T1, T2, T3, T4, T5, T6,
        T7, T8, T9>(this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) => action(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
    }

    public static Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> ToExpression<T0, T1, T2, T3, T4, T5,
        T6, T7, T8, T9, T10>(this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => action(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
    }

    public static Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> ToExpression<T0, T1, T2, T3, T4,
        T5, T6, T7, T8, T9, T10, T11>(this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            action(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
    }

    public static Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> ToExpression<T0, T1, T2, T3,
        T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            action(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
    }

    public static Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> ToExpression<T0, T1,
        T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            action(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
    }

    public static Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> ToExpression<T0,
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            action(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
    }

    public static Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> ToExpression<
        T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            action(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
    }
}