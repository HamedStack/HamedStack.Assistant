using HamedStack.Assistant.Enums;

namespace HamedStack.Assistant.Implementations;

public class DiffResult
{
    public string? NewText { get; set; }

    // Properties to store the old text, new text, and status of the change. The 'OldText' and
    // 'NewText' can be null for insertions and deletions respectively.
    public string? OldText { get; set; }

    public DiffChangeType Status { get; set; }
}