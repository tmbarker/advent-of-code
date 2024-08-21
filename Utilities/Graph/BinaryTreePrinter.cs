namespace Utilities.Graph;

/// <summary>
///     A utility class for printing <see cref="BinaryTreeNode{T}" />'s to the console. Printing a
///     <see cref="BinaryTree{T}" />
///     <see cref="BinaryTree{T}.Root" /> node will print the entire tree.
/// </summary>
public static class BinaryTreePrinter
{
    private class VisualNodeInfo<TNode>
    {
        public required BinaryTreeNode<TNode> LogicalNode { get; init; }
        public required string Text { get; init; }
        public int StartPos { get; set; }

        public int EndPos
        {
            get => StartPos + Text.Length;
            set => StartPos = value - Text.Length;
        }

        public VisualNodeInfo<TNode>? Parent { get; set; }
        public VisualNodeInfo<TNode>? Left { get; set; }
        public VisualNodeInfo<TNode>? Right { get; set; }
    }

    /// <summary>
    ///     Print the <see cref="BinaryTreeNode{T}" /> and it's children to the console.
    /// </summary>
    /// <param name="root">The node to start printing from</param>
    /// <param name="formatter">When not specified nodes are printed using the default ToString implementation</param>
    /// <param name="spacing">The minimum spacing between each run of formatted node text</param>
    /// <param name="topMargin">How many empty lines should precede the root node</param>
    /// <param name="leftMargin">The margin to the furthest left node</param>
    public static void Print<TNode>(this BinaryTreeNode<TNode> root, Func<TNode, string>? formatter = null,
        int spacing = 2, int topMargin = 1, int leftMargin = 1)
    {
        var rootTop = Console.CursorTop + topMargin;
        var last = new List<VisualNodeInfo<TNode>>();
        var next = root;

        for (var level = 0; next != null; level++)
        {
            var item = new VisualNodeInfo<TNode>
            {
                LogicalNode = next,
                Text = formatter?.Invoke(next.Value) ?? next.Value?.ToString() ?? "null"
            };

            if (level < last.Count)
            {
                item.StartPos = last[level].EndPos + spacing;
                last[level] = item;
            }
            else
            {
                item.StartPos = leftMargin;
                last.Add(item);
            }

            if (level > 0)
            {
                item.Parent = last[level - 1];
                if (next == item.Parent.LogicalNode.Left)
                {
                    item.Parent.Left = item;
                    item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos - 1);
                }
                else
                {
                    item.Parent.Right = item;
                    item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos + 1);
                }
            }

            next = next.Left ?? next.Right;
            for (; next == null; item = item.Parent)
            {
                var top = rootTop + 2 * level;
                Print(item.Text, top, left: item.StartPos);
                if (item.Left != null)
                {
                    Print(s: "/", top + 1, item.Left.EndPos);
                    Print(s: "_", top, item.Left.EndPos + 1, item.StartPos);
                }

                if (item.Right != null)
                {
                    Print(s: "_", top, item.EndPos, item.Right.StartPos - 1);
                    Print(s: "\\", top + 1, item.Right.StartPos - 1);
                }

                if (--level < 0) break;
                if (item == item.Parent!.Left)
                {
                    item.Parent.StartPos = item.EndPos + 1;
                    next = item.Parent.LogicalNode.Right;
                }
                else
                {
                    if (item.Parent.Left == null)
                    {
                        item.Parent.EndPos = item.StartPos - 1;
                    }
                    else
                    {
                        item.Parent.StartPos += (item.StartPos - 1 - item.Parent.EndPos) / 2;
                    }
                }
            }
        }

        Console.SetCursorPosition(left: 0, top: rootTop + 2 * last.Count - 1);
    }

    private static void Print(string s, int top, int left, int right = -1)
    {
        Console.SetCursorPosition(left, top);

        if (right < 0)
        {
            right = left + s.Length;
        }

        while (Console.CursorLeft < right)
        {
            Console.Write(s);
        }
    }
}