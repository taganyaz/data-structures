// See https://aka.ms/new-console-template for more information
// Problem: Calculate the sum of all node tilts in a binary tree
// Tilt Definition: For each node, tilt = |sum(left subtree) - sum(right subtree)|
// Return: The sum of tilts of all nodes in the tree

// Approach: Post-order DFS traversal with accumulation
// 1. Use post-order traversal to ensure children are processed before parents
// 2. For each node, recursively calculate left and right subtree sums
// 3. Calculate current node's tilt using the subtree sums
// 4. Accumulate tilts in a running total
// 5. Return subtree sum (node value + left sum + right sum) for parent calculations
//
// Time Complexity: O(n) - visit each node exactly once
// Space Complexity: O(h) - recursion stack depth, where h is tree height
//                    O(log n) for balanced tree, O(n) for skewed tree
//
// Example trace for tree:
//        12
//       /  \
//      6    18
//     / \   / \
//    5  14 13  7
//              /
//             3
//
// Post-order visits: 5, 14, 6, 13, 3, 7, 18, 12
// Node 5: left=0, right=0, tilt=0, returns 5
// Node 14: left=0, right=0, tilt=0, returns 14
// Node 6: left=5, right=14, tilt=|5-14|=9, returns 25
// Node 13: left=0, right=0, tilt=0, returns 13
// Node 3: left=0, right=0, tilt=0, returns 3
// Node 7: left=3, right=0, tilt=|3-0|=3, returns 10
// Node 18: left=13, right=10, tilt=|13-10|=3, returns 41
// Node 12: left=25, right=41, tilt=|25-41|=16, returns 78
// Total tilt = 0+0+9+0+0+3+3+16 = 31
//
// Design considerations:
// - Using instance variable for accumulation vs. passing by reference
// - Trade-off: Simpler code but not thread-safe
// - Alternative: Use ref parameter or tuple return (sum, tilt)
var tree = new TreeNode(12);
tree.Left = new TreeNode(6);
tree.Left.Left = new TreeNode(5);
tree.Left.Right = new TreeNode(14);

tree.Right = new TreeNode(18);
tree.Right.Left = new TreeNode(13);
tree.Right.Right = new TreeNode(7);
tree.Right.Right.Left = new TreeNode(3);

var sol = new Solution();
sol.CalculateNodeTiltSum(tree);

Console.WriteLine("Total tilt:" + sol.TotalTiltSum);
class Solution
{
    private int _totalTiltSum = 0;

    public int TotalTiltSum => _totalTiltSum;

    public int CalculateNodeTiltSum(TreeNode root)
    {
        if (root == null)
            return 0;

        int left = CalculateNodeTiltSum(root.Left);
        int right = CalculateNodeTiltSum(root.Right);

        int currentTilt = Math.Abs(left - right);
        _totalTiltSum += currentTilt;

        return root.Value + left + right;
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
