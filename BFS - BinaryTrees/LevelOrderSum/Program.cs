// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information

// Problem: Level Order Sum of Binary Tree
// Given the root of a binary tree, return the sum of node values at each level.
// The output should be a list where each element represents the sum of all nodes at that depth.
//
// Example:
// Input Tree:
//        1
//       / \
//      3   4
//       \ /
//        2 7
//       /
//      8
// Output: [1, 7, 9, 8]
// Explanation: 
//   - Level 0: 1
//   - Level 1: 3 + 4 = 7
//   - Level 2: 2 + 7 = 9
//   - Level 3: 8

// Approach: Breadth-First Search (BFS) using Queue
// 
// Why BFS?
// - Natural fit for level-order traversal as it processes nodes level by level
// - Guarantees we visit all nodes at depth d before visiting nodes at depth d+1
// - Time Complexity: O(n) where n is the number of nodes
// - Space Complexity: O(w) where w is the maximum width of the tree
//
// Algorithm:
// 1. Handle edge case: if root is null, return empty list
// 2. Initialize a queue with the root node
// 3. While queue is not empty:
//    a. Record current level size (number of nodes in current level)
//    b. Initialize sum accumulator for current level
//    c. Process exactly 'levelSize' nodes:
//       - Dequeue node from front
//       - Add node's value to level sum
//       - Enqueue non-null children (left, then right)
//    d. Add completed level sum to result list
// 4. Return the list of level sums
//
// Key Insight: By processing exactly 'levelSize' nodes in each iteration,
// we ensure we're only summing nodes from the same level, even though
// the queue contains nodes from multiple levels.


var tree = new TreeNode(1);
tree.Left = new TreeNode(3);
tree.Right = new TreeNode(4);
tree.Left.Right = new TreeNode(2);
tree.Right.Left = new TreeNode(7);
tree.Left.Right.Left = new TreeNode(8);

Console.WriteLine($"Level sum:[{string.Join(", ", new Solution().GetLevelsSum(tree))}]");
Console.WriteLine($"Level sum Arr:[{string.Join(", ", new Solution().GetLevelSumArrayBasedQueue(tree))}]");
class Solution
{
    public List<int> GetLevelsSum(TreeNode root)
    {
        var result = new List<int>();
        if (root == null)
            return result;

        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;
            int levelSum = 0;

            for (int i = 0; i < levelSize; i++)
            {
                var node = queue.Dequeue();
                levelSum += node.Value;

                if (node.Left != null)
                {
                    queue.Enqueue(node.Left);
                }
                if (node.Right != null)
                {
                    queue.Enqueue(node.Right);
                }
            }
            result.Add(levelSum);
        }
        return result;
    }

    public List<int> GetLevelSumArrayBasedQueue(TreeNode tree)
    {
        var reslt = new List<int>();
        if (tree == null)
            return reslt;

        var currentLevel = new List<TreeNode>() { tree };
        var nextLevel = new List<TreeNode>();

        while (currentLevel.Count > 0)
        {
            int levelSum = 0;
            foreach (var node in currentLevel)
            {
                levelSum += node.Value;

                if (node.Left != null)
                    nextLevel.Add(node.Left);

                if (node.Right != null)
                    nextLevel.Add(node.Right);
            }

            reslt.Add(levelSum);
            (currentLevel, nextLevel) = (nextLevel, currentLevel);
            nextLevel.Clear();
        }

        return reslt;
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
