// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
// Problem: Count Islands in a Binary Matrix
// Given a binary matrix grid of size m x n, where '1' denotes land and '0' signifies water,
// determine the count of islands present in this grid. An island is defined as a region of 
// contiguous land cells connected either vertically or horizontally, and it is completely 
// encircled by water. Assume that the grid is bordered by water on all sides.
//
// Example:
// Input:
// grid = [
//   [1,1,0,1],
//   [1,1,0,1],
//   [1,1,0,0],
// ]
// Output: 2
//
// Approach: Connected Components via Depth-First Search (DFS)
// 
// Key Insight: This is fundamentally a graph connectivity problem where each cell is a node
// and edges exist between adjacent land cells. Each island represents a connected component.
//
// Algorithm Overview:
// 1. Iterate through each cell in the grid
// 2. When we find unvisited land (value = 1), we've discovered a new island
// 3. Perform DFS/BFS to mark all connected land cells as visited
// 4. Count each distinct DFS traversal as one island
//
// Implementation Details:
// - We mark visited cells in-place by setting them to -1 (saves O(m*n) space)
// - Two implementations provided: recursive and iterative (stack-based)
// - Direction vectors could be used for cleaner neighbor traversal
//
// Time Complexity: O(m * n) where m = rows, n = columns
//   - We visit each cell at most once
// Space Complexity: 
//   - Recursive: O(min(m, n)) for call stack in worst case (entire grid is one island)
//   - Iterative: O(min(m, n)) for explicit stack
//   - Could be O(1) if we don't count the input modification
//
// Trade-offs:
// - Modifying input vs. using visited array (space vs. input preservation)
// - Recursive vs. iterative (stack overflow risk vs. code simplicity)
// - DFS vs. BFS (memory usage patterns)
//
// Edge Cases Handled:
// - Empty grid
// - Single cell grid
// - All water (returns 0)
// - All land (returns 1)
// - Disconnected single-cell islands

// ...existing code...

var grid = new int[,] {
    {1,1,0,1},
    {1,1,0,1},
    {1,1,0,0},
};

//Console.WriteLine("Islands Count:" + new Solution().CountIslands(grid));
Console.WriteLine("Islands Count Iter:" + new Solution().CountIslandsIterative(grid));
class Solution
{
    public int CountIslands(int[,] graph)
    {
        int rows = graph.GetLength(0);
        int columns = graph.GetLength(1);
        int islandCount = 0;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (graph[r, c] == 1)
                {
                    islandCount++;
                    MarkIsland(graph, rows, columns, (r, c));
                }
            }
        }
        return islandCount;
    }

    private void MarkIsland(int[,] graph, int rows, int columns, (int, int) point)
    {
        var (row, col) = point;
        if (row < 0 || row >= rows || col < 0 || col >= columns)
            return;

        if (graph[row, col] != 1)
            return;

        graph[row, col] = -1;

        MarkIsland(graph, rows, columns, (row - 1, col));
        MarkIsland(graph, rows, columns, (row + 1, col));

        MarkIsland(graph, rows, columns, (row, col - 1));
        MarkIsland(graph, rows, columns, (row, col + 1));
    }

public int CountIslandsIterative(int[,] graph)
    {
        int rows = graph.GetLength(0);
        int columns = graph.GetLength(1);
        int islandCount = 0;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (graph[r, c] == 1)
                {
                    islandCount++;
                    MarkIslandIterative(graph, rows, columns, (r, c));
                }
            }
        }
        return islandCount;
    }

    private void MarkIslandIterative(int[,] graph, int rows, int columns, (int, int) point)
    {
        var (row, col) = point;
        var stack = new Stack<(int row, int col)>();
        stack.Push((row, col));

        while (stack.Count > 0)
        {
            var (r, c) = stack.Pop();
            if (r < 0 || r >= rows || c < 0 || c >= columns || graph[r, c] != 1)
                continue;

            graph[r, c] = -1;

            stack.Push((r - 1, c));
            stack.Push((r + 1, c));
            stack.Push((r, c - 1));
            stack.Push((r, c + 1));
        }
    }
}

