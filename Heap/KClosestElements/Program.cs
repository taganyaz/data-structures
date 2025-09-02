// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Given a sorted integer array arr, two integers k and x, return the k closest integers to x in the array. The result should also be sorted in ascending order.
An integer a is closer to x than an integer b if:
|a - x| < |b - x|, or
|a - x| == |b - x| and a < b
 
Example 1:
Input: arr = [1,2,3,4,5], k = 4, x = 3
Output: [1,2,3,4]

Example 2:
Input: arr = [1,1,2,3,4,5], k = 4, x = -1
Output: [1,1,2,3]

Approach:
- Use max heap to store k closest elements

**/
//Debug.Assert(KClosestElemetnts.GetKClosest([1, 2, 3, 4, 5], 4, 3).SequenceEqual([1, 2, 3, 4]), "Test 1: Should return expected output");
Debug.Assert(KClosestElemetnts.GetKClosest([1, 1, 2, 3, 4, 5], 4, -1).SequenceEqual([1, 1, 2, 3]), "Test 2: Should return expected output");

Console.WriteLine("All tests have passed.");

class KClosestElemetnts
{
    public static List<int> GetKClosest(int[] arr, int k, int x)
    {
        if (arr == null || arr.Length < k)
            return [];

        PriorityQueue<int, int> maxHeap = new PriorityQueue<int, int>();

        foreach (var item in arr)
        {
            maxHeap.Enqueue(item, -Math.Abs(x - item));

            if (maxHeap.Count > k)
            {
                maxHeap.Dequeue();
            }
        }

        return maxHeap.UnorderedItems.Select(p => p.Element).OrderBy(t => t).ToList();
    }
}
