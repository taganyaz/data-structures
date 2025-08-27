// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Write a function to find the length of the longest substring containing the same letter in a given string s, after performing at most k operations in which you can choose any character of the string and change it to any other uppercase English letter.
Example
Input:
s = "BBABCCDD"
k = 2
Output: 5

Approach:
- Use sliding window strategy
- Keep track of window start and maxLength seen so far, and maxFreq
- Use a dictionary to track character occurrence count
- For each character
    - Add to dictionary
    - If occurence count - k == window size, update length
    - Else remove characters from dictionary until occurence count - k == window size
**/
string s = "AABABBA";
int k = 1;
Console.WriteLine($"Max Length: {LongestRepeatingChar.FindLength(s, k)}");
// Debug.Assert(4 < 2, "False", "test failed");
class LongestRepeatingChar
{

    public static int FindLength(string s, int k)
    {
        if (string.IsNullOrEmpty(s))
        {
            return 0;
        }
        int maxLength = 0;
        int maxFreq = 0;
        int start = 0;
        Dictionary<char, int> seen = new Dictionary<char, int>();

        for (int end = 0; end < s.Length; end++)
        {
            char c = s[end];
            seen[c] = seen.GetValueOrDefault(c, 0) + 1;
            maxFreq = Math.Max(maxFreq, seen[c]);

            while (end - start + 1 - maxFreq > k)
            {
                seen[s[start]]--;
                start++;
            }

            maxLength = Math.Max(maxLength, end - start + 1);
        }

        return maxLength;
    }
}
