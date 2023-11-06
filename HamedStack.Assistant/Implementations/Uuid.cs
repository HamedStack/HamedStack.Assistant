// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace HamedStack.Assistant.Implementations;

/// <summary>
/// Represents a universally unique identifier (UUID) with conversions to and from the built-in <see
/// cref="Guid"/> type.
/// </summary>
/// <remarks>
/// This UUID struct provides a custom representation of the universal unique identifier, allowing
/// additional customization or performance improvements over the built-in Guid.
/// </remarks>
public readonly struct Uuid : IEquatable<Uuid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Uuid"/> struct using the specified most and
    /// least significant bits.
    /// </summary>
    /// <param name="mostSignificantBits">The most significant bits of the UUID.</param>
    /// <param name="leastSignificantBits">The least significant bits of the UUID.</param>
    public Uuid(long mostSignificantBits, long leastSignificantBits)
    {
        MostSignificantBits = mostSignificantBits;
        LeastSignificantBits = leastSignificantBits;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Uuid"/> struct using the given byte array.
    /// </summary>
    /// <param name="b">The byte array representing the UUID. It must have a length of 16.</param>
    /// <exception cref="ArgumentNullException">Thrown when the provided byte array is null.</exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the length of the byte array is not 16.
    /// </exception>
    public Uuid(byte[] b)
    {
        if (b == null)
            throw new ArgumentNullException(nameof(b));
        if (b.Length != 16)
            throw new ArgumentException("Length of the UUID byte array should be 16");
        MostSignificantBits = BitConverter.ToInt64(b, 0);
        LeastSignificantBits = BitConverter.ToInt64(b, 8);
    }

    /// <summary>
    /// Gets the least significant bits of the UUID.
    /// </summary>
    public long LeastSignificantBits { get; }

    /// <summary>
    /// Gets the most significant bits of the UUID.
    /// </summary>
    public long MostSignificantBits { get; }

    /// <summary>
    /// Performs an explicit conversion from <see cref="Uuid"/> to <see cref="Guid"/>.
    /// </summary>
    /// <param name="uuid">The UUID to convert.</param>
    /// <returns>The converted GUID.</returns>
    public static explicit operator Guid(Uuid uuid)
    {
        if (uuid == default) return default;
        var uuidMostSignificantBytes = BitConverter.GetBytes(uuid.MostSignificantBits);
        var uuidLeastSignificantBytes = BitConverter.GetBytes(uuid.LeastSignificantBits);
        byte[] guidBytes =
        {
            uuidMostSignificantBytes[4],
            uuidMostSignificantBytes[5],
            uuidMostSignificantBytes[6],
            uuidMostSignificantBytes[7],
            uuidMostSignificantBytes[2],
            uuidMostSignificantBytes[3],
            uuidMostSignificantBytes[0],
            uuidMostSignificantBytes[1],
            uuidLeastSignificantBytes[7],
            uuidLeastSignificantBytes[6],
            uuidLeastSignificantBytes[5],
            uuidLeastSignificantBytes[4],
            uuidLeastSignificantBytes[3],
            uuidLeastSignificantBytes[2],
            uuidLeastSignificantBytes[1],
            uuidLeastSignificantBytes[0]
        };
        return new Guid(guidBytes);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="Guid"/> to <see cref="Uuid"/>.
    /// </summary>
    /// <param name="value">The GUID to convert.</param>
    /// <returns>The converted UUID.</returns>
    public static implicit operator Uuid(Guid value)
    {
        if (value == default) return default;
        var guidBytes = value.ToByteArray();
        byte[] uuidBytes =
        {
            guidBytes[6],
            guidBytes[7],
            guidBytes[4],
            guidBytes[5],
            guidBytes[0],
            guidBytes[1],
            guidBytes[2],
            guidBytes[3],
            guidBytes[15],
            guidBytes[14],
            guidBytes[13],
            guidBytes[12],
            guidBytes[11],
            guidBytes[10],
            guidBytes[9],
            guidBytes[8]
        };
        return new Uuid(BitConverter.ToInt64(uuidBytes, 0), BitConverter.ToInt64(uuidBytes, 8));
    }

    /// <summary>
    /// Creates a new UUID with a unique value.
    /// </summary>
    /// <returns>A newly created UUID.</returns>
    public static Uuid NewUuid()
    {
        return Guid.NewGuid();
    }

    /// <summary>
    /// Determines whether two specified UUIDs have different values.
    /// </summary>
    /// <param name="a">The first UUID to compare.</param>
    /// <param name="b">The second UUID to compare.</param>
    /// <returns>true if the two UUIDs are different; otherwise, false.</returns>
    public static bool operator !=(Uuid a, Uuid b)
    {
        return !a.Equals(b);
    }

    /// <summary>
    /// Determines whether two specified UUIDs have the same value.
    /// </summary>
    /// <param name="a">The first UUID to compare.</param>
    /// <param name="b">The second UUID to compare.</param>
    /// <returns>true if the two UUIDs are equal; otherwise, false.</returns>
    public static bool operator ==(Uuid a, Uuid b)
    {
        return a.Equals(b);
    }

    /// <summary>
    /// Parses a string representation of a UUID.
    /// </summary>
    /// <param name="input">The string representation of the UUID.</param>
    /// <returns>The parsed UUID.</returns>
    public static Uuid Parse(string input)
    {
        return Guid.Parse(input);
    }

    /// <summary>
    /// Converts a string representation of a UUID to a Guid.
    /// </summary>
    /// <param name="input">The string representation of the UUID.</param>
    /// <returns>The converted Guid.</returns>
    public static Guid ToGuid(string input)
    {
        return Guid.Parse(input);
    }

    /// <summary>
    /// Converts a UUID to a Guid.
    /// </summary>
    /// <param name="input">The UUID to convert.</param>
    /// <returns>The converted Guid.</returns>
    public static Guid ToGuid(Uuid input)
    {
        return (Guid)input;
    }

    /// <summary>
    /// Tries to parse a string representation of a UUID.
    /// </summary>
    /// <param name="input">The string representation of the UUID.</param>
    /// <param name="uuid">The parsed UUID if successful.</param>
    /// <returns>true if the parsing was successful; otherwise, false.</returns>
    public static bool TryParse(string input, out Uuid uuid)
    {
        try
        {
            uuid = new Guid(input.Replace("-", ""));
            return true;
        }
        catch
        {
            uuid = Guid.Empty;
            return false;
        }
    }

    /// <summary>
    /// Determines whether this instance and a specified object have the same value.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>true if the specified object is equal to this instance; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        return obj is Uuid uuid && Equals(uuid);
    }

    /// <summary>
    /// Determines whether this instance and another specified UUID have the same value.
    /// </summary>
    /// <param name="uuid">The UUID to compare with the current instance.</param>
    /// <returns>true if the specified UUID is equal to this instance; otherwise, false.</returns>
    public bool Equals(Uuid uuid)
    {
        return MostSignificantBits == uuid.MostSignificantBits && LeastSignificantBits == uuid.LeastSignificantBits;
    }

    /// <summary>
    /// Returns the hash code for this UUID.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        return ((Guid)this).GetHashCode();
    }

    /// <summary>
    /// Converts the value of the current UUID to its equivalent byte array representation.
    /// </summary>
    /// <returns>A 16-element byte array that contains the value of the current UUID.</returns>
    public byte[] ToByteArray()
    {
        var uuidMostSignificantBytes = BitConverter.GetBytes(MostSignificantBits);
        var uuidLeastSignificantBytes = BitConverter.GetBytes(LeastSignificantBits);
        return new[]
        {
            uuidMostSignificantBytes[0],
            uuidMostSignificantBytes[1],
            uuidMostSignificantBytes[2],
            uuidMostSignificantBytes[3],
            uuidMostSignificantBytes[4],
            uuidMostSignificantBytes[5],
            uuidMostSignificantBytes[6],
            uuidMostSignificantBytes[7],
            uuidLeastSignificantBytes[0],
            uuidLeastSignificantBytes[1],
            uuidLeastSignificantBytes[2],
            uuidLeastSignificantBytes[3],
            uuidLeastSignificantBytes[4],
            uuidLeastSignificantBytes[5],
            uuidLeastSignificantBytes[6],
            uuidLeastSignificantBytes[7]
        };
    }

    /// <summary>
    /// Converts the value of the current UUID to its equivalent string representation.
    /// </summary>
    /// <returns>The string representation of the current UUID.</returns>
    public override string ToString()
    {
        return GetDigits(MostSignificantBits >> 32, 8) + "-" +
               GetDigits(MostSignificantBits >> 16, 4) + "-" +
               GetDigits(MostSignificantBits, 4) + "-" +
               GetDigits(LeastSignificantBits >> 48, 4) + "-" +
               GetDigits(LeastSignificantBits, 12);
    }

    /// <summary>
    /// Retrieves a subset of the digits from the provided value.
    /// </summary>
    /// <param name="val">The value to extract digits from.</param>
    /// <param name="digits">The number of digits to extract.</param>
    /// <returns>A string representation of the extracted digits.</returns>
    private static string GetDigits(long val, int digits)
    {
        var hi = 1L << (digits * 4);
        return $"{hi | (val & (hi - 1)):X}"[1..];
    }
}