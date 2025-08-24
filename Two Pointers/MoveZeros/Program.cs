// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Given an integer array nums, write a function to rearrange the array by moving all zeros to the end while keeping the order of non-zero elements unchanged. Perform this operation in-place without creating a copy of the array.

Example
Input: nums = [2,0,4,0,9]
Output: [2,4,9,0,0]


Approach:
- Use two pointers to move from start to end of the array
- First pointer indicates current index we're processin
- Second pointer indicates the next non-zero index 
- For each non-zero element
    - Assign the element to next non-zero index
    - Increase non-zero index by 1
- Now replace all elements at current non-zero index to the end with zero
**/
int[] nums = { 2, 0, 4, 0, 9 };
MoveZeros.Move(nums);
Debug.Assert(nums.SequenceEqual(new[] { 2, 4, 9, 0, 0 }));
Console.WriteLine($"Result: {string.Join(", ", nums)}");
class MoveZeros
{
    public static void Move(int[] arr)
    {
        if (arr == null || arr.Length == 1)
            return;

        int n = arr.Length;
        int nextNonZero = 0;

        for (int i = 0; i < n; i++)
        {
            if (arr[i] != 0)
            {
                arr[nextNonZero] = arr[i];
                nextNonZero++;
            }
        }

        Array.Fill(arr, 0, nextNonZero, n - nextNonZero);
        // if (nextNonZero > 0)
        // {
        //     for (int i = nextNonZero; i < n; i++)
        //     {
        //         arr[i] = 0;
        //     }
        // }

    }
}
