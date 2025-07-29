// See https://aka.ms/new-console-template for more information
/*
 * PROBLEM: Binary Tree Path Sum
 * Given a binary tree and a target sum, find all root-to-leaf paths where 
 * the sum of node values equals the target.
 * 
 * CONSTRAINTS & ASSUMPTIONS:
 * - Node values are positive integers (assumption based on early termination logic)
 * - Target sum can be any integer
 * - Path must be root-to-leaf (not root-to-any-node)
 * - Tree size: up to 10^4 nodes (typical interview constraint)
 * 
 * APPROACH: Depth-First Search with Backtracking
 * 
 * TIME COMPLEXITY: O(N^2) worst case
 * - Visit each node once: O(N)
 * - Copy path for each leaf: O(H) where H is height
 * - In balanced tree: O(N log N), skewed tree: O(N^2)
 * 
 * SPACE COMPLEXITY: O(N) worst case
 * - Recursion stack: O(H) 
 * - Path storage: O(H)
 * - Result storage: O(N) for storing all paths
 * 
 * ALGORITHM:
 * 1. Use DFS to traverse the tree while maintaining current path
 * 2. Track remaining sum by subtracting node values as we descend
 * 3. When reaching a leaf with remainingSum = 0, record the path
 * 4. Backtrack by removing current node before returning
 * 
 * OPTIMIZATION OPPORTUNITIES:
 * - Early termination when remainingSum < currentNode.Value (assumes positive values)
 * - Could use iterative approach with explicit stack to avoid recursion limits
 * - For negative values, would need to remove early termination
 * 
 * EDGE CASES HANDLED:
 * - Null tree
 * - Negative target (returns empty - discuss if this is correct behavior)
 * - Single node tree
 * - No valid paths
 */
var tree = new TreeNode(5);
tree.Left = new TreeNode(7);
tree.Right = new TreeNode(6);
tree.Left.Left = new TreeNode(9);
tree.Left.Right = new TreeNode(10);
tree.Right.Left = new TreeNode(11);
tree.Right.Right = new TreeNode(8);
int target = 22;
var result = new Solution().GetAllRootToLeafPathsSumToTarget(tree, target);
Console.WriteLine($"Paths sum to {target}: [{string.Join(", ", result.Select(s => $"[{string.Join(", ", s)}]"))}]");
class Solution
{
    public List<List<int>> GetAllRootToLeafPathsSumToTarget(TreeNode root, int target)
    {
        var result = new List<List<int>>();
        if (root == null)
            return result;

        if (target < 0)
        {
            return result;
        }
        DFS(root, target, new List<int>(), result);

        return result;

    }

    private void DFS(TreeNode node, int target, List<int> path, List<List<int>> paths)
    {
        if (target < node.Value)
            return;

        int remainingSum = target - node.Value;
        path.Add(node.Value);

        if (remainingSum == 0 && node.Left == null && node.Right == null)
        {
            paths.Add([.. path]);
        }

        // Remaining sum could be zero here, but either Left or right child could be zero too!
        if (node.Left != null)
        {
            DFS(node.Left, remainingSum, path, paths);
        }

        if (node.Right != null)
        {
            DFS(node.Right, remainingSum, path, paths);
        }

        path.RemoveAt(path.Count - 1);
    }
}
public class TreeNode
{
    public TreeNode(int value)
    {
        Value = value;
    }
    public int Value { get; private set; }
    public TreeNode? Left { get; set; } = null;
    public TreeNode? Right { get; set; } = null;
}
