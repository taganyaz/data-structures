// See https://aka.ms/new-console-template for more information
// Approach: Multi-Source BFS (Breadth-First Search)
// 
// Key Insight: Instead of running BFS from each '1' to find nearest '0' (O(m*n) BFS operations),
// we run a single BFS starting from ALL '0's simultaneously. This guarantees that when we reach
// any cell, we've found the shortest path from that cell to any '0'.
//
// Algorithm:
// 1. Edge Case Handling: 
//    - Return empty array if matrix is null or has zero dimensions
//
// 2. Initialization:
//    - Create output matrix with same dimensions as input
//    - Create queue for BFS traversal
//    - Define 4-directional movement deltas (up, down, left, right)
//
// 3. Multi-Source Setup:
//    - Iterate through matrix to identify all cells containing '0'
//    - For each '0' cell: mark distance as 0 and add to queue (these are our BFS sources)
//    - For each '1' cell: mark as -1 (unvisited flag)
//
// 4. BFS Traversal:
//    - Process queue level by level (important for distance calculation)
//    - For each cell in current level:
//      a) Dequeue cell coordinates
//      b) Check all 4 adjacent neighbors
//      c) For each valid, unvisited neighbor:
//         - Calculate distance as current_cell_distance + 1
//         - Mark as visited by updating distance
//         - Enqueue for next level processing
//
// 5. Return the populated output matrix
//
// Time Complexity: O(m * n) - each cell is visited at most once
// Space Complexity: O(m * n) - for the output matrix and queue (worst case)


var mat = new int[,] {
  {1, 0, 1},
  {0, 1, 0},
  {1, 1, 1},
};

var output = new Solution().GetDistancesToZero(mat);
printMatrix(output);

static void printMatrix(int[,] matrix)
{


    Console.WriteLine("[");
    for (int r = 0; r < matrix.GetLength(0); r++)
    {
        Console.Write("[");
        for (int c = 0; c < matrix.GetLength(1); c++)
        {
            Console.Write(matrix[r, c] + $"{(c == matrix.GetLongLength(1) - 1 ? "" : ", ")}");
        }
        Console.Write("], \n");
    }
    Console.WriteLine("]");
}

class Solution
{
    public int[,] GetDistancesToZero(int[,] matrix)
    {
        if (matrix == null || matrix.GetLength(0) == 0 || matrix.GetLength(1) == 0)
            return new int[,] { };

        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);
        int[,] output = new int[rows, columns];
        var queue = new Queue<(int row, int col)>();
        var deltas = new List<(int row, int col)> { (-1, 0), (1, 0), (0, -1), (0, 1) };

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (matrix[r, c] == 0)
                {
                    output[r, c] = 0;
                    queue.Enqueue((r, c));
                }
                else
                {
                    output[r, c] = -1;
                }
            }
        }

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;

            for (int i = 0; i < levelSize; i++)
            {
                var (r, c) = queue.Dequeue();

                foreach (var (dr, dc) in deltas)
                {
                    var (nr, nc) = (r + dr, c + dc);

                    if (nr < 0 || nr > rows - 1 || nc < 0 || nc > columns - 1 || output[nr, nc] != -1)
                        continue;

                    output[nr, nc] = output[r, c] + 1;
                    queue.Enqueue((nr, nc));

                }
            }
        }

        return output;

    }
}



