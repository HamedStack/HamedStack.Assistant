// ReSharper disable CheckNamespace

namespace System;

/// <summary>
/// Specifies the date and time format
/// </summary>
public enum PersianDateTimeFormat
{
    /// <summary>
    /// Date format.
    /// </summary>
    Date = 0,
        
    /// <summary>
    /// Date and time format.
    /// </summary>
    DateTime = 1,
        
    /// <summary>
    /// Long date format.
    /// </summary>
    LongDate = 2,
        
    /// <summary>
    /// Long date and time format.
    /// </summary>
    LongDateLongTime = 3,
        
    /// <summary>
    /// Full date format.
    /// </summary>
    FullDate = 4,
        
    /// <summary>
    /// Full date and long time format.
    /// </summary>
    FullDateLongTime = 5,
        
    /// <summary>
    /// Full date and full time format.
    /// </summary>
    FullDateFullTime = 6,
        
    /// <summary>
    /// Date and short time format.
    /// </summary>
    DateShortTime = 7,
        
    /// <summary>
    /// Short date and short time format.
    /// </summary>
    ShortDateShortTime = 8,
        
    /// <summary>
    /// Long date and full time format.
    /// </summary>
    LongDateFullTime = 9
}