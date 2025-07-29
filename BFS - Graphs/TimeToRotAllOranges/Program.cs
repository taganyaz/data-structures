// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
// Problem: Minimum Time to Rot All Oranges
// You are given an m x n grid where each cell can have one of three values:
// 0 representing an empty cell,
// 1 representing a fresh orange, or
// 2 representing a rotten orange.
// Every minute, any fresh orange that is 4-directionally adjacent to a rotten orange becomes rotten.
// Return the minimum number of minutes that must elapse until no cell has a fresh orange. If this is impossible, return -1.
// Example 1:
// Input: grid = [[2,1,1],[1,1,0],[0,1,1]]
// Output: 4
// Example 2:
// Input: grid = [[2,1,1],[0,1,1],[1,0,1]]
// Output: -1

// Solution Approach: Multi-source BFS (Breadth-First Search)
// 
// Key Insights:
// 1. This is a multi-source shortest path problem where all rotten oranges spread simultaneously
// 2. BFS guarantees we explore all oranges at distance k before exploring those at distance k+1
// 3. Level-order traversal naturally models the minute-by-minute progression
//
// Time Complexity: O(m × n) where m and n are grid dimensions
// - We visit each cell at most once
// - Initial scan: O(m × n), BFS traversal: O(m × n)
//
// Space Complexity: O(m × n) 
// - In worst case, all oranges are rotten initially and queue holds O(m × n) elements
//
// Algorithm:
// 1. Input Validation:
//    - Handle null/empty grid edge cases
//    - Return -1 for invalid inputs
//
// 2. Initialization Phase:
//    - Scan entire grid once to:
//      a) Count all fresh oranges (track completion condition)
//      b) Enqueue all initially rotten oranges (multi-source BFS)
//    - Early exit if no fresh oranges exist (return 0)
//
// 3. BFS Propagation:
//    - Process oranges level by level (all oranges that rot in the same minute)
//    - For each level:
//      a) Track level size to ensure synchronous processing
//      b) For each rotten orange, check all 4-directional neighbors
//      c) Convert fresh neighbors to rotten and enqueue them
//      d) Track if any fresh orange was converted in this iteration
//    - Increment minute counter only if rotting occurred
//
// 4. Result Evaluation:
//    - If fresh oranges remain after BFS completes, return -1 (impossible case)
//    - Otherwise, return the minute count
//
// Design Decisions:
// - Using tuple (row, col) for clean coordinate handling
// - In-place modification of grid to avoid extra visited array
// - Early termination when freshCount reaches 0 for optimization

var matrix1 = new int[,] { { 2, 1, 1 }, { 1, 1, 0 }, { 0, 1, 1 } };
var matrix2 = new int[,] { { 2, 1, 1 }, { 0, 1, 1 }, { 1, 0, 1 } };
var sol = new Solution();
Console.WriteLine("Minutes count matrix 1:" + sol.CountMinutesToReachAllFreshCells(matrix1));
Console.WriteLine("Minutes count matrix 2:" + sol.CountMinutesToReachAllFreshCells(matrix2));
class Solution
{
    public int CountMinutesToReachAllFreshCells(int[,] matrix)
    {
        if (matrix == null)
            return -1;

        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);

        int freshCount = 0;
        int minutesCount = 0;
        var queue = new Queue<(int row, int col)>();
        var deltas = new List<(int row, int col)> { (-1, 0), (1, 0), (0, -1), (0, 1) };

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (matrix[r, c] == 1)
                {
                    freshCount++;
                }
                else if (matrix[r, c] == 2)
                {
                    queue.Enqueue((r, c));
                }
            }
        }

        if (freshCount == 0)
            return 0;

        while (queue.Count > 0 && freshCount > 0)
        {
            int levelSize = queue.Count;
            bool hasRottenAnyFresh = false;

            for (int i = 0; i < levelSize; i++)
            {
                var (r, c) = queue.Dequeue();

                foreach (var (dr, dc) in deltas)
                {
                    var (nr, nc) = (r + dr, c + dc);

                    if (nr < 0 || nr > rows - 1 || nc < 0 || nc > columns - 1 || matrix[nr, nc] != 1)
                        continue;

                    freshCount--;
                    hasRottenAnyFresh = true;
                    matrix[nr, nc] = 2;
                    queue.Enqueue((nr, nc));
                }
            }
            if (hasRottenAnyFresh)
                minutesCount++;
        }

        return freshCount > 0 ? -1 : minutesCount;
    }
}

