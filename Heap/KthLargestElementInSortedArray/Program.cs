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

**/
Debug.Assert(ArrayUtil.FindKClosest2([1, 2, 3, 4, 5], 4, 3).SequenceEqual([1, 2, 3, 4]), "Test1: should return correct output");
Debug.Assert(ArrayUtil.FindKClosest2([1, 1, 2, 3, 4, 5], 4, -1).SequenceEqual([1, 1, 2, 3]), "Test2: should return correct output");

Console.WriteLine("All tests have passed!");

class ArrayUtil
{
    public static List<int> FindKClosest(int[] arr, int k, int x)
    {
        if (arr == null || arr.Length < k)
            return [];

        var comparer = Comparer<(int distance, int value)>.Create((a, b) =>
        {
            int result = b.distance.CompareTo(a.distance);
            if (result != 0)
                return result;

            return b.value.CompareTo(a.value);

        });

        PriorityQueue<int, (int distance, int value)> maxHeap = new PriorityQueue<int, (int distance, int value)>(comparer);

        for (int i = 0; i < arr.Length; i++)
        {
            maxHeap.Enqueue(arr[i], (Math.Abs(x - arr[i]), arr[i]));

            if (maxHeap.Count > k)
                maxHeap.Dequeue();
        }

        return maxHeap.UnorderedItems.Select(x => x.Element).OrderBy(x => x).ToList();
    }

    public static List<int> FindKClosest2(int[] arr, int k, int x)
    {
        if (arr == null || arr.Length < k)
            return [];

        int left = 0;
        int right = arr.Length - k;

        while (left < right)
        {
            int mid = left + (right - left) / 2;

            if (x - arr[mid] > arr[mid + k] - x)
            {
                left = mid + 1;
            }
            else
            {
                right = mid;
            }
        }

        return arr.Skip(left).Take(k).ToList();
    }
}
