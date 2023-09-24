// ReSharper disable CheckNamespace

namespace System;

/// <summary>
/// Specifies the persian date and time mode to determining the PersianDateTime.Now.
/// </summary>
public enum PersianDateTimeMode
{
    /// <summary>
    /// Using the current time zone.
    /// </summary>
    System,

    /// <summary>
    /// Using the persian time zone.
    /// </summary>
    PersianTimeZoneInfo,

    /// <summary>
    /// Using the UTC date and time with custom daylight saving time.
    /// </summary>
    UtcOffset
}