namespace HamedStack.Assistant.Implementations.Internal;

internal class TrieNode
{
    internal Dictionary<char, TrieNode> Children { get; } = new();
    internal bool IsWord { get; set; }
}