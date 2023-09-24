
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides utility methods related to .NET operations and configurations.
/// </summary>
public static class DotNetUtility
{
    /// <summary>
    /// Determines whether the current build configuration is in debug mode.
    /// </summary>
    /// <returns><c>true</c> if the current build is in debug mode; otherwise, <c>false</c>.</returns>
    public static bool IsDebugMode()
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    }
}