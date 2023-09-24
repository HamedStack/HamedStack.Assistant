// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
// ReSharper disable CheckNamespace

namespace System;

/// <summary>
/// Provides extension methods for handling and converting Persian date and time.
/// </summary>
public static class PersianDateTimeExtensions
{
    private const string DaysAgo = "{0} روز قبل";
    private const string HoursAgo = "{0} ساعت قبل";
    private const string JustNow = "همین الان";
    private const string LastMonth = "ماه قبل";
    private const string LastWeek = "هفته قبل";
    private const string LastYear = "سال قبل";
    private const string MinutesAgo = "{0} دقیقه قبل";
    private const string MonthsAgo = "{0} ماه قبل";
    private const string NotYet = "هنوز نه";
    private const string ThreeWeeksAgo = "سه هفته قبل";
    private const string TwoWeeksAgo = "دو هفته قبل";
    private const string YearsAgo = "{0} سال قبل";
    private const string Yesterday = "دیروز";

    /// <summary>
    /// Converts the specified nullable DateTime value to a PersianDateTime value.
    /// </summary>
    /// <param name="dt">The DateTime value to convert.</param>
    /// <returns>A nullable PersianDateTime corresponding to the provided DateTime; null if the input is null.</returns>
    public static PersianDateTime? ToPersianDateTime(this DateTime? dt)
    {
        return !dt.HasValue ? null : new PersianDateTime(dt.Value);
    }

    /// <summary>
    /// Converts the specified DateTime value to a PersianDateTime value.
    /// </summary>
    /// <param name="dt">The DateTime value to convert.</param>
    /// <returns>A PersianDateTime corresponding to the provided DateTime.</returns>
    public static PersianDateTime ToPersianDateTime(this DateTime dt)
    {
        return new PersianDateTime(dt);
    }

    /// <summary>
    /// Converts the specified PersianDateTime value to its relative string representation.
    /// </summary>
    /// <param name="date">The PersianDateTime value to convert.</param>
    /// <returns>A string that represents the relative time difference between now and the specified date in Persian.</returns>
    public static string ToPersianRelativeDate(this PersianDateTime date)
    {
        var gDate = date.ToDateTime();
        return gDate.ToPersianRelativeDate();
    }

    /// <summary>
    /// Converts the specified DateTime value to its relative string representation in Persian.
    /// </summary>
    /// <param name="date">The DateTime value to convert.</param>
    /// <returns>A string that represents the relative time difference between now and the specified date in Persian.</returns>
    public static string ToPersianRelativeDate(this DateTime date)
    {
        var timeSince = DateTime.Now.Subtract(date);
        if (timeSince.TotalMilliseconds < 1) return NotYet;
        if (timeSince.TotalMinutes < 1) return JustNow;
        if (timeSince.TotalMinutes < 60) return string.Format(MinutesAgo, timeSince.Minutes);
        if (timeSince.TotalHours < 24) return string.Format(HoursAgo, timeSince.Hours);
        if (timeSince.TotalDays < 2) return Yesterday;
        if (timeSince.TotalDays < 7) return string.Format(DaysAgo, timeSince.Days);
        if (timeSince.TotalDays < 14) return LastWeek;
        if (timeSince.TotalDays < 21) return TwoWeeksAgo;
        if (timeSince.TotalDays < 28) return ThreeWeeksAgo;
        if (timeSince.TotalDays < 60) return LastMonth;
        if (timeSince.TotalDays < 365) return string.Format(MonthsAgo, Math.Round(timeSince.TotalDays / 30));
        return timeSince.TotalDays < 730
            ? LastYear
            : string.Format(YearsAgo, Math.Round(timeSince.TotalDays / 365));
    }

    /// <summary>
    /// Converts the specified nullable DateTime value to its string representation in Persian.
    /// </summary>
    /// <param name="dt">The nullable DateTime value to convert.</param>
    /// <returns>A string that represents the specified Persian date; null if the input is null.</returns>
    public static string? ToPersianRelativeDate(this DateTime? dt)
    {
        return !dt.HasValue ? null : new PersianDateTime(dt.Value).ToString();
    }

    /// <summary>
    /// Converts the specified DateTime value to its string representation in Persian.
    /// </summary>
    /// <param name="dt">The DateTime value to convert.</param>
    /// <returns>A string that represents the specified Persian date.</returns>
    public static string? ToStringPersianDateTime(this DateTime? dt)
    {
        return !dt.HasValue ? null : new PersianDateTime(dt.Value).ToString();
    }

    /// <summary>
    /// Converts the specified DateTime value to its string representation in Persian.
    /// </summary>
    /// <param name="dt">The DateTime value to convert.</param>
    /// <returns>A string that represents the specified Persian date.</returns>
    public static string ToStringPersianDateTime(this DateTime dt)
    {
        return new PersianDateTime(dt).ToString();
    }

    /// <summary>
    /// Converts the specified TimeSpan to a string representation in HH:mm format.
    /// </summary>
    /// <param name="time">The TimeSpan value to convert.</param>
    /// <returns>A string that represents the specified TimeSpan in HH:mm format.</returns>
    internal static string ToHHMM(this TimeSpan time)
    {
        return time.ToString("hh\\:mm");
    }

    /// <summary>
    /// Converts the specified TimeSpan to a string representation in HH:mm:ss format.
    /// </summary>
    /// <param name="time">The TimeSpan value to convert.</param>
    /// <returns>A string that represents the specified TimeSpan in HH:mm:ss format.</returns>
    internal static string ToHHMMSS(this TimeSpan time)
    {
        return time.ToString("hh\\:mm\\:ss");
    }

    /// <summary>
    /// Converts the specified TimeSpan to an integer representation in HHmmss format.
    /// </summary>
    /// <param name="time">The TimeSpan value to convert.</param>
    /// <returns>An integer that represents the specified TimeSpan in HHmmss format.</returns>
    internal static int ToInteger(this TimeSpan time)
    {
        return int.Parse(time.Hours + time.Minutes.ToString().PadLeft(2, '0') +
                         time.Seconds.ToString().PadLeft(2, '0'));
    }

    /// <summary>
    /// Converts the specified TimeSpan to a short representation in HHmm format.
    /// </summary>
    /// <param name="time">The TimeSpan value to convert.</param>
    /// <returns>A short that represents the specified TimeSpan in HHmm format.</returns>
    internal static short ToShort(this TimeSpan time)
    {
        return short.Parse(time.Hours + time.Minutes.ToString().PadLeft(2, '0'));
    }
}