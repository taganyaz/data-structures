// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using System.Text;

/**
Problem:
Given an encoded string, write a function to return its decoded string that follows a specific encoding rule: k[encoded_string], where the encoded_string within the brackets is repeated exactly k times. Note that k is always a positive integer. The input string is well-formed without any extra spaces, and square brackets are properly matched. Also, assume that the original data doesn't contain digits other than the ones that specify the number of times to repeat the following encoded_string.
Example
Inputs: s = "3[a2[c]]"
Output: "accaccacc"

Approach:
- Use two stacks: 1 for numbers, and the other on for string
- Initialize curNumber = 0, and curString = ""
- Traverse string from left to right
    - When you meet a number, update curNumber  curNumber * 10 + number
    - When you meet a character, append it to curString
    - When you meet [, push curNumber, and curString to stacks, reset curNumber = 0 and curString = ""
    - When you meet ], pop curNumber stack, repeat curString curNumber times, pop string from stack and prepend to current string
**/
string s1 = "3[a2[c]]";
string output1 = "accaccacc";
string result1 = StringDecoder.Decode(s1);


Debug.Assert(result1 == output1, $"s1 should decode to the correct ouput. Expected {output1}, Actual: {result1}");

Console.WriteLine("All tests have passed!");
class StringDecoder
{
    public static string Decode(string s)
    {

        if (string.IsNullOrEmpty(s))
        {
            return s ?? string.Empty;
        }

        int curNum = 0;
        string curStr = "";
        Stack<int> numStack = new Stack<int>();
        Stack<string> strStack = new Stack<string>();

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];

            if (char.IsDigit(c))
            {
                curNum = curNum * 10 + (c - '0');
            }
            else if (c == '[')
            {
                numStack.Push(curNum);
                strStack.Push(curStr);

                curNum = 0;
                curStr = "";
            }
            else if (c == ']')
            {
                int repeatCount = numStack.Pop();
                string prevStr = strStack.Pop();

                StringBuilder temp = new StringBuilder(prevStr.Length + curStr.Length * repeatCount);
                temp.Append(prevStr);
                for (int j = 0; j < repeatCount; j++)
                {
                    temp.Append(curStr);
                }
                curStr = temp.ToString();
                curNum = 0;
            }
            else
            {
                curStr += c;
            }
        }

        return curStr;
    }
}


