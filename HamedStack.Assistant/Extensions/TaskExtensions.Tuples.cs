// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

using System.Runtime.CompilerServices;

namespace HamedStack.Assistant.Extensions.TaskExtended;

public static partial class TaskExtensions
{
    public static TaskAwaiter<(T1, T2)> GetAwaiter<T1, T2>(this (Task<T1>, Task<T2>) tuple)
    {
        async Task<(T1, T2)> UnifyTasks()
        {
            var (task1, task2) = tuple;
            await Task.WhenAll(task1, task2);
            return (task1.Result, task2.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3)> GetAwaiter<T1, T2, T3>(this (Task<T1>, Task<T2>, Task<T3>) tuple)
    {
        async Task<(T1, T2, T3)> UnifyTasks()
        {
            var (task1, task2, task3) = tuple;
            await Task.WhenAll(task1, task2, task3);
            return (task1.Result, task2.Result, task3.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3, T4)> GetAwaiter<T1, T2, T3, T4>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>) tuple)
    {
        async Task<(T1, T2, T3, T4)> UnifyTasks()
        {
            var (task1, task2, task3, task4) = tuple;
            await Task.WhenAll(task1, task2, task3, task4);
            return (task1.Result, task2.Result, task3.Result, task4.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3, T4, T5)> GetAwaiter<T1, T2, T3, T4, T5>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>) tuple)
    {
        async Task<(T1, T2, T3, T4, T5)> UnifyTasks()
        {
            var (task1, task2, task3, task4, task5) = tuple;
            await Task.WhenAll(task1, task2, task3, task4, task5);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3, T4, T5, T6)> GetAwaiter<T1, T2, T3, T4, T5, T6>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>) tuple)
    {
        async Task<(T1, T2, T3, T4, T5, T6)> UnifyTasks()
        {
            var (task1, task2, task3, task4, task5, task6) = tuple;
            await Task.WhenAll(task1, task2, task3, task4, task5, task6);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7)> GetAwaiter<T1, T2, T3, T4, T5, T6, T7>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>) tuple)
    {
        async Task<(T1, T2, T3, T4, T5, T6, T7)> UnifyTasks()
        {
            var (task1, task2, task3, task4, task5, task6, task7) = tuple;
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8)> GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>) tuple)
    {
        async Task<(T1, T2, T3, T4, T5, T6, T7, T8)> UnifyTasks()
        {
            var (task1, task2, task3, task4, task5, task6, task7, task8) = tuple;
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>) tuple)
    {
        async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> UnifyTasks()
        {
            var (task1, task2, task3, task4, task5, task6, task7, task8, task9) = tuple;
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8, task9);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result, task9.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>, Task<T10>) tuple)
    {
        async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> UnifyTasks()
        {
            var (task1, task2, task3, task4, task5, task6, task7, task8, task9, task10) = tuple;
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result, task9.Result, task10.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>, Task<T10>, Task<T11>) tuple)
    {
        async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> UnifyTasks()
        {
            var (task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11) = tuple;
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result, task9.Result, task10.Result, task11.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>, Task<T10>, Task<T11>, Task<T12>) tuple)
    {
        async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> UnifyTasks()
        {
            var (task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12) = tuple;
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result, task9.Result, task10.Result, task11.Result, task12.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>, Task<T10>, Task<T11>, Task<T12>, Task<T13>) tuple)
    {
        async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> UnifyTasks()
        {
            var (task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13) = tuple;
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result, task9.Result, task10.Result, task11.Result, task12.Result, task13.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>, Task<T10>, Task<T11>, Task<T12>, Task<T13>, Task<T14>) tuple)
    {
        async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> UnifyTasks()
        {
            var (task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14) = tuple;
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result, task9.Result, task10.Result, task11.Result, task12.Result, task13.Result, task14.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
    public static TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>, Task<T10>, Task<T11>, Task<T12>, Task<T13>, Task<T14>, Task<T15>) tuple)
    {
        async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> UnifyTasks()
        {
            var (task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14, task15) = tuple;
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14, task15);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result, task9.Result, task10.Result, task11.Result, task12.Result, task13.Result, task14.Result, task15.Result);
        }

        return UnifyTasks().GetAwaiter();
    }
}