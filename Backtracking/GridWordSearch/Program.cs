// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information

// Problem: Word Search in 2D Grid
// Given an m x n grid of characters board and a string word, return true if word exists in the grid.
// The word can be constructed from letters of sequentially adjacent cells, where adjacent cells are 
// horizontally or vertically neighboring. The same letter cell may not be used more than once.

// Example 1:
// Input: board = [['B','L','C','H'],['D','E','L','T'],['D','A','K','A']], word = "BLEAK"
// Output: true
// Explanation: B(0,0) → L(0,1) → E(1,1) → A(2,1) → K(2,2)

// Example 2:
// Input: board = [['B','L','C','H'],['D','E','L','T'],['D','A','K','A']], word = "BLEED"
// Output: false
// Explanation: Cannot form "BLEED" without reusing the 'E' at position (1,1)

// Approach: Depth-First Search (DFS) with Backtracking
// 
// Key Insights:
// 1. This is a path-finding problem in a 2D grid with constraints
// 2. We need to explore all possible paths that could form the word
// 3. Backtracking is essential since we need to "undo" visited cells for other paths
//
// Algorithm:
// 1. Iterate through each cell in the grid as a potential starting point
// 2. If a cell matches the first character of the word, initiate DFS
// 3. During DFS:
//    - Mark the current cell as visited (using a sentinel value '#')
//    - Explore all 4 adjacent cells (up, down, left, right)
//    - If an adjacent cell matches the next character, recursively search from there
//    - After exploring all paths from current cell, restore its original value (backtrack)
// 4. Base case: If we've matched all characters (index == word.length - 1), return true
//
// Time Complexity: O(M * N * 4^L) where:
//   - M, N are grid dimensions (we try each cell as starting point)
//   - L is the length of the word (at each step, we explore up to 4 directions)
//   - In practice, pruning makes it much faster
//
// Space Complexity: O(L) where L is the length of the word
//   - Recursion stack depth is at most L
//   - We modify the grid in-place, so no additional space for visited tracking
//
// Design Decisions:
// 1. In-place marking vs separate visited array: Chose in-place to optimize space
// 2. Case sensitivity: Currently case-insensitive (can be made configurable)
// 3. Early termination: Return immediately upon finding the word
//
// Edge Cases Handled:
// - Null or empty grid/word
// - Single character word
// - Word longer than total grid cells
// - Grid boundaries during traversal

// ...existing code...

var board = new char[,] {
    {'B', 'L', 'C', 'H'},
    {'D', 'E', 'L', 'T'},
    {'D', 'A', 'K', 'A'},
};
var word1 = "BLEED";
var word2 = "BLEAK";

Console.WriteLine($"Has the word {word1}: {new Solution().HasWord(board, word1)}");
Console.WriteLine($"Has the word {word2}: {new Solution().HasWord(board, word2)}");

class Solution
{
    private static readonly (int row, int col)[] DIRECTIONS = { (-1, 0), (1, 0), (0, -1), (0, 1) };
    private const char VISITED_MARKER = '#';
    public bool HasWord(char[,] grid, string word)
    {
        if (grid == null || word == null || word.Length == 0)
            return false;

        int rows = grid.GetLength(0);
        int columns = grid.GetLength(1);

        if (word.Length > rows * columns)
            return false;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (grid[r, c] == word[0])
                {
                    if (DFS(grid, rows, columns, r, c, word, 0))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool DFS(char[,] grid, int rows, int columns, int row, int col, string word, int index)
    {
        if (index == word.Length - 1)
            return true;

        char originalChar = grid[row, col];
        grid[row, col] = VISITED_MARKER;

        foreach (var (dr, dc) in DIRECTIONS)
        {
            int nextRow = row + dr;
            int nextCol = col + dc;

            if (IsValidCell(rows, columns, nextRow, nextCol) && grid[nextRow, nextCol] == word[index + 1])
            {
                if (DFS(grid, rows, columns, nextRow, nextCol, word, index + 1))
                {
                    grid[row, col] = originalChar;
                    return true;
                }
            }
        }

        grid[row, col] = originalChar;
        return false;
    }

    private bool IsValidCell(int rows, int columns, int row, int col)
    {
        return row >= 0 && row < rows && col >= 0 && col < columns;
    }
}

