// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Given a sorted array of integers nums and a target value target, write a function to determine if target is in the array. If target is present in the array, return its index. Otherwise, return -1.
Example 1:
Input: nums = [-1,0,3,5,9,12], target = 9
Output: 4 (nums[4] = 9)
Example 2:
Input: nums = [-1,0,3,5,9,12], target = 2
Output: -1 (2 is not in the array, so we return -1.)

Approach:
- Use binary search
- Initialize left to 0 and right to nums.Length -1
- Find the array mid point = left + (right - left)/ 2
- If nums[mid] == target, return mid
- If mid[mid] < target, update left = mid + 1
- Else  update rigt = mid - 1
- If you reach the end, left > right, return -1

**/
int[] nums1 = [-1, 0, 3, 5, 9, 12];
int target1 = 9;
int output1 = 4;
int result1 = BinarySearch.Search(nums1, target1);


int[] nums2 = [-1, 0, 3, 5, 9, 12];
int target2 = 2;
int output2 = -1;
int result2 = BinarySearch.Search(nums2, target2);

Debug.Assert(result1 == output1, "Test 1: should return correct result");
Debug.Assert(result2 == output2, "Test 2: should return correct result");

Console.WriteLine("All test have passed!");

class BinarySearch
{
    public static int Search(int[] nums, int target)
    {
        if (nums == null || nums.Length == 0)
            return -1;

        int left = 0;
        int right = nums.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;

            if (nums[mid] == target)
            {
                return mid;
            }
            else if (nums[mid] < target)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }

        }
        return -1;
    }
}
