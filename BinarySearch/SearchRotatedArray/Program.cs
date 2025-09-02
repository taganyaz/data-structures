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

Approach:
- Use binary search
- Initialize left to 0 and right to nums.Length -1
- While left <= right
    - Find mid.
     a) If nums[mid] == target return mid
     b) If nums[mid] > nums[left] and nums[mid] > target  or nums[mid] < nums[left] and nums[mid] > target, update right = mid - 1
     c) Else update left = mid + 1
- Retrun -1
 
**/
int[] nums = [4, 5, 6, 7, 0, 1, 2];

int output1 = 4;
var result1 = BinarySearch.SearchRotatedArray(nums, 0);

int output2 = -1;
var result2 = BinarySearch.SearchRotatedArray(nums, 3);

Debug.Assert(result1 == output1, "Test1: Should return correct index for existing number");
Debug.Assert(result2 == output2, "Test2: Should return -1 for non-existing number");

Console.WriteLine("All tests have passed!");
class BinarySearch
{
    public static int SearchRotatedArray(int[] nums, int target)
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
            else if (nums[left] <= nums[mid])
            {
                if (nums[left] <= target && target < nums[mid])
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
                if (nums[mid] < target && target <= nums[right])
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
