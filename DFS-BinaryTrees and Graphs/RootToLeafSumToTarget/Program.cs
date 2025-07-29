// See https://aka.ms/new-console-template for more information
// Problem: Given the root of a binary tree and an integer target, write a recursive function to determine 
// if the tree has a root-to-leaf path where all the values along that path sum to the target.

// Approach: Depth-First Search (DFS) with Running Sum
// 1. Base Cases:
//    - If node is null, return false (no valid path exists)
//    - If node is a leaf (no children) and remaining sum equals 0, return true (valid path found)
// 
// 2. Recursive Strategy:
//    - At each node, subtract the current node's value from the target
//    - Recursively check if either the left or right subtree contains a valid path with the updated target
//    - Use short-circuit evaluation (||) to stop searching once a valid path is found
//
// 3. Time Complexity: O(n) where n is the number of nodes (worst case: visit all nodes)
// 4. Space Complexity: O(h) where h is the height of the tree (recursive call stack)
//                      - Best case (balanced tree): O(log n)
//                      - Worst case (skewed tree): O(n)
//
// 5. Alternative Approaches:
//    - Iterative DFS using explicit stack
//    - BFS with level-order traversal (less efficient for this problem)
//    - Tracking the actual path using a list/array if path details are needed
//
// 6. Edge Cases Handled:
//    - Empty tree (root is null)
//    - Single node tree
//    - Negative node values and negative targets
//    - No valid path exists

var tree = new TreeNode(5);
tree.Left = new TreeNode(7);
tree.Left.Left = new TreeNode(7);
tree.Left.Right = new TreeNode(8);
tree.Left.Right.Right = new TreeNode(10);

tree.Right = new TreeNode(2);
tree.Right.Left = new TreeNode(4);

int target = 19;

Console.WriteLine($"Has Root to Leaf Path sum to {target}: {new Solution().HasPathToLeafSumToTarget(tree, target)}");

class Solution
{
    public bool HasPathToLeafSumToTarget(TreeNode? root, int currentTarget)
    {
        if (root == null)
            return false;

        int remainingTarget = currentTarget - root.Value;
        if (root.Left == null && root.Right == null && remainingTarget == 0)
            return true;

        return HasPathToLeafSumToTarget(root.Left, remainingTarget) || HasPathToLeafSumToTarget(root.Right, remainingTarget);
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
