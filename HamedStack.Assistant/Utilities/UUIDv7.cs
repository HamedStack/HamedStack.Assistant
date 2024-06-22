// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

using System.Security.Cryptography;

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// A class that provides functionality to generate UUID version 7.
/// UUID version 7 is a hypothetical new version of UUIDs that includes
/// a timestamp component as part of its value.
/// </summary>
public class UUIDv7
{
    private static readonly RandomNumberGenerator random = RandomNumberGenerator.Create();

    /// <summary>
    /// Generates a UUID version 7 byte array.
    /// UUID version 7 includes a timestamp and random component.
    /// </summary>
    /// <returns>A byte array representing the generated UUID version 7.</returns>
    public static byte[] Generate()
    {
        var value = new byte[16];
        random.GetBytes(value);

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        value[0] = (byte)((timestamp >> 40) & 0xFF);
        value[1] = (byte)((timestamp >> 32) & 0xFF);
        value[2] = (byte)((timestamp >> 24) & 0xFF);
        value[3] = (byte)((timestamp >> 16) & 0xFF);
        value[4] = (byte)((timestamp >> 8) & 0xFF);
        value[5] = (byte)(timestamp & 0xFF);

        value[6] = (byte)((value[6] & 0x0F) | 0x70);
        value[8] = (byte)((value[8] & 0x3F) | 0x80);

        return value;
    }

    /// <summary>
    /// Creates a UUID version 7 string.
    /// This string is a hexadecimal representation of the UUID byte array.
    /// </summary>
    /// <returns>A string representing the generated UUID version 7.</returns>
    public static string New()
    {
        var uuidBytes = Generate();
        return uuidBytes.Aggregate(string.Empty, (current, b) => current + $"{b:x2}");
    }
}