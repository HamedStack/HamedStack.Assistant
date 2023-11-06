// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using System.Runtime.CompilerServices;

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides utility methods to obtain information about the currently executing source file.
/// </summary>
public static class CurrentFileUtility
{
    /// <summary>
    /// Retrieves the directory of the currently executing source file.
    /// </summary>
    /// <param name="file">
    /// The path of the current file. This parameter is automatically populated by the compiler.
    /// </param>
    /// <returns>The directory of the currently executing source file.</returns>
    public static string Directory([CallerFilePath] string file = "") => System.IO.Path.GetDirectoryName(file)!;

    /// <summary>
    /// Retrieves the full path of the currently executing source file.
    /// </summary>
    /// <param name="file">
    /// The path of the current file. This parameter is automatically populated by the compiler.
    /// </param>
    /// <returns>The full path of the currently executing source file.</returns>
    public static string Path([CallerFilePath] string file = "") => file;

    /// <summary>
    /// Computes the absolute path based on a relative path and the location of the currently
    /// executing source file.
    /// </summary>
    /// <param name="relative">The relative path to be combined with the current file directory.</param>
    /// <param name="file">
    /// The path of the current file. This parameter is automatically populated by the compiler.
    /// </param>
    /// <returns>
    /// The absolute path resulting from combining the given relative path with the current file directory.
    /// </returns>
    public static string Relative(string relative, [CallerFilePath] string file = "")
    {
        var directory = System.IO.Path.GetDirectoryName(file)!;
        return System.IO.Path.Combine(directory, relative);
    }
}