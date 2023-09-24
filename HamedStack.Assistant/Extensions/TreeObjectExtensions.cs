
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

using HamedStack.Assistant.Abstractions;

namespace HamedStack.Assistant.Extensions;

/// <summary>
/// Provides extension methods for working with tree structures.
/// </summary>
public static class TreeObjectExtensions
{
    /// <summary>
    /// Performs a Breadth-First Search (BFS) traversal on the tree, starting from the specified root node.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>An enumerable collection of nodes in BFS order.</returns>
    public static IEnumerable<ITreeObject<T>> BreadthFirstSearch<T>(
        this ITreeObject<T> rootNode)
    {
        var queue = new Queue<ITreeObject<T>>();
        queue.Enqueue(rootNode);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return current;

            foreach (var child in current.Children)
                queue.Enqueue(child);
        }
    }

    /// <summary>
    /// Calculates the depth of the tree, i.e., the maximum distance from the root node to any leaf node.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>The depth of the tree.</returns>
    public static int CalculateTreeDepth<T>(this ITreeObject<T> rootNode)
    {
        if (rootNode.Children.Count == 0)
            return 0;

        int maxChildDepth = 0;
        foreach (var child in rootNode.Children)
        {
            int childDepth = child.CalculateTreeDepth();
            maxChildDepth = Math.Max(maxChildDepth, childDepth);
        }

        return maxChildDepth + 1;
    }

    /// <summary>
    /// Counts the total number of internal nodes in the tree (non-leaf nodes).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>The total number of internal nodes in the tree.</returns>
    public static int CountInternalNodes<T>(this ITreeObject<T> rootNode)
    {
        return rootNode.ToFlatten().Count(node => !node.IsLeaf());
    }

    /// <summary>
    /// Counts the total number of leaf nodes in the tree.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>The total number of leaf nodes in the tree.</returns>
    public static int CountLeafNodes<T>(this ITreeObject<T> rootNode)
    {
        return rootNode.GetLeafNodes().Count();
    }

    /// <summary>
    /// Counts the total number of nodes in the tree (including the root node and all descendants).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>The total number of nodes in the tree.</returns>
    public static int CountNodes<T>(this ITreeObject<T> rootNode)
    {
        return rootNode.ToFlatten().Count();
    }

    /// <summary>
    /// Counts the total number of nodes at a specific depth in the tree.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <param name="depth">The depth at which to count nodes.</param>
    /// <returns>The total number of nodes at the specified depth.</returns>
    public static int CountNodesAtDepth<T>(this ITreeObject<T> rootNode, int depth)
    {
        return rootNode.GetNodesAtDepth(depth).Count();
    }

    /// <summary>
    /// Performs a Depth-First Search (DFS) traversal on the tree, starting from the specified root node.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>An enumerable collection of nodes in DFS order.</returns>
    public static IEnumerable<ITreeObject<T>> DepthFirstSearch<T>(
        this ITreeObject<T> rootNode)
    {
        var stack = new Stack<ITreeObject<T>>();
        stack.Push(rootNode);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            yield return current;

            foreach (var child in current.Children)
                stack.Push(child);
        }
    }
    /// <summary>
    /// Retrieves all leaf nodes that match a specific condition using Depth-First Search (DFS).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <param name="predicate">The condition to match leaf nodes against.</param>
    /// <returns>An enumerable collection of leaf nodes that match the condition.</returns>
    public static IEnumerable<ITreeObject<T>> FindLeafNodes<T>(this ITreeObject<T> rootNode, Func<ITreeObject<T>, bool> predicate)
    {
        return rootNode.ToFlatten().Where(node => node.IsLeaf() && predicate(node));
    }

    /// <summary>
    /// Finds a node in the tree structure by its identifier using Depth-First Search (DFS).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <param name="id">The identifier of the node to find.</param>
    /// <returns>The found node or null if not found.</returns>
    public static ITreeObject<T>? FindNodeById<T>(
        this ITreeObject<T> rootNode,
        T id)
    {
        foreach (var node in rootNode.DepthFirstSearch())
        {
            if (EqualityComparer<T>.Default.Equals(node.Id, id))
                return node;
        }
        return null;
    }

    /// <summary>
    /// Retrieves all nodes that match a specific condition using Depth-First Search (DFS).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <param name="predicate">The condition to match nodes against.</param>
    /// <returns>An enumerable collection of nodes that match the condition.</returns>
    public static IEnumerable<ITreeObject<T>> FindNodes<T>(this ITreeObject<T> rootNode, Func<ITreeObject<T>, bool> predicate)
    {
        return rootNode.DepthFirstSearch().Where(predicate);
    }

    /// <summary>
    /// Retrieves all ancestor nodes of the given node (nodes above it in the hierarchy).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node whose ancestors will be retrieved.</param>
    /// <returns>An enumerable collection of ancestor nodes.</returns>
    public static IEnumerable<ITreeObject<T>> GetAncestors<T>(this ITreeObject<T> node)
    {
        var currentNode = node;
        while (currentNode.ParentId != null)
        {
            var parentNode = currentNode.FindNodeById(currentNode.ParentId);
            if (parentNode != null)
            {
                yield return parentNode;
                currentNode = parentNode;
            }
            else
            {
                break; // In case of an invalid parent reference
            }
        }
    }

    /// <summary>
    /// Retrieves the common ancestor of two nodes, if it exists.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node1">The first node.</param>
    /// <param name="node2">The second node.</param>
    /// <returns>The common ancestor node, or null if no common ancestor exists.</returns>
    public static ITreeObject<T>? GetCommonAncestor<T>(this ITreeObject<T> node1, ITreeObject<T> node2)
    {
        var ancestors1 = node1.GetAncestors().ToHashSet();
        foreach (var ancestor2 in node2.GetAncestors())
        {
            if (ancestors1.Contains(ancestor2))
            {
                return ancestor2;
            }
        }
        return null;
    }

    /// <summary>
    /// Retrieves all descendant nodes of the given node (nodes below it in the hierarchy).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node whose descendants will be retrieved.</param>
    /// <returns>An enumerable collection of descendant nodes.</returns>
    public static IEnumerable<ITreeObject<T>> GetDescendants<T>(this ITreeObject<T> node)
    {
        foreach (var child in node.Children)
        {
            yield return child;
            foreach (var descendant in child.GetDescendants())
            {
                yield return descendant;
            }
        }
    }

    /// <summary>
    /// Retrieves all leaf nodes (nodes without children) in the tree.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>An enumerable collection of leaf nodes.</returns>
    public static IEnumerable<ITreeObject<T>> GetLeafNodes<T>(this ITreeObject<T> rootNode)
    {
        if (rootNode.Children.Count == 0)
        {
            yield return rootNode;
        }
        else
        {
            foreach (var child in rootNode.Children)
            {
                foreach (var leafNode in child.GetLeafNodes())
                {
                    yield return leafNode;
                }
            }
        }
    }

    /// <summary>
    /// Retrieves the lowest common ancestor of two nodes, if it exists.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node1">The first node.</param>
    /// <param name="node2">The second node.</param>
    /// <returns>The lowest common ancestor node, or null if no common ancestor exists.</returns>
    public static ITreeObject<T>? GetLowestCommonAncestor<T>(this ITreeObject<T> node1, ITreeObject<T> node2)
    {
        var ancestors1 = node1.GetAncestors().ToHashSet();
        foreach (var ancestor2 in node2.GetAncestors())
        {
            if (ancestors1.Contains(ancestor2))
            {
                return ancestor2;
            }
        }
        return null;
    }

    /// <summary>
    /// Retrieves the maximum depth of the tree (distance from root to the deepest leaf).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>The maximum depth of the tree.</returns>
    public static int GetMaxTreeDepth<T>(this ITreeObject<T> rootNode)
    {
        int maxDepth = 0;

        foreach (var node in rootNode.GetDescendants().Where(node => node.IsLeaf()))
        {
            maxDepth = Math.Max(maxDepth, node.GetNodeDepth());
        }

        return maxDepth;
    }

    /// <summary>
    /// Retrieves the sibling nodes that appear after the given node in the parent's children list.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node whose next siblings will be retrieved.</param>
    /// <returns>An enumerable collection of sibling nodes that come after the given node.</returns>
    public static IEnumerable<ITreeObject<T?>> GetNextSiblings<T>(this ITreeObject<T> node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        var parentNode = node!.FindNodeById(node.ParentId);
        if (parentNode != null)
        {
            bool foundNode = false;

            foreach (var sibling in parentNode.Children)
            {
                if (foundNode)
                {
                    yield return sibling;
                }
                else if (EqualityComparer<T>.Default.Equals(sibling.Id, node.Id))
                {
                    foundNode = true;
                }
            }
        }
    }

    /// <summary>
    /// Retrieves the depth of the given node (distance from the root node).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node whose depth will be retrieved.</param>
    /// <returns>The depth of the node in the tree.</returns>
    public static int GetNodeDepth<T>(this ITreeObject<T> node)
    {
        return node.GetNodeLevel();
    }

    /// <summary>
    /// Retrieves the level of the node in the tree (root node is at level 0).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node whose level will be retrieved.</param>
    /// <returns>The level of the node in the tree.</returns>
    public static int GetNodeLevel<T>(this ITreeObject<T> node)
    {
        int level = 0;
        var currentNode = node;

        while (currentNode != null && currentNode.ParentId != null)
        {
            level++;
            currentNode = currentNode.FindNodeById(currentNode.ParentId);
        }

        return level;
    }

    /// <summary>
    /// Retrieves all nodes at a specific depth in the tree.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <param name="depth">The depth at which to retrieve nodes.</param>
    /// <returns>An enumerable collection of nodes at the specified depth.</returns>
    public static IEnumerable<ITreeObject<T>> GetNodesAtDepth<T>(this ITreeObject<T> rootNode, int depth)
    {
        if (depth < 0)
            yield break;

        if (depth == 0)
        {
            yield return rootNode;
        }
        else
        {
            foreach (var child in rootNode.Children)
            {
                foreach (var node in child.GetNodesAtDepth(depth - 1))
                {
                    yield return node;
                }
            }
        }
    }

    /// <summary>
    /// Retrieves all nodes that have a specific level in the tree.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <param name="level">The level at which to retrieve nodes.</param>
    /// <returns>An enumerable collection of nodes at the specified level.</returns>
    public static IEnumerable<ITreeObject<T>> GetNodesAtLevel<T>(this ITreeObject<T> rootNode, int level)
    {
        return rootNode.ToFlatten().Where(node => node.GetNodeLevel() == level);
    }

    /// <summary>
    /// Retrieves all nodes that have the same depth as the given node.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node for which to retrieve nodes at the same depth.</param>
    /// <returns>An enumerable collection of nodes at the same depth.</returns>
    public static IEnumerable<ITreeObject<T>>? GetNodesAtSameDepth<T>(this ITreeObject<T> node)
    {
        var root = node.GetRoot();
        var depth = node.GetNodeDepth();

        return root?.GetNodesAtDepth(depth);
    }

    /// <summary>
    /// Retrieves all nodes that have no siblings (only children of their parent).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>An enumerable collection of nodes with no siblings.</returns>
    public static IEnumerable<ITreeObject<T>> GetNodesWithNoSiblings<T>(this ITreeObject<T> rootNode)
    {
        return rootNode.ToFlatten().Where(node => node.HasNoSiblings());
    }

    /// <summary>
    /// Retrieves the node with the maximum depth in the tree.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>The node with the maximum depth in the tree.</returns>
    public static ITreeObject<T>? GetNodeWithMaxDepth<T>(this ITreeObject<T> rootNode)
    {
        return rootNode.ToFlatten().MaxBy(node => node.GetNodeDepth());
    }

    /// <summary>
    /// Retrieves the path from the root to the given node, inclusive of both root and node.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node for which to retrieve the path.</param>
    /// <returns>An enumerable collection representing the path from root to the node.</returns>
    public static IEnumerable<ITreeObject<T>> GetPathToNode<T>(this ITreeObject<T> node)
    {
        return node.GetAncestors().Concat(new[] { node }).Reverse();
    }

    /// <summary>
    /// Retrieves the sibling nodes that appear before the given node in the parent's children list.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node whose previous siblings will be retrieved.</param>
    /// <returns>An enumerable collection of sibling nodes that come before the given node.</returns>
    public static IEnumerable<ITreeObject<T?>> GetPreviousSiblings<T>(this ITreeObject<T> node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        var parentNode = node!.FindNodeById(node.ParentId);
        if (parentNode != null)
        {
            foreach (var sibling in parentNode.Children)
            {
                if (EqualityComparer<T>.Default.Equals(sibling.Id, node.Id))
                {
                    break;
                }

                yield return sibling;
            }
        }
    }

    /// <summary>
    /// Retrieves the root node of the tree containing the given node.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node for which to find the root.</param>
    /// <returns>The root node of the tree containing the given node.</returns>
    public static ITreeObject<T>? GetRoot<T>(this ITreeObject<T> node)
    {
        var currentNode = node;

        while (currentNode != null && currentNode.ParentId != null)
        {
            currentNode = currentNode.FindNodeById(currentNode.ParentId);
        }

        return currentNode;
    }

    /// <summary>
    /// Retrieves all root nodes from a collection of nodes.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="nodes">The nodes to consider.</param>
    /// <returns>An enumerable collection of root nodes.</returns>
    public static IEnumerable<ITreeObject<T>> GetRootNodes<T>(this IEnumerable<ITreeObject<T>> nodes)
    {
        return nodes.Where(node => node.IsRoot());
    }

    /// <summary>
    /// Retrieves all sibling nodes (nodes with the same parent) of the given node.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node whose siblings will be retrieved.</param>
    /// <returns>An enumerable collection of sibling nodes.</returns>
    public static IEnumerable<ITreeObject<T>> GetSiblings<T>(this ITreeObject<T> node)
    {
        if (node.ParentId != null)
        {
            var parentNode = node.FindNodeById(node.ParentId);
            if (parentNode != null)
            {
                foreach (var sibling in parentNode.Children)
                {
                    if (!EqualityComparer<T>.Default.Equals(sibling.Id, node.Id))
                    {
                        yield return sibling;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Retrieves the total number of nodes in the entire tree, including the root and all descendants.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>The total number of nodes in the tree.</returns>
    public static int GetTotalNodeCount<T>(this ITreeObject<T> rootNode)
    {
        return rootNode.ToFlatten().Count();
    }

    /// <summary>
    /// Checks if a node has no siblings (only child of its parent).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node to check.</param>
    /// <returns>True if the node has no siblings, false otherwise.</returns>
    public static bool HasNoSiblings<T>(this ITreeObject<T> node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        var parent = node!.FindNodeById(node.ParentId);
        return parent is { Children.Count: 1 };
    }

    /// <summary>
    /// Checks if a node is an ancestor of another node.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node to check.</param>
    /// <param name="potentialDescendant">The potential descendant node.</param>
    /// <returns>True if the node is an ancestor of the potential descendant, false otherwise.</returns>
    public static bool IsAncestorOf<T>(this ITreeObject<T> node, ITreeObject<T> potentialDescendant)
    {
        return potentialDescendant.IsDescendantOf(node);
    }

    /// <summary>
    /// Checks if the tree is a binary tree (each node has at most two children).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>True if the tree is a binary tree, false otherwise.</returns>
    public static bool IsBinaryTree<T>(this ITreeObject<T> rootNode)
    {
        foreach (var node in rootNode.ToFlatten())
        {
            if (node.Children.Count > 2)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if the tree is complete (all levels are fully filled except possibly the last level, which is filled left to right).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>True if the tree is complete, false otherwise.</returns>
    public static bool IsCompleteTree<T>(this ITreeObject<T> rootNode)
    {
        var maxDepth = rootNode.GetMaxTreeDepth();
        for (int i = 0; i < maxDepth - 1; i++)
        {
            if (rootNode.CountNodesAtDepth(i) < (int)Math.Pow(2, i))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Checks if a node is a descendant of another node.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node to check.</param>
    /// <param name="potentialAncestor">The potential ancestor node.</param>
    /// <returns>True if the node is a descendant of the potential ancestor, false otherwise.</returns>
    public static bool IsDescendantOf<T>(this ITreeObject<T> node, ITreeObject<T> potentialAncestor)
    {
        return node.GetAncestors().Any(ancestor => ancestor.Id != null && ancestor.Id.Equals(potentialAncestor.Id));
    }

    /// <summary>
    /// Checks if a node is a direct ancestor of another node.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="ancestor">The potential ancestor node.</param>
    /// <param name="node">The node to check.</param>
    /// <returns>True if the ancestor is a direct ancestor of the node, false otherwise.</returns>
    public static bool IsDirectAncestorOf<T>(this ITreeObject<T> ancestor, ITreeObject<T> node)
    {
        return node.GetAncestors().Any(a => a.Id != null && a.Id.Equals(ancestor.Id));
    }

    /// <summary>
    /// Checks if a node is a direct descendant of another node.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node to check.</param>
    /// <param name="potentialParent">The potential parent node.</param>
    /// <returns>True if the node is a direct descendant of the potential parent, false otherwise.</returns>
    public static bool IsDirectDescendantOf<T>(this ITreeObject<T> node, ITreeObject<T> potentialParent)
    {
        return node.ParentId != null && node.ParentId.Equals(potentialParent.Id);
    }

    /// <summary>
    /// Checks if a node is a leaf node (has no children) and is also a direct child of its parent.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node to check.</param>
    /// <returns>True if the node is a direct leaf node, false otherwise.</returns>
    public static bool IsDirectLeaf<T>(this ITreeObject<T> node)
    {
        return node.IsLeaf() && node.ParentId != null;
    }

    /// <summary>
    /// Checks if the tree is a forest (multiple disconnected trees).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNodes">The root nodes of the potential forest.</param>
    /// <returns>True if the structure is a forest, false otherwise.</returns>
    public static bool IsForest<T>(this IEnumerable<ITreeObject<T>> rootNodes)
    {
        return rootNodes.All(node => node.ParentId == null);
    }

    /// <summary>
    /// Checks if a node is a leaf node (has no children).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node to check.</param>
    /// <returns>True if the node is a leaf node, false otherwise.</returns>
    public static bool IsLeaf<T>(this ITreeObject<T> node)
    {
        return node.Children.Count == 0;
    }

    /// <summary>
    /// Checks if a node is an only child (has siblings but no other children).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node to check.</param>
    /// <returns>True if the node is an only child, false otherwise.</returns>
    public static bool IsOnlyChild<T>(this ITreeObject<T> node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        var parent = node!.FindNodeById(node.ParentId);

        if (parent == null || parent.Children.Count != 1)
            return false;

        if (parent.Children[0].Id == null)
            return false;

        return parent.Children[0].Id!.Equals(node.Id);
    }

    /// <summary>
    /// Checks if the tree is a perfect binary tree (all internal nodes have exactly two children).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>True if the tree is a perfect binary tree, false otherwise.</returns>
    public static bool IsPerfectBinaryTree<T>(this ITreeObject<T> rootNode)
    {
        var totalInternalNodes = rootNode.CountInternalNodes();
        var leafNodes = rootNode.CountLeafNodes();

        // For a perfect binary tree, the number of internal nodes is one less than the number of leaf nodes
        return totalInternalNodes == leafNodes - 1;
    }

    /// <summary>
    /// Checks if a node is the root node of the tree.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node to check.</param>
    /// <returns>True if the node is the root node, false otherwise.</returns>
    public static bool IsRoot<T>(this ITreeObject<T> node)
    {
        return node.ParentId == null;
    }

    /// <summary>
    /// Checks if a node is a sibling of another node (both share the same parent).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="node">The node to check.</param>
    /// <param name="potentialSibling">The potential sibling node.</param>
    /// <returns>True if the node is a sibling of the potential sibling, false otherwise.</returns>
    public static bool IsSiblingOf<T>(this ITreeObject<T> node, ITreeObject<T> potentialSibling)
    {
        if (node.ParentId != null && potentialSibling.ParentId != null)
        {
            return node.ParentId.Equals(potentialSibling.ParentId);
        }
        return false;
    }

    /// <summary>
    /// Checks whether the tree is balanced, i.e., the height of the left and right subtrees of every node differ by at most 1.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>True if the tree is balanced, false otherwise.</returns>
    public static bool IsTreeBalanced<T>(this ITreeObject<T> rootNode)
    {
        if (rootNode.Children.Count == 0)
            return true;

        int? firstSubtreeDepth = null;
        foreach (var child in rootNode.Children)
        {
            int childDepth = child.CalculateTreeDepth();

            if (firstSubtreeDepth == null)
            {
                firstSubtreeDepth = childDepth;
            }
            else if (Math.Abs(firstSubtreeDepth.Value - childDepth) > 1)
            {
                return false;
            }
        }

        return rootNode.Children.All(child => child.IsTreeBalanced());
    }

    /// <summary>
    /// Reconstructs a tree structure from a collection of flattened nodes.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="flattenedNodes">A collection of flattened tree nodes.</param>
    /// <returns>The root node of the reconstructed tree, or null if no root node is found.</returns>
    public static ITreeObject<T>? ReconstructTree<T>(
        this IEnumerable<ITreeObject<T>> flattenedNodes)
        where T : notnull
    {
        var treeObjects = flattenedNodes.ToList();
        var nodeLookup = treeObjects.ToDictionary(node => node.Id);
        ITreeObject<T>? root = null;

        foreach (var node in treeObjects)
        {
            if (node.ParentId is null)
            {
                root = node;
            }
            else if (nodeLookup.TryGetValue(node.ParentId, out var parent))
            {
                parent.Children.Add(node);
            }
        }
        return root;
    }

    /// <summary>
    /// Flattens the tree structure into an enumerable collection of nodes.
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <returns>An enumerable collection of flattened tree nodes.</returns>
    public static IEnumerable<ITreeObject<T>> ToFlatten<T>(
        this ITreeObject<T> rootNode)
    {
        return new[] { rootNode }.Concat(rootNode.Children.SelectMany(ToFlatten));
    }
    /// <summary>
    /// Performs an action on each node while walking through the tree using Depth-First Search (DFS).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <param name="action">The action to be performed on each node.</param>
    public static void WalkOnNodes<T>(
        this ITreeObject<T> rootNode,
        Action<ITreeObject<T>> action)
    {
        foreach (var node in rootNode.DepthFirstSearch())
            action(node);
    }
    /// <summary>
    /// Performs an action on each leaf node in the tree using Depth-First Search (DFS).
    /// </summary>
    /// <typeparam name="T">The type of the node identifier.</typeparam>
    /// <param name="rootNode">The root node of the tree.</param>
    /// <param name="action">The action to be performed on each leaf node.</param>
    public static void WalkOnLeafNodes<T>(
        this ITreeObject<T> rootNode,
        Action<ITreeObject<T>> action)
    {
        foreach (var leafNode in rootNode.GetLeafNodes())
        {
            action(leafNode);
        }
    }
}






