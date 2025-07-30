// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
/*
Problem: Climbing Stairs (LeetCode #70)
========================================
You are climbing a staircase. It takes n steps to reach the top.
Each time you can either climb 1 or 2 steps. In how many distinct ways can you climb to the top?

Example 1:
Input: n = 2
Output: 2
Explanation: There are two ways to climb to the top.
1. 1 step + 1 step
2. 2 steps

Example 2:
Input: n = 3
Output: 3
Explanation: There are three ways to climb to the top.
1. 1 step + 1 step + 1 step
2. 1 step + 2 steps
3. 2 steps + 1 step

Constraints:
- 1 <= n <= 45

Approach: Dynamic Programming (Bottom-Up with Space Optimization)
================================================================

Key Insight:
To reach step n, we can either:
1. Take 1 step from position (n-1), OR
2. Take 2 steps from position (n-2)

Therefore: dp[n] = dp[n-1] + dp[n-2]

This recurrence relation is identical to the Fibonacci sequence, where:
- Base cases: dp[0] = 1 (one way to stay at ground), dp[1] = 1 (one way to reach first step)
- Transition: dp[i] = dp[i-1] + dp[i-2] for i >= 2

Space Optimization:
Since we only need the previous two values to compute the current value,
we can optimize from O(n) space to O(1) space by using two variables.

Time Complexity: O(n) - single pass through n iterations
Space Complexity: O(1) - only two variables needed regardless of input size

Alternative Approaches:
1. Recursive with Memoization: O(n) time, O(n) space
2. Matrix Exponentiation: O(log n) time, O(1) space
3. Binet's Formula: O(1) time, O(1) space (but has precision issues for large n)

Algorithm:
1. Handle edge cases (n < 1 or n > 45)
2. Special case for n = 1 (return 1)
3. Initialize two variables: prev2 = 1 (dp[0]), prev1 = 1 (dp[1])
4. Iterate from i = 2 to n:
   - Calculate current = prev2 + prev1
   - Update variables using tuple assignment: (prev2, prev1) = (prev1, current)
5. Return prev1 (which holds dp[n])
*/

Console.WriteLine($"Ways to climb 2 steps-staircase: {new Solution().GetDistinctWaysToClimbStairCase(2)}");
Console.WriteLine($"Ways to climb 3 steps-staircase: {new Solution().GetDistinctWaysToClimbStairCase(3)}");
Console.WriteLine($"Ways to climb 6 steps-staircase: {new Solution().GetDistinctWaysToClimbStairCase(6)}");
class Solution
{
    public int GetDistinctWaysToClimbStairCase(int n)
    {
        if (n < 1 || n > 45)
        {
            throw new ArgumentOutOfRangeException("n must be between 1 and 45");
        }

        if (n == 1) return 1;

        int prev2 = 1;
        int prev1 = 1;

        for (int i = 2; i < n + 1; i++)
        {
            int current = prev2 + prev1;
            (prev2, prev1) = (prev1, current);
        }

        return prev1;
    }
}
