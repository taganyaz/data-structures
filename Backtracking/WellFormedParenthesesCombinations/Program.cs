// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
/*
Problem: Generate Well-Formed Parentheses
Given n pairs of parentheses, generate all combinations of well-formed parentheses.

Example 1: n = 3 → ["((()))","(()())","(())()","()(())","()()()"]
Example 2: n = 1 → ["()"]
Constraints: 1 <= n <= 8

## Problem Analysis:
- This is a constrained combinatorial generation problem
- Total combinations without constraints: 2^(2n) 
- Valid combinations (Catalan number): C(n) = (2n)! / ((n+1)! * n!) ≈ 4^n / (n^(3/2) * sqrt(π))
- For n=8: ~430 valid combinations from ~65,536 possible arrangements

## Solution Approach: Backtracking with Pruning

### Key Insights:
1. **Constraint-based pruning**: We can eliminate invalid paths early by maintaining invariants:
   - Never place more ')' than '(' at any point (prefix property)
   - Never exceed n opening parentheses
   
2. **State representation**: Track only essential state (openCount, closedCount)
   - Space-efficient: O(n) vs storing full prefix at each recursion level
   
3. **Implicit ordering**: DFS with '(' before ')' naturally produces lexicographical order

### Algorithm:
1. Use DFS with backtracking to explore the solution space
2. At each position, we have up to 2 choices: place '(' or ')'
3. Prune invalid branches using constraints:
   - Can place '(' only if: openCount < n
   - Can place ')' only if: closedCount < openCount
4. Base case: When length reaches 2n, we have a valid combination

### Complexity Analysis:
- Time: O(4^n / sqrt(n)) - bounded by Catalan number * O(n) for string building
- Space: O(n) - recursion depth and StringBuilder
- Output space: O(n * C(n)) where C(n) is the nth Catalan number

### Design Decisions:
1. **StringBuilder over string concatenation**: Avoids O(n²) string operations
2. **In-place backtracking**: Reuse single StringBuilder to minimize allocations
3. **Early validation**: Check constraints before recursion, not after
4. **Exception vs empty list**: Throw for invalid input (fail-fast principle)

### Production Considerations:
- Thread-safe: Each call gets its own StringBuilder and result list
- Memory-efficient: Single StringBuilder reused via backtracking
- Scalable pattern: Easy to extend for other bracket types or constraints
*/

using System.Text;

// Test cases covering edge cases and typical scenarios
int n1 = 3;  // Standard case
int n2 = 1;  // Minimum valid input
int n3 = 5;  // Larger input to verify performance

var sol = new Solution();
Console.WriteLine($"{n1} combinations: [{string.Join(", ", sol.GetAllCombinations(n1))}]");
Console.WriteLine($"{n2} combinations: [{string.Join(", ", sol.GetAllCombinations(n2))}]");
Console.WriteLine($"{n3} combinations: [{string.Join(", ", sol.GetAllCombinations(n3))}]");


class Solution
{
    public List<string> GetAllCombinations(int n)
    {
        var result = new List<string>();
        if (n <= 0 )
        {
            return result;
        }
        if (n <= 0 || n > 8)
            throw new ArgumentOutOfRangeException("n");

        var combination = new StringBuilder(n * 2);
        DFS(n, 0, 0, result, combination);

        return result;
    }

    private void DFS(int n, int openCount, int closedCount, List<string> combinations, StringBuilder combination)
    {
        if (combination.Length == 2 * n)
        {
            combinations.Add(combination.ToString());
            return;
        }

        if (openCount < n)
        {
            combination.Append('(');
            DFS(n, openCount + 1, closedCount, combinations, combination);
            combination.Length--;
        }

        if (closedCount < openCount)
        {
            combination.Append(')');
            DFS(n, openCount, closedCount + 1, combinations, combination);
            combination.Length--;
        }
    }
}
