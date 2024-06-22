namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides methods to run tasks asynchronously on a single-threaded apartment (STA) thread.
/// </summary>
public static class StaTask
{
    /// <summary>
    /// Runs the specified action asynchronously on an STA thread.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method is useful for running actions that require STA thread, such as interacting with COM objects
    /// or Windows Forms controls.
    /// </remarks>
    public static Task RunAsync(Action action) =>
        RunAsync(_ => action(), CancellationToken.None);

    /// <summary>
    /// Runs the specified action asynchronously on an STA thread, supporting cancellation.
    /// </summary>
    /// <param name="action">The action to execute. The action takes a <see cref="CancellationToken"/> that is signaled when the operation is cancelled.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method is useful for running actions that require STA thread, such as interacting with COM objects
    /// or Windows Forms controls. The action can be cancelled by signalling the provided <see cref="CancellationToken"/>.
    /// </remarks>
    public static Task RunAsync(Action<CancellationToken> action, CancellationToken cancellationToken)
    {
        var completionSource = new TaskCompletionSource();
        var thread = new Thread(() =>
        {
            try
            {
                action(cancellationToken);
                completionSource.SetResult();
            }
            catch (OperationCanceledException ex) when (ex.CancellationToken == cancellationToken)
            {
                completionSource.SetCanceled(cancellationToken);
            }
            catch (Exception ex)
            {
                completionSource.SetException(ex);
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return completionSource.Task;
    }
}

