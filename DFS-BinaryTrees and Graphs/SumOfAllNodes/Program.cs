// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
var tree = new TreeNode(10);
tree.Left = new TreeNode(12);
tree.Left.Left = new TreeNode(5);
tree.Left.Right = new TreeNode(6);

tree.Right = new TreeNode(15);

var nodeSum = new Solution().SumOfNodes(tree);
Console.WriteLine("Sum of nodes: " + nodeSum);
class Solution
{
    public int SumOfNodes(TreeNode root)
    {
        if (root == null)
            return 0;

        if (root.Left == null && root.Right == null)
            return root.Value;

        return root.Value + SumOfNodes(root.Left) + SumOfNodes(root.Right);
    }
}
class TreeNode
{
    public TreeNode(int value)
    {
        Value = value;
    }
    public int Value { get; private set; }
    public TreeNode Left { get; set; } = null!;
    public TreeNode Right { get; set; } = null!;
}

