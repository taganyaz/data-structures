// See https://aka.ms/new-console-template for more information
/*
Problem: [existing problem statement]

CLARIFICATIONS & ASSUMPTIONS:
- All candidates are positive integers (based on examples)
- Input array contains distinct integers (no duplicates)
- Order of combinations doesn't matter: [2,2,3] and [3,2,2] are considered the same
- We need ALL valid combinations, not just the first one found
- Maximum 150 unique combinations (per problem constraints)

APPROACH:
This is a classic combinatorial search problem that requires exploring all possible combinations.
Key insights:
1. We can reuse numbers unlimited times → need to consider same index multiple times
2. Order doesn't matter → can enforce ascending selection to avoid duplicates
3. Early termination possible when current number exceeds remaining target

ALGORITHM CHOICE:
Backtracking with DFS is optimal here because:
- We need to explore all paths (not just find one solution)
- The search space is bounded by target/min(candidates)
- We can prune branches early when sum exceeds target
- Space complexity is optimized by reusing the same combination list

COMPLEXITY ANALYSIS:
- Time: O(N^(T/M)) where N = number of candidates, T = target, M = smallest candidate
  - Worst case: candidates=[1], target=40 → exploring 2^40 paths
  - Each recursive call branches into N possibilities
  - Maximum depth is T/M (target divided by minimum value)
- Space: O(T/M) for recursion stack depth + O(K) for K total elements in all combinations

OPTIMIZATION STRATEGIES:
1. Sort candidates to enable early termination
2. Skip candidates larger than remaining target
3. Use index-based iteration to avoid duplicates
4. Pass remaining target instead of accumulating sum

EDGE CASES HANDLED:
- Empty/null input array → return empty list
- No valid combinations → return empty list
- Target = 0 → return [[]] (empty combination)
- Single candidate array → either multiple uses or no solution

ALTERNATIVE APPROACHES CONSIDERED:
- Dynamic Programming: Would use O(target × N) space, good for counting but not for listing combinations
- BFS: Would use more memory for queue, no advantage over DFS here
- Iterative approach: More complex implementation without performance benefits
*/

 var candidates1 = new int[] {2,3,5};
int target1 = 8;
 var candidates2 = new int[] {2,3,6,7};
int target2 = 7;
 var candidates3 = new int[] {2};
int target3 = 1;

var sol = new Solution();
Console.WriteLine($"Candidates 1: [{string.Join(", ", candidates1)}], target 1: {target1}, Combinations: [{string.Join(", ", sol.GetCombinationsSumToTarget(candidates1, target1).Select(s => $"[{string.Join(", ", s)}]"))}]");
Console.WriteLine($"Candidates 2: [{string.Join(", ", candidates2)}], target 2: {target2}, Combinations: [{string.Join(", ", sol.GetCombinationsSumToTarget(candidates2, target2).Select(s => $"[{string.Join(", ", s)}]"))}]");
Console.WriteLine($"Candidates 3: [{string.Join(", ", candidates3)}], target 3: {target3}, Combinations: [{string.Join(", ", sol.GetCombinationsSumToTarget(candidates3, target3).Select(s => $"[{string.Join(", ", s)}]"))}]");
class Solution
{
    public List<List<int>> GetCombinationsSumToTarget(int[] candidates, int target)
    {
        var results = new List<List<int>>();
        if (candidates == null || candidates.Length == 0)
        {
            return results;
        }

        Array.Sort(candidates);

        DFS(candidates, 0, results, [], target);

        return results;
    }

    private void DFS(int[] candidates, int start, List<List<int>> results, List<int> combination, int currentTarget)
    {
        if (currentTarget == 0)
        {
            results.Add([.. combination]);
            return;
        }

        for (int i = start; i < candidates.Length; i++)
        {
            int currentItem = candidates[i];

            if (currentItem > currentTarget)
            {
                break;
            }

            combination.Add(currentItem);

            DFS(candidates, i, results, combination, currentTarget - currentItem);

            combination.RemoveAt(combination.Count - 1);

        }
    }
}
