// See https://aka.ms/new-console-template for more information
/**
Problem:
Given an array of integers nums and an integer k, find the maximum sum of any contiguous subarray of size k.
Example 1: 
Input:
nums = [2, 1, 5, 1, 3, 2]
k = 3
Output: 9

Approach:
- Use sliding window approach
- Initialize maxSum to 0
- Initialize start to 0
- Traverse the array and compute current sum if window size is equal to k
- Update maxSum = max(maxSum, currentSum)
- Keep moving the window to the right by incrementing start by 1 at each step
**/
int[] nums = { 2, 1, 5, 1, 3, 2 };
int k = 3;
Console.WriteLine($"Max Sum: {MaxSumOfSubarrays.CalculateMaxSum(nums, k)}");
class MaxSumOfSubarrays
{
    public static int CalculateMaxSum(int[] nums, int k)
    {
        if (nums == null || nums.Length < k || k <= 0)
        {
            return 0;
        }

        int maxSum = int.MinValue;
        int currentSum = 0;
        int start = 0;

        for (int end = 0; end < nums.Length; end++)
        {
            int windowSize = end - start + 1;
            currentSum += nums[end];

            if (windowSize == k)
            {
                maxSum = Math.Max(maxSum, currentSum);
                currentSum -= nums[start];
                start++;
            }

        }
        return maxSum;
    }
}
