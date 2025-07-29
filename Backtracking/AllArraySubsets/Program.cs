// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
/**
Problem: Given an integer array nums of unique elements, return all possible subsets (the power set).
The solution set must not contain duplicate subsets. Return the solution in any order.

Example 1:
Input: nums = [1,2,3]
Output: [[],[1],[2],[1,2],[3],[1,3],[2,3],[1,2,3]]

Example 2:
Input: nums = [0]
Output: [[],[0]]

Constraints:
- 1 <= nums.length <= 10
- -10 <= nums[i] <= 10
- All elements are unique

Problem Analysis:
This is a classic combinatorial generation problem where we need to generate all 2^n possible subsets
of n elements. Each element has two choices: either be included in the subset or not.

Approach 1: Recursive Backtracking (Include/Exclude Pattern)
Time Complexity: O(2^n * n) - We generate 2^n subsets, each requiring O(n) to copy
Space Complexity: O(n) - Recursion depth and path storage (excluding output)

Key Insights:
- Uses a decision tree where each level represents a choice for one element
- Maintains a single path that's modified via backtracking, saving memory
- Natural fit for problems with binary choices (include/exclude)
- Stack depth limited by array size (max 10 per constraints)

Algorithm:
1. Start with empty path and index 0
2. At each element, make two recursive calls:
   a. Include current element: Add to path, recurse with index+1, then backtrack
   b. Exclude current element: Recurse with index+1 without adding
3. Base case: When index == nums.Length, we've made decisions for all elements

Approach 2: Iterative (Cascading Pattern)
Time Complexity: O(2^n * n) - Same as recursive
Space Complexity: O(2^n * n) - Stores intermediate results

Key Insights:
- Builds subsets incrementally by doubling the result set for each element
- No recursion risk, suitable for environments with limited stack space
- More intuitive for some developers but uses more memory
- Can be optimized to generate subsets on-demand using iterators

Algorithm:
1. Start with empty subset: [[]]
2. For each element in nums:
   - For each existing subset, create a new subset by adding current element
   - Add all new subsets to the result

Alternative Approaches for Production:
1. Bit Manipulation: Use binary representation (000 to 111 for n=3) where each bit
   indicates inclusion/exclusion. Most memory efficient for iteration.
2. Generator Pattern: Yield subsets one at a time for memory-constrained scenarios
3. Parallel Generation: For very large n, partition the problem space

Trade-offs:
- Recursive: Elegant, memory-efficient, but limited by stack depth
- Iterative: No stack limitations, easier to debug, but higher memory usage
- Bit Manipulation: Most efficient for certain operations, less readable

Production Considerations:
- Input validation: Check for null, duplicates (violates problem constraints)
- For n > 20, consider warning user about exponential growth
- Consider returning IEnumerable<IReadOnlyList<int>> for better encapsulation
**/

// ...existing code...
var num1 = new int[] { 1, 2, 3 };
var num2 = new int[] { 0 };
var sol = new Solution();

Console.WriteLine($"[{string.Join(", ", num1)}] Subsets: [{string.Join(", ", sol.GetAllSubsets(num1).Select(s => $"[{string.Join(", ", s)}]"))}]");
Console.WriteLine($"[{string.Join(", ", num2)}] Subsets: [{string.Join(", ", sol.GetAllSubsets(num2).Select(s => $"[{string.Join(", ", s)}]"))}]"); 

Console.WriteLine($"[{string.Join(", ", num1)}] Subsets Iter: [{string.Join(", ", sol.GetAllSubsetsIterative(num1).Select(s => $"[{string.Join(", ", s)}]"))}]"); 
Console.WriteLine($"[{string.Join(", ", num2)}] Subsets Iter: [{string.Join(", ", sol.GetAllSubsetsIterative(num2).Select(s => $"[{string.Join(", ", s)}]"))}]"); 
class Solution
{
    public List<List<int>> GetAllSubsets(int[] nums)
    {

        var results = new List<List<int>>();

        if (nums == null || nums.Length == 0)
            return results;

        DFS(nums, results, [], 0);

        return results;
    }

    public List<List<int>> GetAllSubsetsIterative(int[] nums)
    {
        var results = new List<List<int>>();

        if (nums == null || nums.Length == 0)
            return results;

        var subset = new List<int>();
        results.Add([.. subset]);

        for (int i = 0; i < nums.Length; i++)
        {
            var temp = new List<List<int>>();

            foreach (var s in results)
            {
                temp.Add([.. s]);
                temp.Add([.. s, nums[i]]);
            }

            results = temp;
        }

        return results;
    }

    private void DFS(int[] nums, List<List<int>> subsets, List<int> path, int index)
    {
        if (index == nums.Length)
        {
            subsets.Add([.. path]);
            return;
        }

        path.Add(nums[index]);
        DFS(nums, subsets, path, index + 1);

        path.RemoveAt(path.Count - 1);
        DFS(nums, subsets, path, index + 1);

    }
}
