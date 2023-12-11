namespace Utilities.Graph;

/// <summary>
/// A generic binary tree.
/// </summary>
/// <param name="root">The node to set as the <see cref="Root"/></param>
/// <typeparam name="T">The type associated with each node value</typeparam>
public sealed class BinaryTree<T>(BinaryTreeNode<T> root)
{
    public BinaryTreeNode<T> Root { get; } = root;

    public BinaryTree(T rootValue) : this(root: new BinaryTreeNode<T>(value: rootValue))
    {
    }
    
    /// <summary>
    /// Print the <see cref="BinaryTree{T}"/> to the console, starting at the <see cref="Root"/>. 
    /// </summary>
    /// <param name="formatter">When not specified nodes are printed using the default ToString implementation</param>
    /// <param name="spacing">The minimum spacing between each run of formatted node text</param>
    /// <param name="topMargin">How many empty lines should precede the root node</param>
    /// <param name="leftMargin">The margin to the furthest left node</param>
    public void Print(Func<T, string>? formatter = null, int spacing = 2, int topMargin = 1, int leftMargin = 1)
    {
        Root.Print(formatter, spacing, topMargin, leftMargin);
    }
}