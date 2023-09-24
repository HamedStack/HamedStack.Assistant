
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides utility methods for performing operations on paths.
/// </summary>
public static class PathUtility
{
    /// <summary>
    /// Evaluates the given path and creates the necessary directories if they don't exist.
    /// </summary>
    /// <param name="path">The path to evaluate.</param>
    /// <param name="isFilePath">Indicates whether the given path is a file path.</param>
    /// <returns>True if the path evaluation and directory creation is successful, false otherwise.</returns>
    public static bool EvaluatePath(string path, bool isFilePath = true)
    {
        try
        {
            var folderPath = isFilePath ? Path.GetDirectoryName(path) : path;
            if (!Directory.Exists(folderPath) && folderPath != null)
            {
                Directory.CreateDirectory(folderPath);
            }
            return true;
        }
        catch
        {
            // ignored
        }
        return false;
    }

    /// <summary>
    /// Retrieves the directory name from the given path.
    /// </summary>
    /// <param name="path">The path from which to get the directory name.</param>
    /// <returns>The directory name or null if the path does not have one.</returns>
    public static string? GetDirectoryName(string path)
    {
        if (IsDirectory(path).HasValue)
            return Path.GetFullPath(path).Split(Path.DirectorySeparatorChar).LastOrDefault();
        var newPath = Path.GetDirectoryName(path);
        return Path.GetFullPath(newPath ?? string.Empty).Split(Path.DirectorySeparatorChar).LastOrDefault();
    }

    /// <summary>
    /// Retrieves the file path without its extension.
    /// </summary>
    /// <param name="path">The path from which to remove the extension.</param>
    /// <returns>The path without its extension.</returns>
    public static string GetFilePathWithoutExtension(string path)
    {
        return Path.ChangeExtension(path, null);
    }

    /// <summary>
    /// Retrieves the full path of the file without its extension.
    /// </summary>
    /// <param name="path">The full file path.</param>
    /// <returns>The full path of the file without its extension.</returns>
    public static string GetFullPathWithoutExtension(string path)
    {
        return Path.Combine(Path.GetDirectoryName(path) ?? string.Empty, Path.GetFileNameWithoutExtension(path));
    }

    /// <summary>
    /// Gets the parent directory of the specified path.
    /// </summary>
    /// <param name="path">The path from which to get the parent directory.</param>
    /// <returns>The parent directory or null if the path does not have one.</returns>
    public static string? GetParentDirectory(string path)
    {
        return Path.GetDirectoryName(path.Trim(Path.DirectorySeparatorChar));
    }

    /// <summary>
    /// Determines whether the given path points to a directory.
    /// </summary>
    /// <param name="path">The path to evaluate.</param>
    /// <returns>True if the path points to a directory, false if it points to a file, and null if the path does not exist.</returns>
    public static bool? IsDirectory(string path)
    {
        if (Directory.Exists(path))
            return true;
        if (File.Exists(path))
            return false;
        return null;
    }

    /// <summary>
    /// Determines whether the given path points to a file.
    /// </summary>
    /// <param name="path">The path to evaluate.</param>
    /// <returns>True if the path points to a file, false if it points to a directory, and null if the path does not exist.</returns>
    public static bool? IsFile(string path)
    {
        var isDir = IsDirectory(path);
        // ReSharper disable once MergeConditionalExpression
        return isDir == null ? null : !isDir;
    }

    /// <summary>
    /// Validates the given path.
    /// </summary>
    /// <param name="path">The path to validate.</param>
    /// <param name="allowRelativePaths">Indicates whether to allow relative paths.</param>
    /// <returns>True if the path is valid, false otherwise.</returns>
    public static bool IsValidPath(this string path, bool allowRelativePaths = false)
    {
        bool isValid;
        try
        {
            _ = Path.GetFullPath(path);
            if (allowRelativePaths)
            {
                isValid = Path.IsPathRooted(path);
            }
            else
            {
                var root = Path.GetPathRoot(path);
                isValid = string.IsNullOrEmpty(root?.Trim('\\', '/')) == false;
            }
        }
        catch
        {
            isValid = false;
        }
        return isValid;
    }
}