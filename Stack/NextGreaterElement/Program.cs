// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Given an array of integers, find the next greater element for each element in the array. The next greater element of an element x is the first element to the right of x that is greater than x. If there is no such element, then the next greater element is -1.
Example
Input: [2, 1, 3, 2, 4, 3]
Output: [3, 3, 4, 4, -1, -1]

Approach:
- Use a stack 
- Maintain a result array with all elements set t0 -1
- Initialize a stack with first element
- Traverse the array from left to right (from second element)
- While stack.Peek() < current item
    - Update result[stack.Peek()] current item
- Push current index to stack
**/
int[] nums = [2, 1, 3, 2, 4, 3];
int[] output = [3, 3, 4, 4, -1, -1];
int[] result = ArrayUtil.GetNextGreater(nums);

Debug.Assert(result.SequenceEqual(output), "Should return correct output");

Console.WriteLine("Test passed!");

class ArrayUtil
{
    public static int[] GetNextGreater(int[] nums)
    {
        if (nums == null || nums.Length == 0)
            return Array.Empty<int>();

        int n = nums.Length;
        int[] result = new int[n];
        Array.Fill(result, -1);

        Stack<int> stack = new Stack<int>();

        for (int i = 0; i < n; i++)
        {
            while (stack.Count > 0 && nums[stack.Peek()] < nums[i])
            {
                result[stack.Pop()] = nums[i];
            }
            stack.Push(i);
        }

        return result;
    }
}
