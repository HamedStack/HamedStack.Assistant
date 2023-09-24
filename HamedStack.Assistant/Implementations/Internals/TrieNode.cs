namespace HamedStack.Assistant.Implementations.Internals;

internal class TrieNode
{
    internal Dictionary<char, TrieNode> Children { get; } = new();
    internal bool IsWord { get; set; }
}