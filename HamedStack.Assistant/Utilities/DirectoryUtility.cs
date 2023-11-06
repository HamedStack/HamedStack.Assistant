// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

// ReSharper disable MemberCanBePrivate.Global
namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides utility methods for performing operations on directories.
/// </summary>
public static class DirectoryUtility
{
    /// <summary>
    /// Copies the content of a source directory to a target directory.
    /// </summary>
    /// <param name="sourceDir">The source directory.</param>
    /// <param name="targetDir">The target directory.</param>
    /// <param name="overwrite">Determines whether to overwrite existing files in the target directory.</param>
    public static void Copy(string sourceDir, string targetDir, bool overwrite)
    {
        Directory.CreateDirectory(targetDir);

        foreach (var file in Directory.GetFiles(sourceDir))
            File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), overwrite);

        foreach (var directory in Directory.GetDirectories(sourceDir))
            Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)), overwrite);
    }

    /// <summary>
    /// Creates a directory and all subdirectories as specified by the path.
    /// </summary>
    /// <param name="path">The directory path to create.</param>
    public static void CreateDirectoryRecursively(string path)
    {
        if (Directory.Exists(path))
        {
            return;
        }
        var parentDirPath = Path.GetDirectoryName(path);
        if (parentDirPath != null)
        {
            CreateDirectoryRecursively(parentDirPath);
        }
        Directory.CreateDirectory(path);
    }

    /// <summary>
    /// Creates a new temporary directory and returns its path.
    /// </summary>
    /// <returns>The path of the created temporary directory.</returns>
    public static string CreateTempDirectory()
    {
        var tempDirectory = GetTempDirectory();
        if (!Directory.Exists(tempDirectory)) Directory.CreateDirectory(tempDirectory);

        return tempDirectory;
    }

    /// <summary>
    /// Creates a temporary directory, executes the provided action with the directory path, and
    /// optionally deletes it afterwards.
    /// </summary>
    /// <param name="action">The action to be executed.</param>
    /// <param name="autoDelete">
    /// Determines whether to delete the temporary directory after executing the action.
    /// </param>
    public static void CreateTempDirectory(Action<string> action, bool autoDelete = true)
    {
        var tempDirectory = GetTempDirectory();
        if (!Directory.Exists(tempDirectory)) Directory.CreateDirectory(tempDirectory);
        action(tempDirectory);
        if (autoDelete) Directory.Delete(tempDirectory, true);
    }

    /// <summary>
    /// Deletes a directory and its content even if it contains read-only files or subdirectories.
    /// </summary>
    /// <param name="directoryPath">The directory path to delete.</param>
    public static void DeleteReadOnlyDirectory(string directoryPath)
    {
        foreach (var subDirectory in Directory.EnumerateDirectories(directoryPath))
            DeleteReadOnlyDirectory(subDirectory);
        foreach (var fileName in Directory.EnumerateFiles(directoryPath))
        {
            var fileInfo = new FileInfo(fileName)
            {
                Attributes = FileAttributes.Normal
            };
            fileInfo.Delete();
        }

        Directory.Delete(directoryPath);
    }

    /// <summary>
    /// Deletes all the content of a directory without deleting the directory itself.
    /// </summary>
    /// <param name="directory">The directory to empty.</param>
    public static void Empty(this DirectoryInfo directory)
    {
        try
        {
            foreach (var file in directory.GetFiles()) file.Delete();
            foreach (var subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    /// Gets the directory path of a specified file.
    /// </summary>
    /// <param name="filePath">The path of the file.</param>
    /// <returns>The directory containing the file.</returns>
    public static string? GetDirectoryPath(string filePath)
    {
        return Path.GetDirectoryName(filePath);
    }

    /// <summary>
    /// Gets the parent directory of a specified directory by going up a specified number of levels.
    /// </summary>
    /// <param name="folderPath">The directory path.</param>
    /// <param name="levels">The number of directory levels to go up.</param>
    /// <returns>The parent directory path.</returns>
    public static string? GetParentDirectoryPath(string folderPath, int levels)
    {
        var result = folderPath;
        for (var i = 0; i < levels; i++)
            if (result != null && Directory.GetParent(result) != null)
                result = Directory.GetParent(result)?.FullName;
            else
                return result;
        return result;
    }

    /// <summary>
    /// Gets the immediate parent directory of a specified directory.
    /// </summary>
    /// <param name="folderPath">The directory path.</param>
    /// <returns>The parent directory path.</returns>
    public static string? GetParentDirectoryPath(string folderPath)
    {
        return GetParentDirectoryPath(folderPath, 1);
    }

    /// <summary>
    /// Retrieves an enumerable collection of parent directory paths for the specified path recursively.
    /// </summary>
    /// <param name="path">The path for which to retrieve parent directory paths.</param>
    /// <returns>An enumerable collection of parent directory paths.</returns>
    public static IEnumerable<string> GetParentsRecursively(string path)
    {
        var parentDirPath = Path.GetDirectoryName(path);
        while (!string.IsNullOrEmpty(parentDirPath))
        {
            yield return parentDirPath;
            parentDirPath = Path.GetDirectoryName(parentDirPath);
        }
    }

    /// <summary>
    /// Creates a path for a new temporary directory without actually creating it.
    /// </summary>
    /// <returns>The path for the new temporary directory.</returns>
    public static string GetTempDirectory()
    {
        return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    }

    /// <summary>
    /// Renames a specified directory.
    /// </summary>
    /// <param name="directoryPath">The path of the directory to rename.</param>
    /// <param name="newName">The new name for the directory.</param>
    /// <returns>A value indicating whether the directory was successfully renamed.</returns>
    public static bool RenameDirectory(string directoryPath, string newName)
    {
        var path = Path.GetDirectoryName(directoryPath);
        if (path == null)
            return false;
        try
        {
            Directory.Move(directoryPath, Path.Combine(path, newName));
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Deletes a directory safely by setting normal attributes to all its content first.
    /// </summary>
    /// <param name="path">The directory path to delete.</param>
    /// <param name="recursive">Determines whether to delete the directory's content recursively.</param>
    public static void SafeDeleteDirectory(string path, bool recursive = false)
    {
        if (!Directory.Exists(path)) return;

        var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var files = Directory
              .EnumerateFileSystemEntries(path, "*", searchOption);
        foreach (var file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
        }
        Directory.Delete(path, recursive);
    }
}