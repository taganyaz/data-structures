// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information

// Problem: Count Good Nodes in Binary Tree
// Given the root of a binary tree, find the number of "good nodes" in the tree. 
// A node X is considered "good" if in the path from root to X, there are no nodes 
// with a value greater than X's value.

// Example:
//        5
//       / \
//     10   4
//     / \  / \
//    8  9 7  12
// Good nodes: 5, 10, 7, 12 (count = 4)

// Approach: Depth-First Search (DFS) with Max Tracking
// 
// Key Insight: A node is "good" if its value >= maximum value seen from root to this node.
// We need to track the maximum value encountered along each root-to-node path.
//
// Algorithm:
// 1. Start DFS from root, passing the maximum value seen so far (initially root's value)
// 2. At each node:
//    - If node.value >= currentMax, it's a good node (increment count)
//    - Update currentMax to max(currentMax, node.value) for child traversals
//    - Recursively count good nodes in left and right subtrees
// 3. Return total count
//
// Time Complexity: O(n) - visit each node exactly once
// Space Complexity: O(h) - recursion stack depth, where h is tree height
//                    - Best case (balanced): O(log n)
//                    - Worst case (skewed): O(n)
//
// Edge Cases Handled:
// - Empty tree (null root) → return 0
// - Single node tree → always returns 1 (root is always good)
// - Nodes with equal values → considered good (using >= comparison)
//
// Alternative Approaches:
// - Iterative DFS using stack (avoids recursion stack overflow for deep trees)
// - BFS (less intuitive as we need to track max value per path)

// ...existing code...

var tree = new TreeNode(5);
tree.Left = new TreeNode(10);
tree.Left.Left = new TreeNode(8);
tree.Left.Right = new TreeNode(9);

tree.Right = new TreeNode(4);
tree.Right.Left = new TreeNode(7);
tree.Right.Right = new TreeNode(12);

int goodNodeCount = new Solution().CountGoodNodes(tree);
int goodNodeCountIterative = new Solution().CountGoodNodesIterative(tree);

Console.WriteLine("Good nodes count: " + goodNodeCount);
Console.WriteLine("Good nodes count Iter: " + goodNodeCountIterative);
class Solution
{
    public int CountGoodNodes(TreeNode root)
    {
        if (root == null)
            return 0;

        return CountGoodNodes(root, root.Value);
    }

    private int CountGoodNodes(TreeNode root, int currentMax)
    {
        if (root == null)
            return 0;

        int count = 0;
        if (root.Value >= currentMax)
        {
            count += 1;
            currentMax = root.Value;

        }

        return count + CountGoodNodes(root.Left, currentMax) + CountGoodNodes(root.Right, currentMax);

    }

    public int CountGoodNodesIterative(TreeNode root)
    {
        if (root == null)
            return 0;

        int goodNodeCount = 0;
        var stack = new Stack<(TreeNode node, int maxSoFar)>();

        stack.Push((root, root.Value));

        while (stack.Count > 0)
        {
            (TreeNode currentNode, int maxSoFar) = stack.Pop();

            if (currentNode.Value >= maxSoFar)
            {
                goodNodeCount++;
                maxSoFar = currentNode.Value;
            }

            if (currentNode.Left != null)
            {
                stack.Push((currentNode.Left, maxSoFar));
            }

            if (currentNode.Right != null)
            {
                stack.Push((currentNode.Right, maxSoFar));
            }
        }
        return goodNodeCount;
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