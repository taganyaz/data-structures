// See https://aka.ms/new-console-template for more information


using System.Drawing;

/**
Problem:
Given an unsorted array of integers, implement the logic to sort the array using quicksort

Approach:
- Select a pivot element
- Arrange all the elements around the pivot such that smaller elements are on the left while large elements are on the right
- At this point, the pivot element is at the right position in the final sorted array, and partitions the array into left and right unsorted subarrays
- Repeat the process with the right and left subarrays recursively until the entire array is sorted

**/

var arr = new int[] { 7, 5, 9, 1, 13, 7, 19, 6, 3 };
Console.WriteLine($"Unsorted: [{string.Join(", ", arr)}]");
Quicksort.Sort(arr);
Console.WriteLine($"Sorted: [{string.Join(", ", arr)}]");

class Quicksort
{
    public static void Sort(int[] arr)
    {
        SortRecursive(arr, 0, arr.Length - 1);
    }

    private static void SortRecursive(int[] arr, int l, int r)
    {
        if (l < r)
        {
            int pivot = Partition(arr, l, r);

            SortRecursive(arr, l, pivot - 1);
            SortRecursive(arr, pivot + 1, r);
        }
    }

    private static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int j = low - 1;
        for (int i = low; i < high; i++)
        {
            if (arr[i] < pivot)
            {
                j++;
                (arr[j], arr[i]) = (arr[i], arr[j]);
            }
        }

        (arr[j + 1], arr[high]) = (arr[high], arr[j + 1]);
        return j + 1;

    }
}
