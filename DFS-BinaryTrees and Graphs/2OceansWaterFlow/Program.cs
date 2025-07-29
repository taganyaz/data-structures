// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
// Problem: There is an m x n rectangular island that borders both the Pacific Ocean and Atlantic Ocean. The Pacific Ocean touches the island's left and top edges, and the Atlantic Ocean touches the island's right and bottom edges.
// The island is partitioned into a grid of square cells. You are given an m x n integer matrix heights where heights[r][c] represents the height above sea level of the cell at coordinate (r, c).
// The island receives a lot of rain, and the rain water can flow to neighboring cells directly north, south, east, and west if the neighboring cell's height is less than or equal to the current cell's height. Water can flow from any cell adjacent to an ocean into the ocean.
// Return a 2D list of grid coordinates result where result[i] = [ri, ci] denotes that rain water can flow from cell (ri, ci) to both the Pacific and Atlantic oceans.

// Example:
// Input: heights = [[1,2,2,3,5],[3,2,3,4,4],[2,4,5,3,1],[6,7,1,4,5],[5,1,1,2,4]]
// Output: [[0,4],[1,3],[1,4],[2,2],[3,0],[3,1],[4,0]]

// Approach: Reverse Flow DFS from Ocean Boundaries
// 
// Key Insight: Instead of checking from each cell if water can reach both oceans (expensive),
// we start from the ocean boundaries and find all cells that can flow TO each ocean.
// 
// Algorithm:
// 1. Start DFS from Pacific Ocean boundaries (left column and top row)
//    - Mark all cells reachable by moving to higher or equal elevation cells
// 2. Start DFS from Atlantic Ocean boundaries (right column and bottom row)  
//    - Mark all cells reachable by moving to higher or equal elevation cells
// 3. The intersection of both sets gives cells that can flow to both oceans
//
// Why Reverse Flow?
// - Water flows from higher to lower/equal cells naturally
// - When traversing backwards from ocean, we move from lower to higher/equal cells
// - This avoids checking every cell's path to both oceans (O(m*n) vs O((m*n)²))
//
// Time Complexity: O(m*n) - each cell visited at most twice (once per ocean)
// Space Complexity: O(m*n) - for the two HashSets storing reachable cells

var heights = new int[,] { { 1, 2, 2, 3, 5 }, { 3, 2, 3, 4, 4 }, { 2, 4, 5, 3, 1 }, { 6, 7, 1, 4, 5 }, { 5, 1, 1, 2, 4 } };
var flows = new Solution().GetOceansFlow(heights);

foreach (var (row, col) in flows)
{
    Console.Write($"[{row},{col}] ");
}
Console.WriteLine();
class Solution
{
    public HashSet<(int, int)> GetOceansFlow(int[,] graph)
    {
        int rows = graph.GetLength(0);
        int columns = graph.GetLength(1);
        HashSet<(int, int)> pacificFlows = new HashSet<(int, int)>();
        HashSet<(int, int)> atlanticFlows = new HashSet<(int, int)>();

        for (int r = 0; r < rows; r++)
        {
            FindConnected(graph, rows, columns, (r, 0), pacificFlows);
            FindConnected(graph, rows, columns, (r, columns - 1), atlanticFlows);
        }

        for (int c = 0; c < columns; c++)
        {
            FindConnected(graph, rows, columns, (0, c), pacificFlows);
            FindConnected(graph, rows, columns, (rows - 1, c), atlanticFlows);
        }

        var flows = pacificFlows.Intersect(atlanticFlows).ToHashSet();

        //pacificFlows.IntersectWith(atlanticFlows);
        //return pacificFlows;
        return flows;
    }

    private void FindConnected(int[,] graph, int rows, int columns, (int row, int col) point, HashSet<(int row, int col)> oceanFlows)
    {
        var (r, c) = point;
        if (r < 0 || r >= rows || c < 0 || c >= columns || oceanFlows.Contains(point))
            return;

        oceanFlows.Add(point);

        if (r > 0 && graph[r, c] <= graph[r - 1, c])
            FindConnected(graph, rows, columns, (r - 1, c), oceanFlows);

        if (r < rows - 1 && graph[r, c] <= graph[r + 1, c])
            FindConnected(graph, rows, columns, (r + 1, c), oceanFlows);

        if (c > 0 && graph[r, c] <= graph[r, c - 1])
            FindConnected(graph, rows, columns, (r, c - 1), oceanFlows);

        if (c < columns - 1 && graph[r, c] <= graph[r, c + 1])
            FindConnected(graph, rows, columns, (r, c + 1), oceanFlows);

        
    }
}

