// ReSharper disable UnusedMember.Global
namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides utility methods for file operations.
/// </summary>
public static class FileUtility
{
    /// <summary>
    /// Copies a file from a specified destination to a target location. If the target directory
    /// does not exist, it will be created.
    /// </summary>
    /// <param name="destination">The path of the file to copy.</param>
    /// <param name="target">The path to where the file should be copied.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when unable to determine the directory name for the target.
    /// </exception>
    public static void Copy(string destination, string target)
    {
        var directory = Path.GetDirectoryName(target);
        Directory.CreateDirectory(directory ?? throw new InvalidOperationException());
        File.Copy(destination, target);
    }
}