// ReSharper disable UnusedMember.Global

using System.Text;

namespace HamedStack.Assistant.Implementations;

/// <summary>
/// Generic base class for a tree structure.
/// </summary>
/// <typeparam name="T">The node type of the tree.</typeparam>
public abstract class Tree<T> where T : Tree<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Tree{T}"/> class.
    /// Constructor sets the parent node and adds this node to the parent's child nodes.
    /// </summary>
    /// <param name="parent">The parent node or null if it's a root node.</param>
    protected Tree(T? parent)
    {
        Parent = parent;
        Children = new List<T>();

        parent?.Children.Add((T)this);
    }

    /// <summary>
    /// Gets the ancestors of the current node (all nodes from this node up to the root).
    /// </summary>
    public IEnumerable<T> Ancestors
    {
        get
        {
            var current = (T)this;
            while (current.Parent != null)
            {
                yield return current.Parent;
                current = current.Parent;
            }
        }
    }

    /// <summary>
    /// Gets the list of child nodes of this tree node.
    /// </summary>
    public List<T> Children { get; }

    /// <summary>
    /// Gets the descendants of the current node (all nodes in the subtree rooted at this node).
    /// </summary>
    public IEnumerable<T> Descendants
    {
        get
        {
            foreach (var child in Children)
            {
                yield return child;
                foreach (var descendant in child.Descendants)
                {
                    yield return descendant;
                }
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether this node is a leaf node (has no children).
    /// </summary>
    public bool IsLeaf => Children.Count == 0;

    /// <summary>
    /// Gets a value indicating whether this node is a root node.
    /// </summary>
    public bool IsRoot => Parent == null;

    /// <summary>
    /// Gets the level of this node in the tree. The level is the number of hops to the root object.
    /// </summary>
    public int Level => IsRoot ? 0 : Parent!.Level + 1;

    /// <summary>
    /// Gets the maximum depth of the tree rooted at the current node.
    /// </summary>
    public int MaxDepth => IsLeaf ? 0 : Children.Max(child => 1 + child.MaxDepth);

    /// <summary>
    /// Gets or sets the name associated with the tree node.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets the depth of the current node in the tree.
    /// </summary>
    public int NodeDepth => IsRoot ? 0 : Parent!.NodeDepth + 1;
    
    /// <summary>
    /// Gets the parent node of this tree node.
    /// </summary>
    public T? Parent { get; protected set; }
    /// <summary>
    /// Gets the siblings of the current node (nodes that share the same parent).
    /// </summary>
    public IEnumerable<T> Siblings
    {
        get
        {
            if (Parent != null)
            {
                foreach (var sibling in Parent.Children.Where(sibling => !sibling.Equals(this)))
                {
                    yield return sibling;
                }
            }
        }
    }

    /// <summary>
    /// Gets the total number of nodes in the subtree rooted at the current node.
    /// </summary>
    public int SubtreeNodeCount => IsLeaf ? 1 : Children.Sum(child => child.SubtreeNodeCount);

    /// <summary>
    /// Gets or sets the value associated with the tree node.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Reconstructs the original tree structure from a flattened list of nodes.
    /// </summary>
    /// <typeparam name="T">The node type of the tree.</typeparam>
    /// <param name="flattenedNodes">The flattened list of nodes.</param>
    /// <returns>The root node of the reconstructed tree.</returns>
    public static T? ReconstructTree(List<T> flattenedNodes)
    {
        // Create a dictionary to store the parent-child relationship
        var parentChildMap = new Dictionary<T, T>();

        // Iterate through the flattened nodes and populate the parent-child map
        foreach (var node in flattenedNodes)
        {
            foreach (var child in node.Children)
            {
                parentChildMap[child] = node;
            }
        }

        // Find the root node (a node with no parent)
        T? rootNode = null;
        foreach (var node in flattenedNodes)
        {
            if (!parentChildMap.ContainsKey(node))
            {
                rootNode = node;
                break;
            }
        }

        // If a root node is found, reconstruct the tree structure
        if (rootNode != null)
        {
            foreach (var node in flattenedNodes)
            {
                if (parentChildMap.TryGetValue(node, out var parent))
                {
                    parent.AddChildren(node);
                }
            }
        }

        return rootNode; // Return the root node of the reconstructed tree
    }

    /// <summary>
    /// Adds the specified nodes as children to the current node.
    /// </summary>
    /// <param name="children">The nodes to be added as children.</param>
    public void AddChildren(params T[] children)
    {
        Children.AddRange(children);
        foreach (var child in children)
        {
            child.Parent = (T)this;
        }
    }

    /// <summary>
    /// Calculates the average depth of all nodes in the subtree rooted at the current node.
    /// </summary>
    /// <returns>The average depth of nodes in the subtree.</returns>
    public double AverageNodeDepth()
    {
        var totalDepth = ToFlatten().Sum(node => node.NodeDepth);
        return (double)totalDepth / SubtreeNodeCount;
    }

    /// <summary>
    /// Performs breadth-first traversal of the tree and applies the specified action to each node.
    /// </summary>
    /// <param name="action">The action to apply to each node.</param>
    public void BreadthFirstTraversal(Action<T> action)
    {
        var queue = new Queue<T>();
        queue.Enqueue((T)this);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            action(node);

            foreach (var child in node.Children)
            {
                queue.Enqueue(child);
            }
        }
    }

    /// <summary>
    /// Clears all child nodes of the current node.
    /// </summary>
    public void ClearChildren()
    {
        Children.Clear();
    }

    /// <summary>
    /// Clones the subtree rooted at the current node and returns a new tree.
    /// </summary>
    /// <returns>A cloned tree with the same structure.</returns>
    public T CloneSubtree()
    {
        var clonedTree = (T?)Activator.CreateInstance(typeof(T), Parent);

        if (clonedTree != null)
        {
            foreach (var clonedChild in Children.Select(child => child.CloneSubtree()))
            {
                clonedTree.AddChildren(clonedChild);
            }

            return clonedTree;
        }

        throw new InvalidOperationException(
            "The type parameter T must have a constructor that takes a single parameter of type T.");

    }

    /// <summary>
    /// Creates a balanced binary tree from the nodes in the subtree rooted at the current node.
    /// </summary>
    /// <returns>A balanced binary tree.</returns>
    public T CreateBalancedBinaryTree()
    {
        var flattenedSubtree = ToFlatten().ToList();
        flattenedSubtree.Sort((node1, node2) => node1.NodeDepth.CompareTo(node2.NodeDepth));
        return CreateBalancedBinaryTreeRecursively(flattenedSubtree, 0, flattenedSubtree.Count - 1)!;
    }

    /// <summary>
    /// Performs depth-first traversal (in-order) of the tree and applies the specified action to each node.
    /// </summary>
    /// <param name="action">The action to apply to each node.</param>
    public void DepthFirstInOrderTraversal(Action<T> action)
    {
        if (Children.Count >= 1)
        {
            Children[0].DepthFirstInOrderTraversal(action);
        }
        action((T)this);
        for (var i = 1; i < Children.Count; i++)
        {
            Children[i].DepthFirstInOrderTraversal(action);
        }
    }

    /// <summary>
    /// Performs depth-first traversal (post-order) of the tree and applies the specified action to each node.
    /// </summary>
    /// <param name="action">The action to apply to each node.</param>
    public void DepthFirstPostOrderTraversal(Action<T> action)
    {
        foreach (var child in Children)
        {
            child.DepthFirstPostOrderTraversal(action);
        }
        action((T)this);
    }

    /// <summary>
    /// Performs depth-first traversal (pre-order) of the tree and applies the specified action to each node.
    /// </summary>
    /// <param name="action">The action to apply to each node.</param>
    public void DepthFirstPreOrderTraversal(Action<T> action)
    {
        action((T)this);
        foreach (var child in Children)
        {
            child.DepthFirstPreOrderTraversal(action);
        }
    }
    /// <summary>
    /// Finds a node in the tree that satisfies the given predicate.
    /// </summary>
    /// <param name="predicate">The predicate to check for a match.</param>
    /// <returns>The found node or null if no match is found.</returns>
    public T? FindNode(Func<T, bool> predicate)
    {
        if (predicate((T)this))
            return (T)this;

        return Children.Select(child => child.FindNode(predicate)).FirstOrDefault(found => found != null);
    }

    /// <summary>
    /// Finds a node by its name in the tree.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <returns>The found node or null if not found.</returns>
    public T? FindNodeByName(string name)
    {
        return FindNode(node => node.Name == name);
    }

    /// <summary>
    /// Finds a node by its value in the tree.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>The found node or null if not found.</returns>
    public T? FindNodeByValue(object? value)
    {
        return FindNode(node => node.Value == value);
    }

    /// <summary>
    /// Determines whether the tree rooted at the current node is a valid binary tree.
    /// </summary>
    /// <param name="comparator">A function that compares two nodes.</param>
    /// <returns>True if the tree is a valid binary tree, otherwise false.</returns>
    public bool IsValidBinaryTree(Func<T, T, bool> comparator)
    {
        return IsLeaf || Children.All(child => comparator((T)this, child) && child.IsValidBinaryTree(comparator));
    }

    /// <summary>
    /// Gets the number of nodes at the specified level in the subtree rooted at the current node.
    /// </summary>
    /// <param name="level">The level of the nodes to count.</param>
    /// <returns>The number of nodes at the specified level.</returns>
    public int NodesAtLevel(int level)
    {
        return level switch
        {
            < 0 => 0,
            0 => 1,
            _ => Children.Sum(child => child.NodesAtLevel(level - 1))
        };
    }

    /// <summary>
    /// Gets the nodes with a specific value in the subtree rooted at the current node.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>An enumeration of nodes with the specified value.</returns>
    public IEnumerable<T> NodesWithValue(object value)
    {
        return ToFlatten().Where(node => node.Value == value);
    }

    /// <summary>
    /// Removes the current node from its parent's children collection.
    /// </summary>
    public void Remove()
    {
        Parent?.Children.Remove((T)this);
    }
    /// <summary>
    /// Removes the entire subtree rooted at the current node, including the current node itself.
    /// </summary>
    public void RemoveSubtree()
    {
        foreach (var child in Children)
        {
            child.RemoveSubtree();
        }
        Remove();
    }
    /// <summary>
    /// Replaces the specified child node with another node.
    /// </summary>
    /// <param name="oldChild">The child node to replace.</param>
    /// <param name="newChild">The new node to be added as a child.</param>
    public void ReplaceChild(T? oldChild, T? newChild)
    {
        if (oldChild == null || newChild == null) return;
        var index = Children.IndexOf(oldChild);
        if (index < 0) return;
        Children[index] = newChild;
        newChild.Parent = (T)this;
        oldChild.Parent = null;
    }
    /// <summary>
    /// Reverses the order of child nodes in the current node.
    /// </summary>
    public void ReverseChildrenOrder()
    {
        Children.Reverse();
    }

    /// <summary>
    /// Flattens the subtree rooted at the current node into a list.
    /// </summary>
    /// <returns>A list containing all nodes in the subtree.</returns>
    public IEnumerable<T> ToFlatten()
    {
        var nodeList = new List<T> { (T)this };
        foreach (var child in Children)
        {
            nodeList.AddRange(child.ToFlatten());
        }
        return nodeList;
    }

    /// <summary>
    /// Returns a string representation of the tree rooted at the current node.
    /// </summary>
    /// <returns>A string representation of the tree.</returns>
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        BuildStringRepresentation(stringBuilder, 0);
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Returns a string representation of the tree rooted at the current node with names and values.
    /// </summary>
    /// <returns>A string representation of the tree.</returns>
    public string ToStringWithNamesAndValues()
    {
        var stringBuilder = new StringBuilder();
        BuildStringRepresentationWithNamesAndValues(stringBuilder, 0);
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Validates the tree rooted at the current node by applying the specified validation rule to all nodes.
    /// </summary>
    /// <param name="validationRule">The validation rule to check for each node.</param>
    /// <returns>True if all nodes in the tree satisfy the validation rule, otherwise false.</returns>
    public bool ValidateTree(Func<T, bool> validationRule)
    {
        return ToFlatten().All(validationRule);
    }

    private static T? CreateBalancedBinaryTreeRecursively(IReadOnlyList<T> nodes, int start, int end)
    {
        if (start > end)
            return null;

        var mid = (start + end) / 2;
        var midNode = nodes[mid];
        midNode.ClearChildren();

        var leftChild = CreateBalancedBinaryTreeRecursively(nodes, start, mid - 1);
        var rightChild = CreateBalancedBinaryTreeRecursively(nodes, mid + 1, end);

        if (leftChild != null)
            midNode.AddChildren(leftChild);

        if (rightChild != null)
            midNode.AddChildren(rightChild);

        return midNode;
    }

    private void BuildStringRepresentation(StringBuilder stringBuilder, int indentationLevel)
    {
        stringBuilder.Append(new string(' ', indentationLevel * 4));
        stringBuilder.AppendLine($"- {GetType().Name}");

        foreach (var child in Children)
        {
            child.BuildStringRepresentation(stringBuilder, indentationLevel + 1);
        }
    }
    private void BuildStringRepresentationWithNamesAndValues(StringBuilder stringBuilder, int indentationLevel)
    {
        stringBuilder.Append(new string(' ', indentationLevel * 4));
        stringBuilder.AppendLine($"- {Name} ({Value})");

        foreach (var child in Children)
        {
            child.BuildStringRepresentationWithNamesAndValues(stringBuilder, indentationLevel + 1);
        }
    }
}

