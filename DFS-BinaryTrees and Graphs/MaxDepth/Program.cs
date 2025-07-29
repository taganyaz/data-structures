// See https://aka.ms/new-console-template for more information
// Problem: Given the root of a binary tree, write a recursive function to find its maximum depth, where maximum depth is defined as the number of nodes along the longest path from the root node down to a leaf node.

var tree = new TreeNode(10);
tree.Left = new TreeNode(7);
tree.Left.Left = new TreeNode(3);
tree.Left.Left.Right = new TreeNode(12);
tree.Left.Left.Right.Right = new TreeNode(6);

tree.Right = new TreeNode(2);
tree.Right.Left = new TreeNode(9);
tree.Right.Right = new TreeNode(14);

var sol = new Solution();
sol.GetMaxDepth(tree);

Console.WriteLine("MaxDepth: " + sol.MaxDepth);
// Approach
// Recursive function that get max depth by computing the max depth for left subtree and right subtree
// to get max depth, get max of left subtree depth and right subtree and add 1
// If current depth is greater that the global maxDepth, update the global maxDepth to the new value
// When all nodes have been traversed, the global maxDepth with contain maximum tree depth
public class Solution
{
    private int _maxDepth = 0;

    public int MaxDepth => _maxDepth;

    public int GetMaxDepth(TreeNode root)
    {
        if (root == null)
            return 0;

        int leftDepth = GetMaxDepth(root.Left);
        int rightDepth = GetMaxDepth(root.Right);

        int currentDepth = int.Max(leftDepth, rightDepth) + 1;
        _maxDepth = int.Max(_maxDepth, currentDepth);

        return currentDepth;
    }
}
public class TreeNode
{
    public TreeNode(int value)
    {
        Value = value;
    }
    public int Value { get; private set; }
    public TreeNode Left { get; set; } = null!;
    public TreeNode Right { get; set; } = null!;
}
