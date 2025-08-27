// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Given a string containing just the characters '(' and ')', find the length of the longest valid (well-formed) parentheses substring. A well-formed parentheses string is one that follows these rules:
Open brackets must be closed by a matching pair in the correct order.
For example, given the string "(()", the longest valid parentheses substring is "()", which has a length of 2. Another example is the string ")()())", where the longest valid parentheses substring is "()()", which has a length of 4.

Example 1:
Inputs: s = "())))"
Output: 2
(Explanation: The longest valid parentheses substring is "()")

Example 2:
Inputs: s = "((()()())"
Output:8
(Explanation: The longest valid parentheses substring is "(()()())" with a length of 8)

Example 3:
Inputs: s = ""
Output: 0

Approach:
- Use a stack to keep track of the index of the last unmatched (
- Track the maxLength so far
- Traverse the string from left to right
- If we encounter (, push current index to the stack
- If we encounter ), pop the stack
    - If stack is empty, no unmatched (, push current index to stack
    - Else update maxLength = Max(maxLength, i - stack.Peek())

**/
string s1 = "())))";
int output1 = 2;
int result1 = ParenthesesValidator.GetLongestValid(s1);

string s2 = "((()()())";
int output2 = 8;
int result2 = ParenthesesValidator.GetLongestValid(s2);

string s3 = "";
int output3 = 0;
int result3 = ParenthesesValidator.GetLongestValid(s3);

Debug.Assert(result1 == output1, $"Test 1: Expected: {output1}, Actual: {result1}");
Debug.Assert(result2 == output2, $"Test 2: Expected: {output2}, Actual: {result2}");
Debug.Assert(result3 == output3, $"Test 3: Expected: {output3}, Actual: {result3}");

Console.WriteLine("All tests passed!");

class ParenthesesValidator
{
    public static int GetLongestValid(string s)
    {
        if (string.IsNullOrEmpty(s) || s.Length == 1)
        {
            return 0;
        }

        Stack<int> stack = new Stack<int>();
        stack.Push(-1);

        int maxLength = 0;

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];

            if (c == '(')
            {
                stack.Push(i);
            }
            else
            {
                stack.Pop();
                if (stack.Count == 0)
                {
                    stack.Push(i);
                }
                else
                {
                    maxLength = Math.Max(maxLength, i - stack.Peek());
                }
            }
        }

        return maxLength;
    }
}
