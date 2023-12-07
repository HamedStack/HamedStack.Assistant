// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

using System.Text.Json;

namespace HamedStack.Assistant.Extensions.RandomExtended;

public static class RandomExtensions
{
    public static bool CoinToss(this Random @this)
    {
        return @this.Next(2) == 0;
    }

    public static bool NextBoolean(this Random random)
    {
        return random.Next(0, 2) == 0;
    }
    public static DateTime NextDateTime(this Random random, DateTime minValue, DateTime maxValue)
    {
        if (minValue > maxValue)
        {
            throw new ArgumentException("minValue should be less than or equal to maxValue");
        }
        long range = maxValue.Ticks - minValue.Ticks;
        long randomTicks = (long)(random.NextDouble() * range);

        return new DateTime(minValue.Ticks + randomTicks);
    }

    public static DateTime NextDateTime(this Random @this)
    {
        long ticks = (long)(@this.NextDouble() * DateTime.MaxValue.Ticks);
        return new DateTime(ticks);
    }

    public static decimal NextDecimal(this Random @this)
    {
        var sign = @this.Next(2) == 1;
        return @this.NextDecimal(sign);
    }

    public static decimal NextDecimal(this Random @this, bool sign)
    {
        var scale = (byte)@this.Next(29);
        return new decimal(@this.NextInt32(),
            @this.NextInt32(),
            @this.NextInt32(),
            sign,
            scale);
    }

    public static decimal NextDecimal(this Random @this, decimal maxValue)
    {
        return @this.NextNonNegativeDecimal() / decimal.MaxValue * maxValue;
    }

    public static decimal NextDecimal(this Random @this, decimal minValue, decimal maxValue)
    {
        if (minValue >= maxValue) throw new InvalidOperationException();
        var range = maxValue - minValue;
        return @this.NextDecimal(range) + minValue;
    }

    public static double NextDouble(this Random random, double minValue, double maxValue, int decimalPlaces)
    {
        double randomValue = random.NextDouble() * (maxValue - minValue) + minValue;
        return Math.Round(randomValue, decimalPlaces);
    }

    public static double NextDouble(this Random @this, double min, double max)
    {
        return @this.NextDouble() * (max - min) + min;
    }

    public static T? NextEnum<T>(this Random random) where T : Enum
    {
        var type = typeof(T);
        if (!type.IsEnum) throw new InvalidOperationException();

        var array = Enum.GetValues(type);
        var index = random.Next(array.GetLowerBound(0), array.GetUpperBound(0) + 1);
        return (T?)array.GetValue(index);
    }

    public static int NextInt32(this Random @this)
    {
        var firstBits = @this.Next(0, 1 << 4) << 28;
        var lastBits = @this.Next(0, 1 << 28);
        return firstBits | lastBits;
    }

    public static long NextInt64(this Random @this, long maxValue)
    {
        return (long)(@this.NextNonNegativeLong() / (double)long.MaxValue * maxValue);
    }

    public static long NextInt64(this Random @this, long minValue, long maxValue)
    {
        if (minValue >= maxValue) throw new InvalidOperationException();
        var range = maxValue - minValue;
        return @this.NextInt64(range) + minValue;
    }

    public static long NextInt64(this Random @this)
    {
        var buffer = new byte[sizeof(long)];
        @this.NextBytes(buffer);
        return BitConverter.ToInt64(buffer, 0);
    }

    public static string NextJsonString(this Random @this, int maxDepth = 10)
    {
        var md = @this.Next(1, maxDepth);
        var obj = GenerateRandomJsonObject(0, md);
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            WriteIndented = true,
        });
    }

    public static decimal NextNonNegativeDecimal(this Random @this)
    {
        return @this.NextDecimal(false);
    }

    public static long NextNonNegativeLong(this Random @this)
    {
        var bytes = new byte[sizeof(long)];
        @this.NextBytes(bytes);
        // strip out the sign bit
        bytes[7] = (byte)(bytes[7] & 0x7f);
        return BitConverter.ToInt64(bytes, 0);
    }

    public static string NextString(this Random random, int minLength, int maxLength, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")
    {
        if (minLength < 0) minLength = 0;
        if (maxLength < 0) maxLength = 0;
        if (minLength > chars.Length) minLength = chars.Length;
        if (maxLength > chars.Length) maxLength = chars.Length;
        if (minLength > maxLength)
            throw new ArgumentException("minLength should be less than or equal to maxLength");

        var length = random.Next(minLength, maxLength);
        var stringChars = new char[length];
        for (var i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }
        return new string(stringChars);
    }
    public static T PickOneOf<T>(this Random @this, params T[] values)
    {
        return values[@this.Next(values.Length)];
    }

    public static T PickOneOf<T>(this Random @this, IEnumerable<T> values)
    {
        var arr = values.ToArray();
        return @this.PickOneOf(arr);
    }
    private static object GenerateRandomJsonArray(int depth, int maxDepth)
    {
        var random = new Random();
        var arrayLength = random.Next(1, 10);
        var array = new object?[arrayLength];
        for (var i = 0; i < arrayLength; i++)
        {
            array[i] = GenerateRandomJsonValue(depth + 1, maxDepth);
        }
        return array;
    }

    private static object GenerateRandomJsonObject(int depth, int maxDepth)
    {
        var obj = new Dictionary<string, object?>();
        var random = new Random();
        var elements = random.Next(1, 10);
        for (int i = 0; i < elements; i++)
        {
            obj["key" + i] = GenerateRandomJsonValue(depth + 1, maxDepth);
        }
        return obj;
    }

    private static object? GenerateRandomJsonValue(int depth, int maxDepth)
    {
        if (maxDepth < 1 || maxDepth > 50)
        {
            throw new ArgumentOutOfRangeException(nameof(maxDepth), "maxDepth must be between 1 and 50.");
        }
        var random = new Random();
        if (depth > maxDepth)
        {
            switch (random.Next(0, 5))
            {
                case 0: return random.Next(-100, 100);
                case 1: return random.NextDouble(-100, 100, 3);
                case 2: return random.NextString(0, 10);
                case 3: return random.NextBoolean();
                case 4: return random.NextDateTime().ToString("O");
                default: return null;
            }
        }
        else
        {
            switch (random.Next(0, 7))
            {
                case 0: return random.Next(-1000, 1000);
                case 1: return random.NextDouble(-1000, 1000, 3);
                case 2: return random.NextString(0, 15);
                case 3: return random.NextBoolean();
                case 4: return GenerateRandomJsonObject(depth + 1, maxDepth);
                case 5: return GenerateRandomJsonArray(depth + 1, maxDepth);
                case 6: return random.NextDateTime().ToString("O");
                default: return null;
            }
        }
    }
}