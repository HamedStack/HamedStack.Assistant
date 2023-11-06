// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Implementations;

/// <summary>
/// Provides a mechanism for exclusive asynchronous locking.
/// </summary>
public class AsyncLock : IDisposable
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    /// <summary>
    /// Releases the held lock and cleans up the resources.
    /// </summary>
    public void Dispose()
    {
        if (_semaphoreSlim.CurrentCount == 0) _semaphoreSlim.Release();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Asynchronously acquires an exclusive lock.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous lock acquisition, with a result of the acquired <see cref="AsyncLock"/>.
    /// </returns>
    public async Task<AsyncLock> LockAsync()
    {
        await _semaphoreSlim.WaitAsync().ConfigureAwait(false);
        return this;
    }
}