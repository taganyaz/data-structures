// See https://aka.ms/new-console-template for more information
/**
Problem: 
Given an unsorted array of integers, write a function to sort the array using merge sort

Approach
- We'll use divide and conqor approach
- We will start by dividing the array into two halves and sort each have separately
- We'll recursively call the merge function to sort each half, dividing the array into smaller subarrays until we have array of 1 item which is effectively sorted
- The we will merge each of the subarrays in an ordrerdfashion
**/

var arr = new int[] { 8, 9, 2, 7, 5, 4, 12, 1, 6 };
Console.WriteLine($"Unsorted: [{string.Join(", ", arr)}]");
Solution.Sort(arr);
Console.WriteLine($"Sorted: [{string.Join(", ", arr)}]");
class Solution
{
    public static void Sort(int[] arr)
    {
        SortUtil(arr, 0, arr.Length - 1);
    }

    private static void SortUtil(int[] arr, int l, int r)
    {
        if (l < r)
        {
            int mid = l + (r - l) / 2;

            SortUtil(arr, l, mid);
            SortUtil(arr, mid + 1, r);

            Merge(arr, l, mid, r);
        }
    }

    private static void Merge(int[] arr, int l, int mid, int r)
    {
        int n1 = mid - l + 1;
        int n2 = r - mid;

        int[] arr1 = new int[n1];
        int[] arr2 = new int[n2];

        for (int i = 0; i < n1; i++)
        {
            arr1[i] = arr[l + i];
        }

        for (int i = 0; i < n2; i++)
        {
            arr2[i] = arr[mid + i + 1];
        }

        int k = l;
        int p1 = 0;
        int p2 = 0;

        while (p1 < n1 && p2 < n2)
        {
            if (arr1[p1] < arr2[p2])
            {
                arr[k] = arr1[p1];
                k++;
                p1++;
            }
            else
            {
                arr[k] = arr2[p2];
                k++;
                p2++;
            }
        }

        while (p1 < n1)
        {
            arr[k] = arr1[p1];
            k++;
            p1++;
        }

        while (p2 < n2)
        {
            arr[k] = arr2[p2];
            k++;
            p2++;
        }


    }
}
