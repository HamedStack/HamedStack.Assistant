namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Represents the result of a command execution.
/// </summary>
public class CommandResult
{
    /// <summary>
    /// Gets or sets the standard error from the executed command.
    /// </summary>
    public string Error { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the exit code of the executed command.
    /// </summary>
    public int ExitCode { get; set; }

    /// <summary>
    /// Gets or sets the standard output from the executed command.
    /// </summary>
    public string Output { get; set; } = string.Empty;
}