﻿// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Implementations.Internals;

internal static class AsyncHelper
{
    private static readonly TaskFactory TaskFactoryHelper = new(CancellationToken.None,
        TaskCreationOptions.None,
        TaskContinuationOptions.None,
        TaskScheduler.Default);

    internal static TResult RunSync<TResult>(Func<Task<TResult>> func)
    {
        return TaskFactoryHelper
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    internal static void RunSync(Func<Task> func)
    {
        TaskFactoryHelper
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    internal static void RunSync(Task task)
    {
        TaskFactoryHelper
            .StartNew(() => task)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    internal static TResult RunSync<TResult>(Task<TResult> task)
    {
        return TaskFactoryHelper
            .StartNew(() => task)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }
}