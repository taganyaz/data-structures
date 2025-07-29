// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
// Problem: Given the root of a binary tree, write a recursive function to determine if it is a valid binary search tree.

// Approach: DFS with Min-Max Boundary Validation
// 
// Key Insight: 
// In a valid BST, every node must satisfy not just local constraints (left < root < right),
// but also global constraints inherited from all ancestors. A node's value must fall within
// a valid range determined by its position in the tree.
//
// BST Properties:
// 1. For any node, ALL values in its left subtree must be less than the node's value
// 2. For any node, ALL values in its right subtree must be greater than the node's value
// 3. These properties must hold recursively for every subtree
//
// Algorithm:
// 1. Start DFS traversal from root with initial bounds [MinValue, MaxValue]
// 2. For each node, verify it falls within its inherited bounds: min < node.value < max
// 3. When traversing left: update upper bound to current node's value
//    - Left subtree values must be less than current node
// 4. When traversing right: update lower bound to current node's value  
//    - Right subtree values must be greater than current node
// 5. Recursively validate both subtrees with their updated bounds
//
// Time Complexity: O(n) - visit each node once
// Space Complexity: O(h) - recursion stack depth, where h is tree height
//
// Edge Cases Handled:
// - Empty tree (null root) - returns true
// - Single node tree - always valid
// - Duplicate values - not allowed in this implementation
//
// Limitation: Using int.MinValue/MaxValue as bounds prevents these values from being valid node values

// ...existing code...

var tree = new TreeNode(12);
tree.Left = new TreeNode(5);
tree.Left.Left = new TreeNode(4);
tree.Left.Right = new TreeNode(6);

tree.Right = new TreeNode(18);
tree.Right.Left = new TreeNode(16);
tree.Right.Right = new TreeNode(24);
tree.Right.Left.Right = new TreeNode(17);

Console.WriteLine("Is BST:" + new Solution().IsBST(tree));

Console.WriteLine("Is BST Iter:" + new Solution().IsBSTIterative(tree));
class Solution
{
    public bool IsBST(TreeNode root)
    {
        return IsBST(root, int.MinValue, int.MaxValue);
    }

    private bool IsBST(TreeNode root, int min, int max)
    {
        if (root == null)
            return true;

        if (root.Value <= min || root.Value >= max)
            return false;

        return IsBST(root.Left, min, root.Value) && IsBST(root.Right, root.Value, max);
    }

    public bool IsBSTIterative(TreeNode root)
    {
        if (root == null)
            return true;

        var stack = new Stack<(TreeNode node, int min, int max)>();
        stack.Push((root, int.MinValue, int.MaxValue));

        while (stack.Count > 0)
        {
            (TreeNode node, int min, int max) = stack.Pop();

            if (node.Value <= min || node.Value >= max)
                return false;

            if (node.Left != null)
            {
                stack.Push((node.Left, min, node.Value));
            }

            if (node.Right != null)
            {
                stack.Push((node.Right, node.Value, max));
            }
        }
        return true;
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
