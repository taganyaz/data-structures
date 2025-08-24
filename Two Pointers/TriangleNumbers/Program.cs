// See https://aka.ms/new-console-template for more information
/**
Problem:
Write a function to count the number of triplets in an integer array nums that could form the sides of a triangle. The triplets do not need to be unique.
Input: nums = [11,4,9,6,15,18]
Output: 10

Approach:
- Sort the array to use two pointers
- Starting from end, fix the first element
- Use two pointers, right = i -1, and left = 0 to find sum greater than current first item
- If sum is fount, increment the count of triplets count += right - left and decrement right by 1
- Else increment left by 1


**/
int[] nums = { 4,2,3,4 };
Console.WriteLine($"Triplets to triangle: {TriangleNumber.CountTriplets(nums)}");
class TriangleNumber
{
    public static int CountTriplets(int[] arr)
    {
        if (arr == null || arr.Length < 3)
            return 0;

        Array.Sort(arr);
        int n = arr.Length;
        int count = 0;

        for (int i = n - 1; i >= 2; i--)
        {
            int left = 0;
            int right = i - 1;

            while (left < right)
            {
                if (arr[left] + arr[right] > arr[i])
                {
                    count += right - left;
                    right--;
                }
                else
                {
                    left++;
                }
            }
        }

        return count;
    }
}
