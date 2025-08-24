// See https://aka.ms/new-console-template for more information
/**
 * Problem: Sort an array of integers using Heap Sort algorithm
 * 
 * Algorithm Overview:
 * Heap Sort is an in-place, comparison-based sorting algorithm that uses a binary heap data structure.
 * Time Complexity: O(n log n) for all cases (best, average, worst)
 * Space Complexity: O(1) - sorts in-place
 * 
 * Implementation Details:
 * 1. Build Phase (Heapification):
 *    - Convert the input array into a max-heap structure
 *    - Start from the last non-leaf node (at index n/2 - 1) and heapify backwards
 *    - This ensures parent nodes are processed after their children
 *    
 * 2. Extract Phase (Sorting):
 *    - Repeatedly extract the maximum element (root) and place it at the end
 *    - After each extraction, restore the heap property for the remaining elements
 *    - The sorted portion grows from right to left
 *    
 * Key Concepts:
 * - Max-Heap Property: Parent node ≥ all children
 * - Array Representation: For node at index i:
 *   - Left child: 2*i + 1
 *   - Right child: 2*i + 2
 *   - Parent: (i-1)/2
 *   
 * Trade-offs:
 * - Pros: Guaranteed O(n log n), in-place sorting, no worst-case degradation
 * - Cons: Not stable, poor cache locality compared to quicksort
 */

int[] arr = { 9, 7, 1, 10, 15, 17, 4, 6, 7, 3 };

Console.WriteLine($"Unsorted: [{string.Join(", ", arr)}]");
HeapSort.Sort(arr);
Console.WriteLine($"Sorted: [{string.Join(", ", arr)}]");

class HeapSort
{
    public static void Sort(int[] arr)
    {
        int n = arr.Length;

        for (int i = n / 2 - 1; i >= 0; i--)
        {
            BubbleDown(arr, i, n);
        }

        for (int i = n - 1; i >= 0; i--)
        {
            (arr[0], arr[i]) = (arr[i], arr[0]);

            BubbleDown(arr, 0, i);
        }
    }

    private static void BubbleDown(int[] arr, int i, int n)
    {
        int leftChild = 2 * i + 1;
        int rightChild = 2 * i + 2;

        int highest = i;

        if (leftChild < n && arr[leftChild] > arr[highest])
        {
            highest = leftChild;
        }

        if (rightChild < n && arr[rightChild] > arr[highest])
        {
            highest = rightChild;
        }

        if (highest != i)
        {
            (arr[highest], arr[i]) = (arr[i], arr[highest]);

            BubbleDown(arr, highest, n);
        }
    }
}

