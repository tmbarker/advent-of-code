namespace Utilities.Graph;

/// <summary>
///     A generic node in a <see cref="BinaryTree{T}" /> instance.
/// </summary>
/// <param name="value">The node value</param>
/// <typeparam name="T">The type associated with node value</typeparam>
public sealed class BinaryTreeNode<T>(T value)
{
    public T Value { get; set; } = value;
    public BinaryTreeNode<T>? Left { get; set; }
    public BinaryTreeNode<T>? Right { get; set; }

    /// <summary>
    ///     Print the <see cref="BinaryTreeNode{T}" /> and it's children to the console.
    /// </summary>
    /// <param name="formatter">When not specified nodes are printed using the default ToString implementation</param>
    /// <param name="spacing">The minimum spacing between each run of formatted node text</param>
    /// <param name="topMargin">How many empty lines should precede the root node</param>
    /// <param name="leftMargin">The margin to the furthest left node</param>
    public void Print(Func<T, string>? formatter = null, int spacing = 2, int topMargin = 1, int leftMargin = 1)
    {
        BinaryTreePrinter.Print(root: this, formatter, spacing, topMargin, leftMargin);
    }
}