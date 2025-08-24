// See https://aka.ms/new-console-template for more information
/**
Problem:
Given an integer input array heights representing the heights of vertical lines, write a function that returns the maximum area of water that can be contained by two of the lines (and the x-axis). The function should take in an array of integers and return an integer.

Example 1:
Inputs: heights = [3,4,1,2,2,4,1,3,2]
Output: 21

Example 2:
Inputs: heights = [1,2,1]
Output: 2

Approach:
- Use two pointer
- Place two pointers at both ends
- Initialize maxArea to track the maximum area found so far
- While left < right
- Calculate current area as right - left * min(heights[left], heights[right])
- If currentArea is greater than maxArea, update maxArea to currentArea
- If heights[left] < heights[right], increment left by 1, else decrement right by 1

**/
int[] heights = { 3, 4, 1, 2, 2, 4, 1, 3, 2 };
Console.WriteLine($"MaxArea: {ContainerWater.GetMaxWater(heights)}");
class ContainerWater
{
    public static int GetMaxWater(int[] heights)
    {
        if (heights == null || heights.Length < 2)
            return 0;

        int maxArea = 0;
        int left = 0;
        int right = heights.Length - 1;

        while (left < right)
        {
            int width = right - left;
            int currentArea = width * Math.Min(heights[left], heights[right]);

            if (currentArea > maxArea)
                maxArea = currentArea;

            if (heights[left] < heights[right])
                left++;
            else
                right--;
        }
        return maxArea;

    }
}
