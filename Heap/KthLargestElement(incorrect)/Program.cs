// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Given an integer array nums and an integer k, return the kth largest element in the array.
Note that it is the kth largest element in the sorted order, not the kth distinct element.
Can you solve it without sorting?

Example 1:
Input: nums = [3,2,1,5,6,4], k = 2
Output: 5

Example 2:
Input: nums = [3,2,3,1,2,4,5,5,6], k = 4
Output: 4

Approach:
- Use a max heap
- Add the first elements of the array in a min heap
- Traverse the reamining elements
    - If element is greater than top of the heap, deque and enque current element
**/
Debug.Assert(ArrayUtil.GetKthLargestElement([3, 2, 1, 5, 6, 4], 2) == 5, "Test 1: Should return 5");
Debug.Assert(ArrayUtil.GetKthLargestElement([3,2,3,1,2,4,5,5,6], 4) == 4, "Test 2: Should return 4");

Console.WriteLine("All tests have passed!");

class ArrayUtil
{
    public static int? GetKthLargestElement(int[] nums, int k)
    {
        if (nums == null || nums.Length < k)
            return null;

        PriorityQueue<int, int> minHeap = new PriorityQueue<int, int>();

        foreach (int num in nums)
        {
            minHeap.Enqueue(num, num);

            if (minHeap.Count > k)
            {
                minHeap.Dequeue();
            }
        }

        return minHeap.Peek();

    }
}
