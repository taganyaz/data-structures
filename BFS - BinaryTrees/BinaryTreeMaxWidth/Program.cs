// See https://aka.ms/new-console-template for more information
// ...existing code...
// Problem: Given the root of a binary tree, return the maximum width of the given tree.
// The maximum width of a tree is the maximum width among all levels.
// The width of one level is defined as the length between the end-nodes (the leftmost and rightmost non-null nodes), where the null nodes between the end-nodes that would be present in a complete binary tree extending down to that level are also counted into the length calculation.
// It is guaranteed that the answer will in the range of a 32-bit signed integer.
// Example 1:
// Input: root = [1,3,2,5,3,null,9]
// Output: 4
// Example 2:
// Input: root = [1,3,2,5,null,null,9,6,null,7]
// Output: 7

// Approach: BFS with Position Indexing
// 
// Key Insight: 
// In a complete binary tree, nodes can be indexed using heap-style numbering:
// - Root has index 0
// - For any node at index i: left child = 2*i, right child = 2*i + 1
// - Width at any level = rightmost_index - leftmost_index + 1
//
// Why BFS?
// - Natural level-by-level processing
// - Can track all nodes at current level simultaneously
// - Space-efficient for wide trees (only stores one level at a time)
//
// Algorithm:
// 1. Edge case: Return 0 for null root
// 2. Initialize BFS queue with (root, position=0)
// 3. For each level:
//    a. Record level size (number of nodes to process)
//    b. Normalize positions to prevent integer overflow:
//       - Find minimum position in current level
//       - Subtract from all positions (maintains relative distances)
//    c. Track leftmost (first) and rightmost (last) positions
//    d. Enqueue children with calculated positions
//    e. Calculate width = rightmost - leftmost + 1
// 4. Return maximum width encountered
//
// Overflow Prevention:
// Without normalization, positions grow exponentially (2^depth).
// By resetting the leftmost position to 0 each level, we ensure
// positions stay within reasonable bounds while preserving relative distances.
//
// Complexity Analysis:
// - Time: O(n) - visit each node exactly once
// - Space: O(w) where w is max width - queue holds at most one level
//
// Edge Cases Handled:
// - Empty tree (null root)
// - Single node tree
// - Skewed trees (all nodes on one side)
// - Trees with gaps between nodes at same level

var tree = new TreeNode(1);
tree.Left = new TreeNode(3);
tree.Right = new TreeNode(2);
tree.Left.Left = new TreeNode(5);
tree.Left.Right = new TreeNode(3);
tree.Right.Right = new TreeNode(9);

Console.WriteLine("Max Width:" + new Solution().GetMaxWidth(tree));
class Solution
{
    public int GetMaxWidth(TreeNode root)
    {
        if (root == null)
            return 0;

        int maxWidth = 0;
        var queue = new Queue<(TreeNode node, int pos)>();
        queue.Enqueue((root, 0));

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;
            int leftPosition = 0;
            int rightPosition = 0;
            int minPos = queue.Peek().pos; // Normalize position to prevent overflow

            for (int i = 0; i < levelSize; i++)
            {
                var (node, pos) = queue.Dequeue();
                pos -= minPos; // Normalize position
                if (i == 0)
                    leftPosition = pos;
                if (i == levelSize - 1)
                    rightPosition = pos;

                if (node.Left != null)
                    queue.Enqueue((node.Left, 2 * pos));

                if (node.Right != null)
                    queue.Enqueue((node.Right, 2 * pos + 1));
            }

            int currentWidth = rightPosition - leftPosition + 1;
            maxWidth = Math.Max(maxWidth, currentWidth);
        }
        return maxWidth;
    }
}
class TreeNode
{
    public TreeNode(int value)
    {
        Value = value;
    }
    public int Value { get; private set; }
    public TreeNode? Left { get; set; } = null;
    public TreeNode? Right { get; set; } = null;
}

