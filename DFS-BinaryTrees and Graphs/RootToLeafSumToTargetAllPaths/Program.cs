// See https://aka.ms/new-console-template for more information
// Problem: Find All Root-to-Leaf Paths with Target Sum
// Given the root of a binary tree and an integer target, return all root-to-leaf paths 
// where the sum of node values along the path equals the target.
//
// Example:
//        5
//       / \
//      7   6
//     / \   / \
//    3   2  2  8
//       / \  /
//      1   4 2
// Target: 15
// Output: ["5->7->3", "5->7->2->1", "5->6->2->2"]
//
// Approach 1: Recursive DFS with Backtracking
// Time Complexity: O(N) where N is the number of nodes
// Space Complexity: O(H) for recursion stack + O(L*H) for storing paths
//                   where H is height of tree and L is number of leaf nodes
// 
// Algorithm:
// 1. Use DFS to traverse from root to leaves
// 2. Maintain a running path and remaining sum at each node
// 3. At each node:
//    - Add current node value to path
//    - Subtract node value from remaining target
//    - If leaf node and remaining sum equals 0, save current path
//    - Recursively explore left and right subtrees
//    - Backtrack by removing current node from path
//
// Key Insights:
// - Backtracking ensures path list is reused efficiently
// - Subtracting from target avoids sum overflow for large paths
// - Early termination possible if all values are positive
//
// Approach 2: Iterative DFS using Stack
// Time Complexity: O(N) where N is the number of nodes  
// Space Complexity: O(N) in worst case for stack storage
//
// Algorithm:
// 1. Use explicit stack to simulate DFS traversal
// 2. Each stack entry contains: (node, remaining_sum, path_so_far)
// 3. For each popped node:
//    - Create new path including current node
//    - If leaf with matching sum, add path to results
//    - Push children with updated sum and path
//
// Trade-offs:
// - Recursive: Cleaner code, risk of stack overflow for deep trees
// - Iterative: More memory per node, but avoids call stack limitations
//
// Edge Cases Handled:
// - Empty tree (null root)
// - Single node tree
// - No valid paths exist
// - Multiple valid paths
// - Negative node values
var tree = new TreeNode(5);
tree.Left = new TreeNode(7);
tree.Left.Left = new TreeNode(3);
tree.Left.Right = new TreeNode(2);
tree.Left.Right.Left = new TreeNode(1);
tree.Left.Right.Right = new TreeNode(4);

tree.Right = new TreeNode(6);
tree.Right.Left = new TreeNode(2);
tree.Right.Left.Left = new TreeNode(2);
tree.Right.Right = new TreeNode(8);
int target = 15;
var paths = new Solution().FindSumToTargetPaths(tree, target);

var paths2 = new Solution().FindSumToTargetPathsIterative(tree, target);

Console.WriteLine($"Paths Sum to {target}: {string.Join(", ", paths)}");

Console.WriteLine($"Paths Sum to {target} Iter: {string.Join(", ", paths2)}");
class Solution
{
    public List<string> FindSumToTargetPaths(TreeNode root, int target)
    {
        var paths = new List<string>();
        var currentPath = new List<int>();

        FindSumToTargetPaths(root, target, paths, currentPath);
        return paths;
    }

    private void FindSumToTargetPaths(TreeNode root, int target, List<string> paths, List<int> currentPath)
    {
        if (root == null)
            return;

        int runningSum = target - root.Value;
        currentPath.Add(root.Value);

        if (root.Left == null && root.Right == null && runningSum == 0)
        {
            var pathString = string.Join("->", currentPath);
            paths.Add(pathString);
        }
        else
        {
            FindSumToTargetPaths(root.Left, runningSum, paths, currentPath);
            FindSumToTargetPaths(root.Right, runningSum, paths, currentPath);
        }
        currentPath.RemoveAt(currentPath.Count - 1);
    }

    public List<string> FindSumToTargetPathsIterative(TreeNode root, int target)
    {
        List<string> paths = new List<string>();
        if (root == null)
            return paths;

        var stack = new Stack<(TreeNode node, int remainingSum, List<int> path)>();

        stack.Push((root, target, new List<int>()));

        while (stack.Count > 0)
        {
            var (currentNode, remainingSum, path) = stack.Pop();

            var newPath = new List<int>(path) { currentNode.Value };

            if (currentNode.Left == null && currentNode.Right == null && currentNode.Value == remainingSum)
            {
                paths.Add(string.Join("->", newPath));
                continue;
            }

            if (currentNode.Left != null)
                stack.Push((currentNode.Left, remainingSum - currentNode.Value, newPath));

            if (currentNode.Right != null)
                stack.Push((currentNode.Right, remainingSum - currentNode.Value, newPath));
        }
        return paths;
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
