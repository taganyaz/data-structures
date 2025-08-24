// See https://aka.ms/new-console-template for more information
/**
Problem: Trapping Rain Water
Given n non-negative integers representing an elevation map where the width of each bar is 1, 
compute how much water can be trapped after raining.

Example:
Input: height = [3, 4, 1, 2, 2, 5, 1, 0, 2]
Output: 10

Visual Representation:
      |
    | * * * |
    | * * * | * * |
| * | * | | | * | |
| | | | | | | | | |
3 4 1 2 2 5 1 0 2
Water trapped: 0+0+3+2+2+0+3+4+0 = 10

Approach: Two-Pointer Technique
Time Complexity: O(n) - Single pass through the array
Space Complexity: O(1) - Only using constant extra space

Key Insight:
Water trapped at any position is determined by the minimum of the maximum heights 
to its left and right, minus its own height.
Water[i] = min(maxLeft[i], maxRight[i]) - height[i]

Algorithm:
1. Initialize two pointers at array boundaries (left=0, right=n-1)
2. Track maximum heights encountered from each direction (maxLeft, maxRight)
3. Move the pointer with smaller max height inward:
   - This guarantees that the water level at current position is determined by the smaller max
   - If current height < corresponding max, water can be trapped
   - Otherwise, update the max for that direction

Why This Works:
- At any position, water level is constrained by the lower of the two boundaries
- By moving the pointer with smaller max, we ensure we've found the limiting factor
- The algorithm efficiently computes water trapped without pre-computing max arrays

Edge Cases:
- Array with less than 3 elements (no water can be trapped)
- All elements equal (no water trapped)
- Strictly increasing/decreasing array (no water trapped)
- Null or empty array
**/

int[] heights = { 3, 4, 1, 2, 2, 5, 1, 0, 2 };
Console.WriteLine($"Max Water: {Trap.CalculateMaxTrappedWater(heights)}");
class Trap
{
    public static int CalculateMaxTrappedWater(int[] arr)
    {
        if (arr == null || arr.Length < 3)
            return 0;

        int n = arr.Length;
        int left = 0;
        int right = n - 1;

        int maxLeft = arr[left];
        int maxRight = arr[right];
        int totalWater = 0;

        while (left < right)
        {
            if (maxLeft < maxRight)
            {
                left++;
                if (arr[left] >= maxLeft)
                {
                    maxLeft = arr[left];
                }
                else
                {
                    totalWater += maxLeft - arr[left];
                }
            }
            else
            {
                right--;
                if (arr[right] >= maxRight)
                {
                    maxRight = arr[right];
                }
                else
                {
                    totalWater += maxRight - arr[right];
                }
            }
        }

        return totalWater;
    }
}
