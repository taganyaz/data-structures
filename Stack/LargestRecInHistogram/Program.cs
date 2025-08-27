// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Given an integer array heights representing the heights of histogram bars, write a function to find the largest rectangular area possible in a histogram, where each bar's width is 1.
Example
Inputs: heights = [2,8,5,6,2,3]
Output: 15

Approach:
- Use a stack (monotonically increasing)
- For height, find smaller left and right and calc area that includes the height
- Track maxArea found so far
- Traverse array from left to right
- At each step
    - if stack is empty or current height is greater than stack top height, push current index to stack 
    - Else calc area at current
        a) set topIndex = stack.Pop()
        b) set height = heights[topIndex]
        c) set width = stack.Count == 0? index: index - stack.Peek() -1
        d) update maxArea = max(maxArea, height * width)

- Process all the items in stack
- While stack is not empty
    a) set topIndex = stack.Pop()
    b) set height = heights[topIndex]
    c) set width = stack.count == 0? index : index - stack.Peek() - 1
    d) update maxArea = max(maxArea, height * width)

**/
int[] heights1 = [2, 8, 5, 6, 2, 3];
int output1 = 15;
int result1 = Solution.LargestRectangleArea(heights1);

Debug.Assert(result1 == output1, "Test 1: Should return correct output");

Console.WriteLine("All tests passed!");
class Solution
{
    public static int LargestRectangleArea(int[] heights)
    {
        if (heights == null || heights.Length == 0)
        {
            return 0;
        }

        int n = heights.Length;
        int index = 0;
        int maxArea = 0;
        Stack<int> stack = new Stack<int>();

        while (index < n)
        {
            if (stack.Count == 0 || heights[index] >= heights[stack.Peek()])
            {
                stack.Push(index);
                index++;
            }
            else
            {
                int topIndex = stack.Pop();
                int height = heights[topIndex];
                int width = stack.Count == 0 ? index : index - stack.Peek() - 1;

                maxArea = Math.Max(maxArea, height * width);
            }
        }

        while (stack.Count > 0)
        {
            int topIndex = stack.Pop();
            int height = heights[topIndex];
            int width = stack.Count == 0 ? index : index - stack.Peek() - 1;

            maxArea = Math.Max(maxArea, height * width);
        }

        return maxArea;
    }
}
