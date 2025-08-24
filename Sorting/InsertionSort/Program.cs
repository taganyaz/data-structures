// See https://aka.ms/new-console-template for more information
/**
 * PROBLEM: Insertion Sort Implementation
 * 
 * Given an unsorted array of integers, implement the insertion sort algorithm 
 * to sort the array in ascending order.
 * 
 * ALGORITHM OVERVIEW:
 * Insertion sort builds the final sorted array one element at a time by repeatedly
 * inserting each element into its correct position among the previously sorted elements.
 * It's analogous to how most people sort playing cards in their hands.
 * 
 * APPROACH:
 * The algorithm maintains two logical partitions within the array:
 * 1. Sorted partition: Elements from index 0 to i-1 (left side)
 * 2. Unsorted partition: Elements from index i to n-1 (right side)
 * 
 * ALGORITHM STEPS:
 * 1. Start with the second element (index 1) as the first element is trivially sorted
 * 2. For each element in the unsorted partition:
 *    a. Store the current element as 'key' (the element to be inserted)
 *    b. Compare key with elements in the sorted partition, moving right-to-left
 *    c. Shift elements greater than key one position to the right
 *    d. Insert key at the position where no more elements need shifting
 * 
 * TIME COMPLEXITY:
 * - Best Case: O(n) - When array is already sorted (only comparisons, no shifts)
 * - Average Case: O(n²) - Random order requires quadratic comparisons and shifts
 * - Worst Case: O(n²) - When array is sorted in reverse order
 * 
 * SPACE COMPLEXITY: O(1) - In-place sorting algorithm
 * 
 * CHARACTERISTICS:
 * - Stable: Maintains relative order of equal elements
 * - In-place: Requires only O(1) additional memory
 * - Adaptive: Performs well on data that is already substantially sorted
 * - Online: Can sort a list as it receives it
 * 
 * USE CASES:
 * - Small datasets (n < 50)
 * - Nearly sorted data
 * - Online algorithm requirements (sorting data as it arrives)
 * - When simplicity is preferred over efficiency for small inputs
 * 
 * EXAMPLE:
 * Input:  [1, 9, 4, 2, 10, 3, 5, 7]
 * Output: [1, 2, 3, 4, 5, 7, 9, 10]
 * 
 * Trace:
 * i=1: [1, 9, 4, 2, 10, 3, 5, 7] - 9 stays in place
 * i=2: [1, 4, 9, 2, 10, 3, 5, 7] - 4 inserted between 1 and 9
 * i=3: [1, 2, 4, 9, 10, 3, 5, 7] - 2 inserted between 1 and 4
 * i=4: [1, 2, 4, 9, 10, 3, 5, 7] - 10 stays in place
 * i=5: [1, 2, 3, 4, 9, 10, 5, 7] - 3 inserted between 2 and 4
 * i=6: [1, 2, 3, 4, 5, 9, 10, 7] - 5 inserted between 4 and 9
 * i=7: [1, 2, 3, 4, 5, 7, 9, 10] - 7 inserted between 5 and 9
 */

var arr = new int[] { 1, 9, 4, 2, 10, 3, 5, 7 };
Console.WriteLine($"Unsorted: [{string.Join(", ", arr)}]");
Solution.Sort(arr);
Console.WriteLine($"Sorted: [{string.Join(", ", arr)}]");

class Solution
{
    /// <summary>
    /// Sorts an array in-place using the insertion sort algorithm.
    /// </summary>
    /// <param name="array">The array to be sorted</param>
    public static void Sort(int[] array)
    {
        // Iterate through unsorted portion starting from index 1
        for (int i = 1; i < array.Length; i++)
        {
            // Store current element to be inserted into sorted portion
            int key = array[i];
            
            // Start comparing from the rightmost element of sorted portion
            int j = i - 1;
            
            // Shift elements greater than key to the right
            while (j >= 0 && array[j] > key)
            {
                array[j + 1] = array[j]; // Shift element one position right
                j--;
            }
            
            // Insert key at its correct position in sorted portion
            array[j + 1] = key;
        }
    }
}
var arr = new int[] { 1, 9, 4, 2, 10, 3, 5, 7 };
Console.WriteLine($"Unsorted: [{string.Join(", ", arr)}]");
Solution.Sort(arr);
Console.WriteLine($"Sorted: [{string.Join(", ", arr)}]");
class Solution
{
    public static void Sort(int[] array)
    {
        for (int i = 1; i < array.Length; i++)
        {
            int key = array[i];
            int j = i - 1;
            while (j >= 0 && array[j] > key)
            {
                array[j + 1] = array[j]; // Shift right
                j--;
            }
            array[j + 1] = key; // insert key at correct position
        }
    }
}
