// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

using System.Text;

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Generates unique, ordered ID numbers with the Snowflake algorithm. 
/// Each generated ID is based on the current time, worker ID, datacenter ID, and sequence number.
/// Note: It is recommended to keep the generator as a singleton to avoid the possibility of duplicate IDs. 
/// Multiple instances of the generator can lead to duplicate IDs.
/// </summary>
public class SnowflakeIdGenerator
{
    private readonly long _epochMilliseconds;

    private const int WORKER_ID_BITS = 5;
    private const int DATACENTER_ID_BITS = 5;

    private readonly long _maxSequence;
    private readonly int _sequenceShift;

    private const long MAX_WORKER_ID = -1L ^ (-1L << WORKER_ID_BITS);
    private const long MAX_DATACENTER_ID = -1L ^ (-1L << DATACENTER_ID_BITS);

    private const int WORKER_ID_SHIFT = 13;
    private const int DATACENTER_ID_SHIFT = 18;
    private const int TIMESTAMP_LEFT_SHIFT = 23;

    private readonly long _workerId;
    private readonly long _datacenterId;
    private long _sequence;
    private long _lastTimestamp = -1L;

    private const string BASE62_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    private static readonly object Lock = new();

    /// <summary>
    /// Creates a new instance of the Snowflake ID generator.
    /// </summary>
    /// <param name="workerId">The worker ID. Must be between 0 and 31, inclusive.</param>
    /// <param name="datacenterId">The data center ID. Must be between 0 and 31, inclusive.</param>
    /// <param name="projectStartDate">The reference date for timestamp calculation. If not specified, the date will be set to January 1, 1970 (Unix epoch time).</param>
    /// <param name="sequenceBits">The number of bits used for the sequence number. Must be between 1 and 22, inclusive.</param>
    /// <exception cref="ArgumentException">Thrown when provided values are out of expected range.</exception>
    public SnowflakeIdGenerator(long workerId = 0L, long datacenterId = 0L, DateTime? projectStartDate = default, int sequenceBits = 1)
    {
        projectStartDate ??= new DateTime(1970, 1, 1);

        _epochMilliseconds = (long)(projectStartDate.Value.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;

        if (workerId is > MAX_WORKER_ID or < 0)
        {
            throw new ArgumentException($"Worker ID must be between 0 and {MAX_WORKER_ID}.");
        }
        if (datacenterId is > MAX_DATACENTER_ID or < 0)
        {
            throw new ArgumentException($"Data center ID must be between 0 and {MAX_DATACENTER_ID}.");
        }
        if (sequenceBits is <= 0 or > 22)
        {
            throw new ArgumentException("Sequence bits must be between 1 and 22 (inclusive).");
        }

        _workerId = workerId;
        _datacenterId = datacenterId;
        _maxSequence = (1L << sequenceBits) - 1;
        _sequenceShift = 22 - sequenceBits;
    }

    /// <summary>
    /// Generates a new Snowflake ID.
    /// </summary>
    /// <returns>The generated Snowflake ID as a long.</returns>
    /// <exception cref="Exception">Thrown when the system clock goes backward.</exception>
    public long NextId()
    {
        lock (Lock)
        {
            var timestamp = TimeGen();

            if (timestamp < _lastTimestamp)
            {
                throw new Exception($"Invalid system clock: The last timestamp was {_lastTimestamp}, but the current timestamp is {timestamp}.");
            }

            if (_lastTimestamp == timestamp)
            {
                _sequence = (_sequence + 1) & _maxSequence;
                if (_sequence == 0)
                {
                    timestamp = TilNextMillis(_lastTimestamp);
                }
            }
            else
            {
                _sequence = 0L;
            }

            _lastTimestamp = timestamp;

            return ((timestamp - _epochMilliseconds) << TIMESTAMP_LEFT_SHIFT) |
                   (_datacenterId << DATACENTER_ID_SHIFT) |
                   (_workerId << WORKER_ID_SHIFT) |
                   (_sequence << _sequenceShift);
        }
    }

    /// <summary>
    /// Generates a batch of Snowflake IDs.
    /// </summary>
    /// <param name="batchSize">The number of IDs to generate. Must be a positive number.</param>
    /// <returns>A collection of the generated Snowflake IDs as longs.</returns>
    /// <exception cref="ArgumentException">Thrown when the batch size is not a positive number.</exception>
    public ICollection<long> NextIds(int batchSize)
    {
        if (batchSize <= 0)
        {
            throw new ArgumentException("Batch size must be a positive number.");
        }

        var ids = new List<long>(batchSize);
        for (var i = 0; i < batchSize; i++)
        {
            ids.Add(NextId());
        }

        return ids;
    }

    /// <summary>
    /// Generates a new Snowflake ID and returns it as a base62 string.
    /// </summary>
    /// <returns>The generated Snowflake ID as a string.</returns>
    public string NextIdAsString()
    {
        var id = NextId();
        return ToBase62(id);
    }

    /// <summary>
    /// Generates a batch of Snowflake IDs and returns them as base62 strings.
    /// </summary>
    /// <param name="batchSize">The number of IDs to generate. Must be a positive number.</param>
    /// <returns>A collection of the generated Snowflake IDs as strings.</returns>
    /// <exception cref="ArgumentException">Thrown when the batch size is not a positive number.</exception>
    public ICollection<string> NextIdsAsString(int batchSize)
    {
        if (batchSize <= 0)
        {
            throw new ArgumentException("Batch size must be a positive number.");
        }

        var ids = new List<string>(batchSize);
        for (var i = 0; i < batchSize; i++)
        {
            ids.Add(NextIdAsString());
        }

        return ids;
    }

    /// <summary>
    /// Generates a new Snowflake ID and returns it as a base64 string.
    /// </summary>
    /// <param name="removePadding">Specifies whether to remove padding (default is false).</param>
    /// <returns>The generated Snowflake ID as a string.</returns>
    public string NextIdAsBase64(bool removePadding = false)
    {
        var id = NextId();
        return ToBase64(id, removePadding);
    }

    /// <summary>
    /// Generates a batch of Snowflake IDs and returns them as base64 strings.
    /// </summary>
    /// <param name="batchSize">The number of IDs to generate. Must be a positive number.</param>
    /// <param name="removePadding">Specifies whether to remove padding (default is false).</param>
    /// <returns>A collection of the generated Snowflake IDs as strings.</returns>
    /// <exception cref="ArgumentException">Thrown when the batch size is not a positive number.</exception>
    public ICollection<string> NextIdsAsBase64(int batchSize, bool removePadding = false)
    {
        if (batchSize <= 0)
        {
            throw new ArgumentException("Batch size must be a positive number.");
        }

        var ids = new List<string>(batchSize);
        for (var i = 0; i < batchSize; i++)
        {
            ids.Add(NextIdAsBase64(removePadding));
        }

        return ids;
    }

    /// <summary>
    /// Converts a given number to its base64 string representation.
    /// </summary>
    /// <param name="number">The number to convert to base64.</param>
    /// <param name="removePadding">If set to <c>true</c>, removes the padding from the base64 string.</param>
    /// <returns>The base64 string representation of the given number.</returns>
    private static string ToBase64(long number, bool removePadding)
    {
        var byteArray = BitConverter.GetBytes(number);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(byteArray);
        var base64 = Convert.ToBase64String(byteArray);
        return removePadding ? base64.TrimEnd('=') : base64;
    }

    /// <summary>
    /// Converts a given number to its base62 string representation.
    /// </summary>
    /// <remarks>
    /// This method uses the BASE62_CHARS constant to map numbers to characters.
    /// </remarks>
    /// <param name="number">The number to convert to base62.</param>
    /// <returns>The base62 string representation of the given number.</returns>
    private static string ToBase62(long number)
    {
        var sb = new StringBuilder();

        do
        {
            sb.Insert(0, BASE62_CHARS[(int)(number % 62)]);
            number /= 62;
        } while (number > 0);

        return sb.ToString();
    }

    /// <summary>
    /// Generates a timestamp greater than the last provided timestamp.
    /// </summary>
    /// <param name="lastTimestamp">The last known timestamp.</param>
    /// <returns>A timestamp that is greater than the provided <paramref name="lastTimestamp"/>.</returns>
    private static long TilNextMillis(long lastTimestamp)
    {
        var timestamp = TimeGen();
        while (timestamp <= lastTimestamp)
        {
            timestamp = TimeGen();
        }
        return timestamp;
    }

    /// <summary>
    /// Generates a current timestamp in milliseconds since Unix epoch time.
    /// </summary>
    /// <returns>The current timestamp in milliseconds.</returns>
    private static long TimeGen()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}