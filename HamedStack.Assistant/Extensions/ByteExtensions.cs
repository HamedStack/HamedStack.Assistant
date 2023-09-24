// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
namespace HamedStack.Assistant.Extensions.ByteExtended;

public static class ByteExtensions
{
    public static string ToHexString(this byte @byte)
    {
        return @byte.ToString("x2");
    }
}