// See https://aka.ms/new-console-template for more information
/*
 * Problem: Letter Combinations of a Phone Number
 * 
 * Given a string containing digits from 2-9 inclusive, return all possible letter combinations 
 * that the number could represent. Return the answer in any order.
 * 
 * A mapping of digit to letters (just like on the telephone buttons) is given below. 
 * Note that 1 does not map to any letters.
 * 
 * 2: "abc"    3: "def"    4: "ghi"    5: "jkl"
 * 6: "mno"    7: "pqrs"   8: "tuv"    9: "wxyz"
 * 
 * Example:
 * Input: "23"
 * Output: ["ad", "ae", "af", "bd", "be", "bf", "cd", "ce", "cf"]
 * 
 * ===== Solution Approach: Backtracking with DFS =====
 * 
 * Key Insights:
 * 1. This is a combinatorial problem where we need to generate all possible combinations
 * 2. Each digit maps to multiple letters, creating a branching factor at each position
 * 3. The total number of combinations = product of letter counts for each digit
 *    Example: "23" → 3 letters × 3 letters = 9 combinations
 * 
 * Algorithm Design:
 * - Use backtracking to explore all possible letter combinations
 * - Maintain a path (current combination being built) using StringBuilder for O(1) append/remove
 * - At each recursive level, we process one digit position
 * - Branch into all possible letters for the current digit
 * 
 * Implementation Details:
 * 1. Base Case: When we've processed all digits (index == digits.Length), 
 *    add the current path to results
 * 2. Recursive Case: For each letter mapped to current digit:
 *    - Append letter to path
 *    - Recursively process next digit
 *    - Backtrack by removing the letter (crucial for exploring other branches)
 * 
 * Time Complexity: O(N × 4^N) where N is the length of input digits
 * - Worst case: all digits map to 4 letters (e.g., "7" or "9")
 * - We generate 4^N combinations, each taking O(N) to construct the string
 * 
 * Space Complexity: O(N) for recursion stack and path storage
 * - Maximum recursion depth equals input length
 * - StringBuilder reused across all combinations
 * 
 * Optimization Techniques Used:
 * 1. StringBuilder instead of string concatenation to avoid O(N²) string operations
 * 2. Pre-sized StringBuilder to avoid dynamic resizing
 * 3. Direct character arithmetic (digits[index] - '0') for digit conversion
 * 4. Static readonly dictionary for constant-time letter lookups
 */

using System.Text;

Console.WriteLine("23: [" + string.Join(", ", new Solution().GetLetterCombinations("23")) + "]");
Console.WriteLine("489: [" + string.Join(", ", new Solution().GetLetterCombinations("489")) + "]");
class Solution
{
    private static readonly Dictionary<int, string> DIGITS_MAP = new(){
        {2, "abc" },
        {3, "def" },
        {4, "ghi" },
        {5, "jkl" },
        {6, "mno" },
        {7, "pqrs" },
        {8, "tuv" },
        {9, "wxyz" }

        };


    public List<string> GetLetterCombinations(string digits)
    {
        var results = new List<string>();

        if (digits == null || digits.Length == 0)
            return results;

        FindCombinations(digits, results, new StringBuilder(digits.Length), 0);

        return results;
    }

    private void FindCombinations(string digits, List<string> paths, StringBuilder path, int index)
    {
        if (index == digits.Length)
        {
            paths.Add(path.ToString());
            return;
        }

        foreach (char letter in DIGITS_MAP[digits[index] - '0'])
        {
            path.Append(letter);

            FindCombinations(digits, paths, path, index + 1);

            path.Length--;
        }
    }
}

