// See https://aka.ms/new-console-template for more information
// Problem: Given the root of a binary tree, return the diameter of the tree.
// The diameter is the length of the longest path between any two nodes, measured in edges.
// This path may or may not pass through the root.

// Example:
//        10
//       /  \
//      5    18
//     / \   /
//    7   9 16
//         \
//          12
//         /
//        11
// Diameter = 6 (path: 11->12->9->5->10->18->16)

// Approach: Post-order DFS with height calculation
// 
// Key Insight: At each node, the longest path either:
// 1. Passes through the current node (left_height + right_height)
// 2. Exists entirely in one of the subtrees
//
// Algorithm:
// 1. Perform post-order traversal (process children before parent)
// 2. For each node, calculate:
//    - Height of left subtree
//    - Height of right subtree
//    - Potential diameter through this node = left_height + right_height
// 3. Track global maximum diameter across all nodes
// 4. Return height to parent = 1 + max(left_height, right_height)
//
// Time Complexity: O(n) - visit each node once
// Space Complexity: 
//   - Recursive: O(h) where h is tree height (call stack)
//   - Iterative: O(n) for stack and heights dictionary
//
// Edge Cases:
// - Empty tree (null root): diameter = 0
// - Single node: diameter = 0
// - Skewed tree (linked list): diameter = n-1
var tree = new TreeNode(10);
tree.Left = new TreeNode(5);
tree.Left.Left = new TreeNode(7);
tree.Left.Right = new TreeNode(9);
tree.Left.Right.Right = new TreeNode(12);
tree.Left.Right.Right.Left = new TreeNode(11);

tree.Right = new TreeNode(18);
tree.Right.Left = new TreeNode(16);

Console.WriteLine("Tree Diameter: " + new Solution().FindDiameter(tree));
Console.WriteLine("Tree Diameter Iter: " + new Solution().FindDiameterIterative(tree));
class Solution
{
    private int _maxDiameter = 0;

    public int FindDiameter(TreeNode root)
    {
        FindDiameterUtil(root);

        return _maxDiameter;
    }

    private int FindDiameterUtil(TreeNode root)
    {
        if (root == null)
            return 0;

        int leftHeight = FindDiameterUtil(root.Left);
        int rightHeight = FindDiameterUtil(root.Right);

        _maxDiameter = Math.Max(_maxDiameter, leftHeight + rightHeight);

        return 1 + Math.Max(leftHeight, rightHeight);
    }

    public int FindDiameterIterative(TreeNode root)
    {
        if (root == null)
            return 0;

        int maxDiameter = 0;
        var stack = new Stack<(TreeNode node, bool visited)>();
        var heights = new Dictionary<TreeNode, int>();

        stack.Push((root, false));

        while (stack.Count > 0)
        {
            var (currentNode, visited) = stack.Pop();

            if (visited)
            {
                int leftHeight = 0;
                int rightHeight = 0;

                if (currentNode.Left != null && heights.ContainsKey(currentNode.Left))
                    leftHeight = heights[currentNode.Left];

                if (currentNode.Right != null && heights.ContainsKey(currentNode.Right))
                    rightHeight = heights[currentNode.Right];

                maxDiameter = Math.Max(maxDiameter, leftHeight + rightHeight);

                heights[currentNode] = 1 + Math.Max(leftHeight, rightHeight);
            }
            else
            {
                stack.Push((currentNode, true));

                if (currentNode.Left != null)
                    stack.Push((currentNode.Left, false));

                if (currentNode.Right != null)
                    stack.Push((currentNode.Right, false));
            }
        }

        return maxDiameter;
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
