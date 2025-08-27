// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Given an integer array nums and an integer k, write a function to identify the highest possible sum of a subarray within nums, where the subarray meets the following criteria: its length is k, and all of its elements are unique.
Example 1:
Input:
nums = [3, 2, 2, 3, 4, 6, 7, 7, -1]
k = 4
Output: 20

Approach:
- Sliding window strategy 
- Keep track of a sliding window of unique elements of the start of the array, and the maxSum found so far
- As we sliding the window towards left, calculate the current sum and update the maxSum accordingly
- Ensure we consider unique elments in the subarray, use a dictionary to keep track of elements occurence count for this purpose

**/
int[] nums = { 3, 2, 2, 3, 4, 6, 7, 7, -1 };
int k = 4;

Debug.Assert(DistinctSubarraySum.FindMaxSum(nums, k) == 20, "Test 1 Failed");

int[] nums2 = { 1,5,4,2,9,9,9 };
int k2 = 3;

Debug.Assert(DistinctSubarraySum.FindMaxSum(nums2, k2) == 15, "Test 2 Failed");

int[] nums3 = { 4,4,4 };
int k3 = 3;

Debug.Assert(DistinctSubarraySum.FindMaxSum(nums3, k3) == 0, "Test 3 Failed");

Console.WriteLine("All tests passed!");

class DistinctSubarraySum
{
    public static int FindMaxSum(int[] nums, int k)
    {
        if (nums == null || nums.Length < k || k <= 0)
        {
            return 0;
        }

        int start = 0;
        int maxSum = int.MinValue;
        int currentSum = 0;
        Dictionary<int, int> seen = new Dictionary<int, int>();

        for (int end = 0; end < nums.Length; end++)
        {
            int currentValue = nums[end];

            seen[currentValue] = seen.GetValueOrDefault(currentValue, 0) + 1;
            currentSum += currentValue;

            while (seen[currentValue] > 1)
            {
                currentSum -= nums[start];
                seen[nums[start]]--;
                start++;

            }

            if (end - start + 1 == k)
            {
                maxSum = Math.Max(maxSum, currentSum);
                currentSum -= nums[start];
                seen[nums[start]]--;
                start++;
            }
        }

        return maxSum == int.MinValue ? 0 : maxSum;
    }
}
