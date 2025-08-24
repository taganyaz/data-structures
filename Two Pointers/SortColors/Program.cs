// See https://aka.ms/new-console-template for more information
/**
Problem:
Write a function to sort a given integer array nums in-place (and without the built-in sort function), where the array contains n integers that are either 0, 1, and 2 and represent the colors red, white, and blue. Arrange the objects so that same-colored ones are adjacent, in the order of red, white, and blue (0, 1, 2).
Example
Input: nums = [2,1,2,0,1,0,1,0,1]
Output: [0,0,0,1,1,1,1,2,2]

Approach:
- Use two pointers move through the array (left, right)
- Use additional marker for next redIndex
- While left < right
- If arr[left] == 2, swap arr[left], arr[right], then decrement right by 1
- If arr[left] == 0, swap arr[left], arr[nextRedIndex], the increment left and  nextRedIndex by 1
- Esle increment left by 1

**/
int[] nums = { 2, 1, 2, 0, 1, 0, 1, 0, 1 };
Console.WriteLine($"Unsorted: [{string.Join(", ", nums)}]");

SortColors.Sort(nums);

Console.WriteLine($"Sorted: [{string.Join(", ", nums)}]");
class SortColors
{
    public static void Sort(int[] colors)
    {
        if (colors == null || colors.Length == 1)
            return;

        int n = colors.Length;
        int nextRedIndex = 0;
        int left = 0;
        int right = n - 1;

        while (left <= right)
        {
            if (colors[left] == 2)
            {
                (colors[left], colors[right]) = (colors[right], colors[left]);
                right--;
            }
            else if (colors[left] == 0)
            {
                (colors[left], colors[nextRedIndex]) = (colors[nextRedIndex], colors[left]);
                nextRedIndex++;
                left++;
            }
            else
            {
                left++;
            }
        }
    }
}
