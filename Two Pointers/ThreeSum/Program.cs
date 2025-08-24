// See https://aka.ms/new-console-template for more information
/**
Problem: Three Sum (LeetCode #15)
Given an integer array nums, return all unique triplets [nums[i], nums[j], nums[k]] such that:
- i != j != k (distinct indices)
- nums[i] + nums[j] + nums[k] = 0
- The solution set must not contain duplicate triplets

Example:
Input: nums = [-1,0,1,2,-1,-1]
Output: [[-1,-1,2],[-1,0,1]]

Constraints:
- 3 <= nums.length <= 3000
- -10^5 <= nums[i] <= 10^5

Approach: Two-Pointer Technique with Sorting

Algorithm Overview:
1. Sort the array to enable two-pointer technique and duplicate handling
2. Fix one element and find pairs that sum to its negative using two pointers
3. Handle duplicates at each level to ensure unique triplets

Detailed Steps:
1. Sort the array - O(n log n)
   - Enables efficient two-pointer search
   - Groups duplicate values together for easy skipping
   
2. Iterate through array fixing first element - O(n)
   - Early termination: if arr[i] > 0, no valid triplets possible (sorted array)
   - Skip duplicate first elements to avoid duplicate triplets
   
3. For each fixed element, use two pointers - O(n)
   - left pointer starts at i+1, right pointer at end
   - Target sum: -arr[i] (so arr[i] + arr[left] + arr[right] = 0)
   - If sum equals target: found valid triplet
     - Skip duplicate values for both pointers
   - If sum < target: increment left (need larger values)
   - If sum > target: decrement right (need smaller values)

Time Complexity: O(n²)
- Sorting: O(n log n)
- Nested loops: O(n) × O(n) = O(n²)
- Overall: O(n log n + n²) = O(n²)

Space Complexity: O(1) or O(n)
- O(1) if we don't count output array
- O(n) for the output array in worst case
- Sorting space depends on implementation (typically O(log n))

Edge Cases:
- Array with less than 3 elements
- All positive or all negative numbers
- Array with all duplicates
- No valid triplets exist

Alternative Approaches:
1. HashSet approach: Fix two elements, check if -(a+b) exists - O(n²) time, O(n) space
2. Brute force: Check all triplets - O(n³) time, O(1) space
**/
int[] nums = { -1, 0, 1, 2, -1, -1 };
var result = ThreeSum.FindTripletsSumToZero(nums);

Console.WriteLine($"Result: [{string.Join(" ,", result.Select(s => $"[{string.Join(", ", s)}]").ToArray())}]");
class ThreeSum
{
    public static List<List<int>> FindTripletsSumToZero(int[] arr)
    {
        if (arr == null || arr.Length < 3)
            return [[]];

        List<List<int>> result = new();
        Array.Sort(arr);
        int n = arr.Length;

        for (int i = 0; i < n - 2; i++)
        {
            if (arr[i] > 0)
            {
                break;
            }
            if (i > 0 && arr[i] == arr[i - 1])
                {
                    continue;
                }

            int left = i + 1;
            int right = n - 1;

            while (left < right)
            {
                int sum = arr[i] + arr[left] + arr[right];

                if (sum == 0)
                {
                    result.Add(new() { arr[i], arr[left], arr[right] });

                    while (left < right && arr[left] == arr[left + 1])
                    {
                        left++;
                    }

                    while (left < right && arr[right] == arr[right - 1])
                    {
                        right--;
                    }

                    left++;
                    right--;
                }
                else if (sum < 0)
                {
                    left++;
                }
                else
                {
                    right--;
                }
            }
        }

        return result;

    }
}
