// See https://aka.ms/new-console-template for more information
/**
Problem:
Write a function to calculate the total amount of water trapped between bars on an elevation map, where each bar's width is 1. The input is given as an array of n non-negative integers height representing the height of each bar.
Input:

# .  .  .  .  .  x  .  .  .
# .  x  .  .  .  x  .  .  .
# x  x  .  .  .  x  .  .  .
# x  x  .  x  x  x  .  .  x
# x  x  x  x  x  x  x  .  x
# ==================================
height = [3, 4, 1, 2, 2, 5, 1, 0, 2]
Output:10

Approach:
- Use two pointers strategy
- At each index i, trapped water = min(maxLeft, maxRight) -  height[i]
- Initialize left to 0 and maxLeft = height[0]
- Initialize right = n -1 and maxRight = height[n - 1]
- Initialize maxWater = 0
- While left < right
    - If maxLeft < maxRight
        a) if height[left] >= maxLeft, update maxLeft = height[left]
        b) else update maxWater +=  maxLeft - height[left]
        c) increment left by 1
    - Else
        a) if height[right] >= maxRight, update maxRight = height[right]
        b) else update maxWater += maxRight - height[right]
        c) decrement right by 1
**/
int[] height = { 3, 4, 1, 2, 2, 5, 1, 0, 2 };
Console.WriteLine($"Max Water: {RainWater.Calculate(height)}");
class RainWater
{
    public static int Calculate(int[] height)
    {
        if (height == null || height.Length < 2)
            return 0;

        int maxWater = 0;
        int left = 0;
        int right = height.Length - 1;
        int maxLeft = height[left];
        int maxRight = height[right];

        while (left <= right)
        {
            if (maxLeft < maxRight)
            {
                if (height[left] >= maxLeft)
                {
                    maxLeft = height[left];
                }
                else
                {
                    maxWater += maxLeft - height[left];
                }
                left++;
            }
            else
            {
                if (height[right] >= maxRight)
                {
                    maxRight = height[right];
                }
                else
                {
                    maxWater += maxRight - height[right];
                }
                right--;
            }
        }
        return maxWater;
    }
}
