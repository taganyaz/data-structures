// See https://aka.ms/new-console-template for more information
// Problem: Given a m x n integer grid image and integers sr, sc, and newColor, write a function to perform a flood fill on the image starting from the pixel image[sr][sc].
// In a 'flood fill', start by changing the color of image[sr][sc] to newColor. Then, change the color of all pixels connected to image[sr][sc] from either the top, bottom, left or right that have the same color as image[sr][sc], along with all the connected pixels of those pixels, and so on.
// Example:
// Input: image = [[1,0,1],[1,0,0],[0,0,1]], sr = 1, sc = 1, color = 2
// Output: [[1,2,1],[1,2,2],[2,2,1]]

// Approach: Graph traversal using DFS (Depth-First Search)
// 
// Problem Analysis:
// - This is a connected component problem in a 2D grid
// - We need to find all cells connected to (sr, sc) with the same color
// - Connection is defined as 4-directional (no diagonals)
//
// Algorithm Design:
// 1. Validate input bounds and handle edge case where original color equals new color
// 2. Store the original color at starting position
// 3. Use DFS to traverse all connected cells with matching color
// 4. For each valid cell, change its color and explore its 4 neighbors
//
// Implementation Choices:
// - Recursive DFS: Simple, readable, but risks stack overflow for large grids
// - Iterative DFS with Stack: Safer for larger inputs, explicit memory control
// - Both approaches have O(m*n) time and space complexity in worst case
//
// Trade-offs:
// - DFS vs BFS: DFS uses less memory in average case, BFS would fill level-by-level
// - In-place modification vs creating new array: Chose in-place for memory efficiency
// - Stack-based iteration avoids call stack limitations (~1MB default in .NET)

int newColor = 2;
int sr = 1, sc = 1;
int[,] graph = new int[,] {
    {1, 0, 1},
    {1, 0 , 0},
    { 0, 0, 1}
};

PrintGraph(graph);
Console.WriteLine();
new Solution().FloodFillIterative(graph, sr, sc, newColor);
PrintGraph(graph);

static void PrintGraph(int[,] graph)
{
    int rows = graph.GetLength(0);
    int cols = graph.GetLength(1);

    for (int r = 0; r < rows; r++)
    {
        for (int c = 0; c < cols; c++)
        {
            Console.Write(graph[r, c] + " ");
        }
        Console.WriteLine();
    }
}
class Solution
{
    public void FloodFill(int[,] graph, int sr, int sc, int newColor)
    {
        int rows = graph.GetLength(0);
        int cols = graph.GetLength(1);

        if (sr < 0 || sr >= rows || sc < 0 || sc >= cols)
            return;

        int originalColor = graph[sr, sc];

        if (originalColor == newColor)
            return;

        changeColor(graph, rows, cols, (sr, sc), originalColor, newColor);
    }

    public void FloodFillIterative(int[,] graph, int sr, int sc, int newColor)
    {
        int rows = graph.GetLength(0);
        int cols = graph.GetLength(1);

        if (sr < 0 || sr >= rows || sc < 0 || sc >= cols)
            return;

        int originalColor = graph[sr, sc];

        if (originalColor == newColor)
            return;

        changeColorIterative(graph, rows, cols, (sr, sc), originalColor, newColor);
    }

    private void changeColor(int[,] graph, int rows, int columns, (int row, int col) point, int originalColor, int newColor)
    {
        var (row, col) = point;

        if (row < 0 || row >= rows || col < 0 || col >= columns)
            return;

        if (graph[row, col] != originalColor)
            return;

        graph[row, col] = newColor;

        changeColor(graph, rows, columns, (row - 1, col), originalColor, newColor);

        changeColor(graph, rows, columns, (row + 1, col), originalColor, newColor);

        changeColor(graph, rows, columns, (row, col - 1), originalColor, newColor);

        changeColor(graph, rows, columns, (row, col + 1), originalColor, newColor);
    }

    private void changeColorIterative(int[,] graph, int rows, int columns, (int row, int col) point, int originalColor, int newColor)
    {
        var (row, col) = point;
        var stack = new Stack<(int, int)>();
        stack.Push((row, col));

        while (stack.Count > 0)
        {
            var (currentRow, currentCol) = stack.Pop();
            if (currentRow < 0 || currentRow >= rows
                || currentCol < 0 || currentCol >= columns)
            {
                continue;
            }

            if (graph[currentRow, currentCol] != originalColor)
                continue;

            graph[currentRow, currentCol] = newColor;

            stack.Push((currentRow - 1, currentCol));
            stack.Push((currentRow + 1, currentCol));

            stack.Push((currentRow, currentCol - 1));
            stack.Push((currentRow, currentCol + 1));
        }
    }
}

