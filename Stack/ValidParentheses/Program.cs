// See https://aka.ms/new-console-template for more information


using System.Diagnostics;

/**
Problem:
Given an input string s consisting solely of the characters '(', ')', '{', '}', '[' and ']', determine whether s is a valid string. A string is considered valid if every opening bracket is closed by a matching type of bracket and in the correct order, and every closing bracket has a corresponding opening bracket of the same type.
Example 1:

Inputs: s = "(){({})}"
Output: True

Example 2:

Inputs: s = "(){({}})"
Output: False

Approach:
- Use a stack to check if all the parentheses types have been correctly opened and closed
- Traverse the string character by character
- If you meet an opening brace, push it to the stack
- If you meet a closing brace
    - if stack is empty, return false
    - if top of stack is a matching opening brace, pop
    - else return false
**/
string validString = "(){({})}";
string invalidString = "(){({}})";

Debug.Assert(ParenthesesValidator.Validate(validString), "Should return true for valid string");
Debug.Assert(!ParenthesesValidator.Validate(invalidString), "Should return false for invalid string");

Console.WriteLine("All tests have passed!");
class ParenthesesValidator
{
    public static bool Validate(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return true;
        }

        if (s.Length % 2 != 0)
        {
            return false;
        }

        HashSet<char> openingBraces = new HashSet<char> { '{', '[', '(' };
        Dictionary<char, char> expectedMatches = new Dictionary<char, char>
        {
            {'}', '{'},
            {']', '['},
            {')', '('}
        };

        Stack<char> stack = new Stack<char>(s.Length / 2);

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];

            if (openingBraces.Contains(c))
            {
                stack.Push(c);
            }
            else if (expectedMatches.TryGetValue(c, out char expectedOpening))
            {
                if (stack.Count == 0 || stack.Pop() != expectedOpening)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return stack.Count == 0;
    }
}
