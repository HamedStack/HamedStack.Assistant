using System.Diagnostics;

// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides methods for executing shell commands.
/// </summary>
public static class CommandLineUtility
{
    /// <summary>
    /// Executes a shell command asynchronously.
    /// </summary>
    /// <param name="command">The command to be executed.</param>
    /// <param name="workingDirectory">Optional working directory from which the command is executed.</param>
    /// <param name="timeout">Optional timeout after which the command execution will be halted.</param>
    /// <param name="shellPath">Optional path to the shell executable. By default, it uses "/bin/bash" for Unix/Mac and "cmd" for Windows.</param>
    /// <param name="shellArgs">Optional arguments for the shell executable. By default, it uses "-c" for Unix/Mac and "/c" for Windows to execute the command.</param>
    /// <param name="onOutputReceived">Optional action to be performed when output data is received.</param>
    /// <param name="onErrorReceived">Optional action to be performed when error data is received.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the command execution.</param>
    /// <returns>A <see cref="CommandResult"/> containing the output, error, and exit code of the executed command or null if execution failed or did not pass the outputCheck.</returns>
    public static async Task<CommandResult?> ExecuteCommandAsync(
        string command,
        string? workingDirectory = null,
        int? timeout = null,
        string? shellPath = null,
        string? shellArgs = null,
        Action<string>? onOutputReceived = null,
        Action<string>? onErrorReceived = null,
        CancellationToken? cancellationToken = null
    )
    {
        var result = new CommandResult();
        try
        {
            if (shellPath == null)
            {
                if (Environment.OSVersion.Platform == PlatformID.Unix ||
                    Environment.OSVersion.Platform == PlatformID.MacOSX)
                {
                    shellPath = "/bin/bash";
                    shellArgs ??= "-c";
                }
                else
                {
                    shellPath = "cmd";
                    shellArgs ??= "/c";
                }
            }

            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = shellPath,
                Arguments = $"{shellArgs} {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory ?? Directory.GetCurrentDirectory()
            };

            process.OutputDataReceived += (_, e) =>
            {
                if (e.Data != null)
                {
                    onOutputReceived?.Invoke(e.Data);
                }
            };

            process.ErrorDataReceived += (_, e) =>
            {
                if (e.Data != null)
                {
                    onErrorReceived?.Invoke(e.Data);
                }
            };

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (timeout.HasValue)
            {
                if (!process.WaitForExit(timeout.Value))
                {
                    throw new TimeoutException("The command execution timed out.");
                }
            }
            else
            {
                await process.WaitForExitAsync(cancellationToken ?? CancellationToken.None);
            }

            result.Output = await process.StandardOutput.ReadToEndAsync();
            result.Error = await process.StandardError.ReadToEndAsync();
            result.ExitCode = process.ExitCode;

            return result;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Executes a shell command synchronously.
    /// </summary>
    /// <param name="command">The command to be executed.</param>
    /// <param name="workingDirectory">Optional working directory from which the command is executed.</param>
    /// <param name="timeout">Optional timeout in milliseconds after which the command execution will be halted.</param>
    /// <param name="shellPath">Optional path to the shell executable. By default, it uses "/bin/bash" for Unix/Mac and "cmd" for Windows.</param>
    /// <param name="shellArgs">Optional arguments for the shell executable. By default, it uses "-c" for Unix/Mac and "/c" for Windows to execute the command.</param>
    /// <param name="onOutputReceived">Optional action to be performed when output data is received.</param>
    /// <param name="onErrorReceived">Optional action to be performed when error data is received.</param>
    /// <returns>A <see cref="CommandResult"/> containing the output, error, and exit code of the executed command or null if execution failed or did not pass the outputCheck.</returns>
    public static CommandResult? ExecuteCommand(
        string command,
        string? workingDirectory = null,
        int? timeout = null,
        string? shellPath = null,
        string? shellArgs = null,
        Action<string>? onOutputReceived = null,
        Action<string>? onErrorReceived = null)
    {
        var result = new CommandResult();

        try
        {
            if (shellPath == null)
            {
                if (Environment.OSVersion.Platform == PlatformID.Unix ||
                    Environment.OSVersion.Platform == PlatformID.MacOSX)
                {
                    shellPath = "/bin/bash";
                    shellArgs ??= "-c";
                }
                else
                {
                    shellPath = "cmd";
                    shellArgs ??= "/c";
                }
            }

            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = shellPath,
                Arguments = $"{shellArgs} {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory ?? Directory.GetCurrentDirectory()
            };

            process.OutputDataReceived += (_, e) =>
            {
                if (e.Data != null)
                {
                    onOutputReceived?.Invoke(e.Data);
                }
            };

            process.ErrorDataReceived += (_, e) =>
            {
                if (e.Data != null)
                {
                    onErrorReceived?.Invoke(e.Data);
                }
            };

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (timeout.HasValue)
            {
                if (!process.WaitForExit(timeout.Value))
                {
                    throw new TimeoutException("The command execution timed out.");
                }
            }
            else
            {
                process.WaitForExit();
            }

            result.Output = process.StandardOutput.ReadToEnd();
            result.Error = process.StandardError.ReadToEnd();
            result.ExitCode = process.ExitCode;

            return result;
        }
        catch
        {
            return null;
        }
    }
}