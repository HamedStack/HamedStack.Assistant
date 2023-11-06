// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.Runtime.InteropServices;

namespace HamedStack.Assistant.Extensions.SpanExtended;

public static class SpanExtensions
{
    public static T Read<T>(this ReadOnlySpan<byte> span) where T : unmanaged => MemoryMarshal.Read<T>(span);

    public static void Write<T>(this Span<byte> span, T obj) where T : unmanaged => MemoryMarshal.Write(span, ref obj);
}