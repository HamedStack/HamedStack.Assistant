// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Concurrent;

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Interface for asynchronous pipes.
/// </summary>
/// <typeparam name="T">Type of input and output data.</typeparam>
public interface IAsyncPipeFilter<T>
{
    /// <summary>
    /// Asynchronously processes the input data and returns the processed data.
    /// </summary>
    /// <param name="input">The input data.</param>
    /// <returns>A task representing the processed data.</returns>
    Task<T> ProcessAsync(T input);
}

/// <summary>
/// Interface for synchronous pipes.
/// </summary>
/// <typeparam name="T">Type of input and output data.</typeparam>
public interface IPipeFilter<T>
{
    /// <summary>
    /// Processes the input data and returns the processed data.
    /// </summary>
    /// <param name="input">The input data.</param>
    /// <returns>The processed data.</returns>
    T Process(T input);
}

/// <summary>
/// Fluent Pipeline implementation that supports both synchronous and asynchronous pipes.
/// </summary>
/// <typeparam name="T">Type of input and output data.</typeparam>
public class FluentPipeline<T>
{
    private readonly ConcurrentBag<Func<T, Task<T>>> _parallelPipes = new();
    private readonly List<Func<T, Task<T>>> _pipes = new();
    private Action<Exception>? _onError;
    private Func<T, Task<T>>? _postProcess;
    private Func<T, Task<T>>? _preProcess;

    /// <summary>
    /// Executes the pipeline asynchronously.
    /// </summary>
    /// <param name="input">Initial input data.</param>
    /// <returns>Task representing the final processed data.</returns>
    public async Task<T> ExecuteAsync(T input)
    {
        try
        {
            // PreProcess
            if (_preProcess != null)
            {
                input = await _preProcess(input);
            }

            // Sequential execution
            foreach (var pipe in _pipes)
            {
                input = await pipe(input);
            }

            // Parallel execution
            var parallelTasks = _parallelPipes.Select(pipe => pipe(input)).ToArray();
            await Task.WhenAll(parallelTasks);

            // PostProcess
            if (_postProcess != null)
            {
                input = await _postProcess(input);
            }

            return input;
        }
        catch (Exception ex)
        {
            // Global error handling
            _onError?.Invoke(ex);
            throw;
        }
    }

    /// <summary>
    /// Sets a global OnError function that acts upon an unhandled exception during execution.
    /// </summary>
    /// <param name="action">Function to execute when an exception occurs.</param>
    /// <returns>Updated FluentPipeline object for method chaining.</returns>
    public FluentPipeline<T> OnError(Action<Exception> action)
    {
        _onError = action;
        return this;
    }

    /// <summary>
    /// Registers a parallel synchronous pipe with an optional condition.
    /// </summary>
    /// <param name="pipe">Synchronous pipe to register.</param>
    /// <param name="condition">Optional condition for executing the pipe.</param>
    /// <param name="errorHandler">Optional error handler.</param>
    /// <returns>Updated FluentPipeline object for method chaining.</returns>
    public FluentPipeline<T> RegisterParallel(IPipeFilter<T> pipe, Func<T, bool>? condition = null,
        Action<Exception>? errorHandler = null)
    {
        _parallelPipes.Add(input =>
        {
            if (condition != null && !condition(input))
            {
                return Task.FromResult(input);
            }

            try
            {
                return Task.FromResult(pipe.Process(input));
            }
            catch (Exception ex)
            {
                errorHandler?.Invoke(ex);
                return Task.FromResult(input);
            }
        });

        return this;
    }

    /// <summary>
    /// Registers a parallel asynchronous pipe with an optional condition.
    /// </summary>
    /// <param name="pipe">Asynchronous pipe to register.</param>
    /// <param name="condition">Optional condition for executing the pipe.</param>
    /// <param name="errorHandler">Optional error handler.</param>
    /// <returns>Updated FluentPipeline object for method chaining.</returns>
    public FluentPipeline<T> RegisterParallel(IAsyncPipeFilter<T> pipe, Func<T, bool>? condition = null,
        Action<Exception>? errorHandler = null)
    {
        _parallelPipes.Add(async input =>
        {
            if (condition != null && !condition(input))
            {
                return input;
            }

            try
            {
                return await pipe.ProcessAsync(input);
            }
            catch (Exception ex)
            {
                errorHandler?.Invoke(ex);
                return input;
            }
        });

        return this;
    }

    /// <summary>
    /// Registers a global PostProcess function that executes after all pipes have completed.
    /// </summary>
    /// <param name="action">Function to execute.</param>
    /// <returns>Updated FluentPipeline object for method chaining.</returns>
    public FluentPipeline<T> RegisterPostProcess(Func<T, Task<T>> action)
    {
        _postProcess = action;
        return this;
    }

    /// <summary>
    /// Registers a global PreProcess function that executes before any pipe runs.
    /// </summary>
    /// <param name="action">Function to execute.</param>
    /// <returns>Updated FluentPipeline object for method chaining.</returns>
    public FluentPipeline<T> RegisterPreProcess(Func<T, Task<T>> action)
    {
        _preProcess = action;
        return this;
    }

    /// <summary>
    /// Registers a sequential synchronous pipe with an optional condition.
    /// </summary>
    /// <param name="pipe">Synchronous pipe to register.</param>
    /// <param name="condition">Optional condition for executing the pipe.</param>
    /// <param name="errorHandler">Optional error handler.</param>
    /// <returns>Updated FluentPipeline object for method chaining.</returns>
    public FluentPipeline<T> RegisterSequential(IPipeFilter<T> pipe, Func<T, bool>? condition = null,
        Action<Exception>? errorHandler = null)
    {
        return RegisterSequentialAsync(async input => await Task.FromResult(pipe.Process(input)), condition,
            errorHandler);
    }

    /// <summary>
    /// Registers an asynchronous pipe with an optional condition.
    /// </summary>
    /// <param name="pipe">Asynchronous pipe to register.</param>
    /// <param name="condition">Optional condition for executing the pipe.</param>
    /// <param name="errorHandler">Optional error handler.</param>
    /// <returns>Updated FluentPipeline object for method chaining.</returns>
    public FluentPipeline<T> RegisterSequentialAsync(Func<T, Task<T>> pipe, Func<T, bool>? condition = null,
        Action<Exception>? errorHandler = null)
    {
        _pipes.Add(async input =>
        {
            if (condition != null && !condition(input))
            {
                return input;
            }

            try
            {
                return await pipe(input);
            }
            catch (Exception ex)
            {
                errorHandler?.Invoke(ex);
                return input;
            }
        });

        return this;
    }
}