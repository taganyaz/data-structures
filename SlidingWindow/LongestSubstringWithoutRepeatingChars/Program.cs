// See https://aka.ms/new-console-template for more information
/**
Problem:
Write a function to return the length of the longest substring in a provided string s where all characters in the substring are distinct.
Example 1: 
Input: s = "eghghhgg"
Output: 3

Approach:
- Use sliding window appraoch
- Use a dictionary to track characters and number of occurrence
- Initialize start to 0 to mark start of the window
- Traverse the string from left to right
- For each character
    - If character does not exist in the dictionary, add it and update maxLength = max(maxLength, end - start + 1)
    - Else increment the start by 1

**/
string s = "abcabcbb";
Console.WriteLine($"Longest substring: {LongestSubstring.FindLongestSubStringLength(s)}");
class LongestSubstring
{
    public static int FindLongestSubStringLength(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return 0;
        }

        int start = 0;
        int maxLength = 0;
        Dictionary<char, int> seen = new Dictionary<char, int>();

        for (int end = 0; end < s.Length; end++)
        {
            char c = s[end];

            seen[c] = seen.GetValueOrDefault(c, 0) + 1;

            while (seen[c] > 1)
            {
                seen[s[start]]--;
                start++;
            }

            maxLength = Math.Max(maxLength, end - start + 1);
        }

        return maxLength;
    }
}
