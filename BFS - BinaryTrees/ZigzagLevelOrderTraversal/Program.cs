// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
// Problem: Given the root of a binary tree, return the zigzag level order traversal of its nodes' values. (i.e., from left to right, then right to left for the next level and alternate between).
// Example 1:
// Input: root = [3,9,20,null,null,15,7]
// Output: [[3],[20,9],[15,7]]

// Example 2:
// Input: root = [1]
// Output: [[1]]

// Approach 1: BFS with Two Lists (Level Swapping)
// Key Insight: Maintain two lists to separate current level processing from next level collection
// Algorithm:
// 1. Handle edge case: return empty list for null root
// 2. Initialize data structures:
//    - currentLevel: nodes to process in current iteration
//    - nextLevel: collect children for next iteration
//    - leftToRight: direction flag (starts true for root level)
// 3. Process levels iteratively:
//    a. Extract values from currentLevel nodes
//    b. Apply zigzag pattern: reverse if right-to-left direction
//    c. Collect all children in nextLevel (always left-to-right order)
//    d. Swap lists and toggle direction for next iteration
// Time Complexity: O(n) - visit each node once
// Space Complexity: O(w) where w is max width of tree (worst case n/2 for complete tree)
// Pros: Clear separation of concerns, easy to understand
// Cons: Additional O(k) time for reversing levels where k is level size

// Approach 2: BFS with Queue and LinkedList (Optimized)
// Key Insight: Use LinkedList's bidirectional insertion to avoid explicit reversal
// Algorithm:
// 1. Use standard BFS queue pattern with level size tracking
// 2. For each level:
//    - Track levelSize before processing (queue will grow during iteration)
//    - Use LinkedList for O(1) insertion at both ends
//    - Insert at tail for left-to-right, at head for right-to-left
// 3. Convert LinkedList to List for result (required by problem signature)
// Time Complexity: O(n) - visit each node once, no reversal needed
// Space Complexity: O(w) for queue + O(k) for LinkedList per level
// Pros: No explicit reversal, more efficient for large levels
// Cons: LinkedList to List conversion still O(k), slightly more complex logic

// Trade-off Analysis:
// - Approach 1: Better for teaching/interviews (clearer intent)
// - Approach 2: Better for production (avoids unnecessary operations)
// - Both approaches maintain children in natural left-to-right order in queue/list

var tree = new TreeNode(3);
tree.Left = new TreeNode(9);
tree.Right = new TreeNode(20);
tree.Right.Left = new TreeNode(15);
tree.Right.Right = new TreeNode(7);

var result = new Solution().ZigzagLevelOrder(tree);
var result2 = new Solution().ZigzagLevelOrderWithQueueAndLinkedList(tree);
Console.WriteLine($"[{string.Join(",", result.Select(l => $"[{string.Join(",", l)}]"))}]");
Console.WriteLine($"[{string.Join(",", result2.Select(l => $"[{string.Join(",", l)}]"))}]");
class Solution
{
    public List<List<int>> ZigzagLevelOrder(TreeNode root)
    {
        var result = new List<List<int>>();
        if (root == null)
            return result;

        bool leftToRight = true;
        var currentLevel = new List<TreeNode> { root };
        var nextLevel = new List<TreeNode>();

        while (currentLevel.Count > 0)
        {
            var levelItems = currentLevel.Select(x => x.Value).ToList();
            if (!leftToRight)
            {
                levelItems.Reverse();
            }
            result.Add(levelItems);

            foreach (var node in currentLevel)
            {
                if (node.Left != null)
                    nextLevel.Add(node.Left);

                if (node.Right != null)
                    nextLevel.Add(node.Right);
            }

            leftToRight = !leftToRight;
            (currentLevel, nextLevel) = (nextLevel, currentLevel);
            nextLevel.Clear();
        }
        return result;
    }

    public List<List<int>> ZigzagLevelOrderWithQueueAndLinkedList(TreeNode root)
    {
        var result = new List<List<int>>();
        if (root == null)
            return result;

        bool leftToRight = true;
        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;
            var level = new LinkedList<int>();

            for (int i = 0; i < levelSize; i++)
            {
                var node = queue.Dequeue();
                if (leftToRight)
                    level.AddLast(node.Value);
                else
                    level.AddFirst(node.Value);

                if (node.Left != null)
                    queue.Enqueue(node.Left);

                if (node.Right != null)
                    queue.Enqueue(node.Right);
            }
            leftToRight = !leftToRight;
            result.Add(level.ToList());
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

