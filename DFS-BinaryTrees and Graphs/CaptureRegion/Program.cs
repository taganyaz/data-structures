// See https://aka.ms/new-console-template for more information
// Problem: Capture Surrounded Regions
// Given an m x n matrix containing 'X' and 'O', capture all regions surrounded by 'X'.
// A region is captured by flipping all 'O's into 'X's in that surrounded region.
// 
// Key Definitions:
// - Connected: Cells are connected horizontally or vertically (not diagonally)
// - Region: A group of connected 'O' cells
// - Surrounded: A region is surrounded if it cannot reach the board's edge
//
// Example:
// Input:                    Output:
// X X X X O                 X X X X O
// X X O X X                 X X X X X  
// X X O X O     ------>     X X X X O
// X O X X X                 X O X X X
// X O X X X                 X O X X X
//
// Explanation: 
// - The 'O' cells at positions (1,2) and (2,2) form a surrounded region and are captured
// - The 'O' cells on the right and bottom edges remain unchanged as they're not surrounded

// Approach: Reverse Thinking - Mark Safe Regions First
// Instead of finding surrounded regions, we identify regions that CANNOT be captured:
// 1. Any 'O' on the boundary cannot be captured
// 2. Any 'O' connected to a boundary 'O' cannot be captured
// 3. All other 'O' cells must be surrounded and should be captured
//
// Algorithm:
// 1. Traverse all boundary cells (first/last row and first/last column)
// 2. For each boundary 'O', perform DFS/BFS to mark all connected 'O' cells as safe
//    - We temporarily mark these cells with 'S' (Safe) to distinguish them
// 3. Traverse the entire grid:
//    - Convert 'S' back to 'O' (these are boundary-connected, safe cells)
//    - Convert remaining 'O' to 'X' (these are surrounded cells to be captured)
//
// Time Complexity: O(m × n) where m = rows, n = columns
// - We visit each cell at most twice: once during DFS and once during final update
// Space Complexity: O(m × n) in worst case for recursion stack
// - Worst case: entire grid is 'O' and connected
// - Average case: O(min(m, n)) for recursion depth along the boundary

var grid = new char[,] {
{'X','X','X','X','O'},
{'X','X','O','X','X'},
{'X','X','O','X','O'},
{'X','O','X','X','X'},
{'X','O','X','X','X'}
};

var graph = (char[,]) grid.Clone();

Console.WriteLine("Before:");
PrintGraph(graph);
new Solution().CaptureRegion(graph);

Console.WriteLine("After:");
PrintGraph(graph);


static void PrintGraph(char[,] graph)
{
    int rows = graph.GetLength(0);
    int columns = graph.GetLength(1);

    for (int r = 0; r < rows; r++)
    {
        for (int c = 0; c < columns; c++)
        {
            Console.Write(graph[r, c] + " ");
        }
        Console.WriteLine();
    }
}
class Solution
{
    public void CaptureRegion(char[,] graph)
    {
        int rows = graph.GetLength(0);
        int columns = graph.GetLength(1);

        // Mark connected 'O' with 'S'
        for (int r = 0; r < rows; r++)
        {
            MarkConnectedRegion(graph, rows, columns, (r, 0));
            MarkConnectedRegion(graph, rows, columns, (r, columns - 1));
        }

        for (int c = 0; c < columns; c++)
        {
            MarkConnectedRegion(graph, rows, columns, (0, c));
            MarkConnectedRegion(graph, rows, columns, (rows - 1, c));
        }

        // Update cell values: 'S' -> 'O', 'O' -> 'X'

        UpdateRegions(graph, rows, columns);

    }

    private void MarkConnectedRegion(char[,] graph, int rows, int columns, (int row, int col) point)
    {
        var (r, c) = point;

        if (r < 0 || r >= rows || c < 0 || c >= columns || graph[r, c] != 'O')
            return;

        graph[r, c] = 'S';

        MarkConnectedRegion(graph, rows, columns, (r - 1, c));
        MarkConnectedRegion(graph, rows, columns, (r + 1, c));
        MarkConnectedRegion(graph, rows, columns, (r, c - 1));
        MarkConnectedRegion(graph, rows, columns, (r, c + 1));
    }

    private void MarkConnectedRegionIterative(char[,] graph, int rows, int columns, (int row, int col) point)
    {
        var (row, col) = point;
        var stack = new Stack<(int row, int col)>();
        stack.Push((row, col));

        while (stack.Count > 0)
        {
            var (r, c) = stack.Pop();
            if (r < 0 || r >= rows || c < 0 || c >= columns || graph[r, c] != Char.ToUpper('O'))
                continue;

            graph[r, c] = 'S';
            stack.Push((r - 1, c));
            stack.Push((r + 1, c));
            stack.Push((r, c - 1));
            stack.Push((r, c + 1));

        }
        
    }

    private void UpdateRegions(char[,] graph, int rows, int columns)
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (graph[r, c] == 'S')
                {
                    graph[r, c] = 'O';
                }
                else if (graph[r, c] == 'O')
                {
                    graph[r, c] = 'X';
                }
            }
        }
    }
}



