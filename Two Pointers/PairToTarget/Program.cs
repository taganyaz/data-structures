// See https://aka.ms/new-console-template for more information
/**
Problem:
Given a sorted array of integers nums, determine if there exists a pair of numbers that sum to a given target.
Example:
Input: nums = [1,3,4,6,8,10,13], target = 13
Output: True (3 + 10 = 13)
Input: nums = [1,3,4,6,8,10,13], target = 6
Output: False

Approach:
- I'll use two pointer approach
- Start by sorting the array
- Place two pointer at both ends of the array
- While left < right
    - Compare the sum of the elements at left and right with the target
    - If sum is equal, return true
    - If sum is less than target, increment the left pointer
    - Else decrement the right pointer
- If not pair is found, return false
**/
int[] nums = { 1, 3, 4, 6, 8, 10, 13 };
int target = 13;
Console.WriteLine($"Pair sum to {target} exists: {PairsToTarget.PairToTargetExist(nums, target)}");
class PairsToTarget
{
    public static bool PairToTargetExist(int[] arr, int target)
    {
        if (arr == null || arr.Length == 0)
            return false;

        Array.Sort(arr);
        int left = 0;
        int right = arr.Length - 1;

        while (left < right)
        {
            int sum = arr[left] + arr[right];

            if (sum == target)
                return true;

            if (sum < target)
                left++;
            else
            {
                right--;
            }


        }

        return false;
    }
}
