// See https://aka.ms/new-console-template for more information
// Problem: Given the root of the binary tree, find the longest path where all nodes along the path have the same value. This path doesn't have to include the root node. Return the number of edges on that path, not the number of nodes.

// Approach: Recursive DFS with max path length tracking
// Alogorith:
// Traverse the tree from root to leaves
// Maintain the max path length found so far
// At each node
// Calculate  path lenghths in left and right subtrees
// If current value is same as left and right subtree, update the maxPath to max(maxPath, left + right)
// Return current paht leght to path = max(left-arrow, right-arrow)

var tree = new TreeNode(5);
tree.Left = new TreeNode(4);
tree.Left.Left = new TreeNode(4);
tree.Left.Right = new TreeNode(4);
tree.Left.Right.Left = new TreeNode(6);

tree.Right = new TreeNode(5);
tree.Right.Right = new TreeNode(5);
tree.Right.Right.Right = new TreeNode(5);

Console.WriteLine("Longest Path: " + new Solution().FindLongestPathWithSameValue(tree));

Console.WriteLine("Longest Path Iter: " + new Solution().FindLongestPathWithSameValueIterative(tree));
class Solution
{
    public int FindLongestPathWithSameValue(TreeNode root)
    {
        if (root == null)
            return 0;

        int maxPath = 0;

        FindLongestPathWithSameValue(root, ref maxPath);

        return maxPath;
    }

    private int FindLongestPathWithSameValue(TreeNode? root, ref int maxPath)
    {
        if (root == null)
            return 0;

        int leftPath = FindLongestPathWithSameValue(root.Left, ref maxPath);
        int rightPath = FindLongestPathWithSameValue(root.Right, ref maxPath);

        int leftArrow = 0;
        int rightArrow = 0;

        if (root.Left != null && root.Value == root.Left.Value)
        {
            leftArrow = leftPath + 1;
        }

        if (root.Right != null && root.Value == root.Right.Value)
        {
            rightArrow = rightPath + 1;
        }

        maxPath = Math.Max(maxPath, leftArrow + rightArrow);

        return Math.Max(leftArrow, rightArrow);
    }

    public int FindLongestPathWithSameValueIterative(TreeNode root)
    {
        if (root == null)
            return 0;

        int maxPath = 0;
        var stack = new Stack<(TreeNode node, bool visited)>();
        var pathLengths = new Dictionary<TreeNode, int>();

        stack.Push((root, false));

        while (stack.Count > 0)
        {
            var (currentNode, visited) = stack.Pop();

            if (visited)
            {
                int leftArrow = 0;
                int rightArrow = 0;

                if (currentNode.Left != null && currentNode.Value == currentNode.Left.Value && pathLengths.ContainsKey(currentNode.Left))
                {
                    leftArrow = 1 + pathLengths[currentNode.Left];
                }

                if (currentNode.Right != null && currentNode.Value == currentNode.Right.Value && pathLengths.ContainsKey(currentNode.Right))
                {
                    rightArrow = 1 + pathLengths[currentNode.Right];
                }

                maxPath = Math.Max(maxPath, leftArrow + rightArrow);

                pathLengths[currentNode] = Math.Max(leftArrow, rightArrow);
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

        return maxPath;
    }
}
public class TreeNode
{
    public TreeNode(int value) => Value = value;
    public int Value { get; private set; }
    public TreeNode? Left { get; set; } = null;
    public TreeNode? Right { get; set; } = null;
}
