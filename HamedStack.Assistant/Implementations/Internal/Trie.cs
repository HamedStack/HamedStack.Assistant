namespace HamedStack.Assistant.Implementations.Internal;

internal class Trie
{
    private readonly TrieNode _root = new();

    internal void AddWord(string word)
    {
        var node = _root;

        foreach (var c in word)
        {
            node.Children.TryAdd(c, new TrieNode());

            node = node.Children[c];
        }

        node.IsWord = true;
    }

    internal bool Search(string word, bool ignoreCase = false)
    {
        var node = _root;
        word = ignoreCase ? word.ToLower() : word;
        foreach (var c in word)
        {
            var cc = ignoreCase ? char.ToLower(c) : c;
            if (!node.Children.ContainsKey(cc))
                return false;

            node = node.Children[cc];
        }

        return node.IsWord;
    }
}