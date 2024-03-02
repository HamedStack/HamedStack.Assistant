// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace HamedStack.Assistant.Extensions.ValueTaskExtended;

public static class ValueTaskExtensions
{
    public static T Await<T>(this ValueTask<T> task)
    {
        return task.GetAwaiter().GetResult();
    }
    public static void Await(this ValueTask task)
    {
        task.GetAwaiter().GetResult();
    }

    public static async void SafeFireAndForget(this ValueTask @this, bool continueOnCapturedContext = true,
            Action<Exception>? onException = null)
    {
        try
        {
            await @this.ConfigureAwait(continueOnCapturedContext);
        }
        catch (Exception e) when (onException != null)
        {
            onException(e);
        }
    }
}