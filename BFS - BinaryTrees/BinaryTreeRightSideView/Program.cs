// See https://aka.ms/new-console-template for more information
// Approach: Level-Order Traversal (BFS)
// 
// Why BFS over DFS?
// - BFS naturally processes nodes level by level, making it straightforward to identify the rightmost node
// - DFS would require tracking depths and potentially overwriting values, adding complexity
// - BFS guarantees we visit all nodes at depth d before any node at depth d+1
//
// Algorithm Overview:
// We perform a level-order traversal, capturing only the last node of each level.
// This simulates "viewing from the right side" - the rightmost node at each depth is visible.
//
// Time Complexity: O(n) where n is the number of nodes
// - We visit each node exactly once
// - Queue operations (enqueue/dequeue) are O(1)
//
// Space Complexity: O(w) where w is the maximum width of the tree
// - In the worst case (complete binary tree), the last level contains n/2 nodes
// - Best case (skewed tree): O(1) space
// - Average balanced tree: O(n^0.5) space
//
// Trade-offs considered:
// - Queue-based BFS vs Array-based BFS: Queue is more memory efficient for sparse trees
// - BFS vs DFS: BFS is more intuitive for this problem and has clearer implementation
// - Could optimize space to O(h) using DFS, but adds complexity for marginal gain
//
// Edge Cases Handled:
// - Null/empty tree → returns empty list
// - Single node tree → returns [node.value]
// - Skewed trees (only left or right children) → works correctly
// - Complete binary trees → captures rightmost at each level

var tree = new TreeNode(1);
tree.Left = new TreeNode(2);
tree.Right = new TreeNode(3);
tree.Left.Right = new(5);
tree.Right.Right = new(4);

Console.WriteLine($"Right Side View: [{string.Join(", ", new Solution().GetRightSideView(tree))}]");
Console.WriteLine($"Right Side View Array-Based: [{string.Join(", ", new Solution().GetRightSideViewArrayBased(tree))}]");
class Solution
{
    public List<int> GetRightSideView(TreeNode root)
    {
        var result = new List<int>();
        if (root == null)
            return result;

        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;

            for (int i = 0; i < levelSize; i++)
            {
                var node = queue.Dequeue();
                if (i == levelSize - 1)
                {
                    result.Add(node.Value);
                }

                if (node.Left != null)
                    queue.Enqueue(node.Left);

                if (node.Right != null)
                    queue.Enqueue(node.Right);
            }
        }
        return result;
    }

    public List<int> GetRightSideViewArrayBased(TreeNode root)
    {
        var result = new List<int>();
        if (root == null)
            return result;

        var currentLevel = new List<TreeNode> { root };
        var nextLevel = new List<TreeNode>();

        while (currentLevel.Count > 0)
        {
            result.Add(currentLevel.Last().Value);
            foreach (var node in currentLevel)
            {
                if (node.Left != null)
                    nextLevel.Add(node.Left);

                if (node.Right != null)
                    nextLevel.Add(node.Right);
            }
            (currentLevel, nextLevel) = (nextLevel, currentLevel);
            nextLevel.Clear();
        }
        return result;
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

