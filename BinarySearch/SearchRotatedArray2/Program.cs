// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
You are given a sorted array that has been rotated at an unknown pivot point, along with a target value. Develop an algorithm to locate the index of the target value in the array. If the target is not present, return -1. 
The algorithm should have a time complexity of O(log n).
Note:

The array was originally sorted in ascending order before being rotated.
The rotation could be at any index, including 0 (no rotation).
You may assume there are no duplicate elements in the array.
Example 1:
Input: nums = [4,5,6,7,0,1,2], target = 0
Output: 4 (The index of 0 in the array)

Example 2:
Input: nums = [4,5,6,7,0,1,2], target = 3
Output: -1 (3 is not in the array)

**/
Debug.Assert(ArraySearch.SearchRotated([4, 5, 6, 7, 0, 1, 2], 0) == 4, "Test 1: should return 4");
Debug.Assert(ArraySearch.SearchRotated([4, 5, 6, 7, 0, 1, 2], 3) == -1, "Test 1: should return -1");

Console.WriteLine("All tests have passed!");
class ArraySearch
{
    public static int SearchRotated(int[] nums, int target)
    {
        if (nums == null || nums.Length == 0)
            return -1;

        int left = 0;
        int right = nums.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;

            if (nums[mid] == target)
                return mid;

            if (nums[left] <= nums[mid])
            {
                if (target < nums[mid] && nums[left] <= target)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }
            else
            {
                if (target > nums[mid] && target <= nums[right])
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }
        }
        return -1;
    }
}
