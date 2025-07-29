// See https://aka.ms/new-console-template for more information
// ...existing code...
// Problem: You are given a chessboard of infinite size where the coordinates of each cell are defined by integer pairs (x, y). The knight piece moves in an L-shape, either two squares horizontally and one square vertically, or two squares vertically and one square horizontally.
// Write a function to determine the minimum number of moves required for the knight to move from the starting position (0, 0) to the target position (x, y). Assume that it is always possible to reach the target position, and that x and y are both integers in the range [-200, 200]
// Example 1:
// Input: x = 1, y = 2
// Output: 1
// Example 2:
// Input: x = 4, y = 4
// Output: 4

// Approach: BFS (Breadth-First Search) - Optimal for unweighted shortest path problems
// 
// Why BFS?
// - Guarantees shortest path in unweighted graphs (all moves have equal cost)
// - Explores positions level by level, finding minimum moves naturally
// - Time Complexity: O(|x| * |y|) in worst case, where we explore area proportional to target distance
// - Space Complexity: O(|x| * |y|) for visited set and queue
//
// Algorithm:
// 1. Base case: if target is origin (0,0), return 0 moves
// 2. Initialize BFS structures:
//    - Queue: tracks positions to explore (starting with origin)
//    - Visited set: prevents revisiting positions (cycle prevention)
//    - Move deltas: 8 possible L-shaped knight moves
// 3. Level-wise BFS traversal:
//    - Process all positions at current distance before moving to next
//    - For each position, try all 8 possible knight moves
//    - Early termination: return immediately when target is reached
//    - Mark new positions as visited to avoid redundant exploration
//
// Optimization Opportunities:
// - Symmetry reduction: leverage 8-fold symmetry to reduce search space
// - Bidirectional search: search from both start and target simultaneously
// - Pruning: limit search boundary based on target coordinates
// - Precomputation: for repeated queries, cache results or use formula for small targets
//
// Edge Cases Handled:
// - Starting position equals target (return 0)
// - Negative coordinates (knight moves work identically)
// - Maximum distance targets within [-200, 200] range


Console.WriteLine($"Moves from (0,0) to (1,2): {new Solution().GetKnightMovesToPosition(1, 2)}");
Console.WriteLine($"Moves from (0,0) to (4,4): {new Solution().GetKnightMovesToPosition(4, 4)}");
class Solution
{
    public int GetKnightMovesToPosition(int x, int y)
    {
        if ((x, y) == (0, 0))
            return 0;

        int movesCount = 0;
        var visited = new HashSet<(int x, int y)>();
        var queue = new Queue<(int x, int y)>();
        var deltas = new List<(int x, int y)> { (2, 1), (2, -1), (-2, 1), (-2, -1), (1, 2), (1, -2), (-1, 2), (-1, -2) };

        queue.Enqueue((0, 0));
        visited.Add((0, 0));

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;
            movesCount++;

            for (int i = 0; i < levelSize; i++)
            {
                var (cx, cy) = queue.Dequeue();

                foreach (var (dx, dy) in deltas)
                {
                    var (nx, ny) = (cx + dx, cy + dy);

                    if ((nx, ny) == (x, y))
                        return movesCount;

                    if (!visited.Contains((nx, ny)))
                    {
                        queue.Enqueue((nx, ny));
                        visited.Add((nx, ny));
                    }
                }
            }
        }
        return -1;
    }
}

