// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Given an integer array nums, return true if there exists a triple of indices (i, j, k) such that i < j < k and nums[i] < nums[j] < nums[k]. If no such indices exists, return false.

Example 1:

Input: nums = [1,2,3,4,5]
Output: true
Explanation: Any triplet where i < j < k is valid.
Example 2:

Input: nums = [5,4,3,2,1]
Output: false
Explanation: No triplet exists.
Example 3:

Input: nums = [2,1,5,0,4,6]
Output: true
Explanation: One of the valid triplet is (3, 4, 5), because nums[3] == 0 < nums[4] == 4 < nums[5] == 6.

Approach:
- Use monotonically increasing stack
**/
Debug.Assert(Triples.TripletsExists([1, 2, 3, 4, 5]) == true, "Test 1: Should return true");
Debug.Assert(Triples.TripletsExists([5, 4, 3, 2, 1]) == false, "Test 2: Should return false");
Debug.Assert(Triples.TripletsExists([2, 1, 5, 0, 4, 6]) == true, "Test 3: Should return true");

Console.WriteLine("All tests have passed!");

class Triples
{
    public static bool TripletsExists(int[] nums)
    {
        if (nums == null || nums.Length < 3)
        {
            return false;
        }

        int first = int.MaxValue;
        int second = int.MaxValue;

        foreach (var num in nums)
        {
            if (num <= first)
            {
                first = num;
            }
            else if (num <= second)
            {
                second = num;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
}
