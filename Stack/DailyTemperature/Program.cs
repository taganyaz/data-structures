// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem: 
Given an integer array temps representing daily temperatures, write a function to calculate the number of days one has to wait for a warmer temperature after each given day. The function should return an array answer where answer[i] represents the wait time for a warmer day after the ith day. If no warmer day is expected in the future, set answer[i] to 0.
Example
Inputs: temps = [65, 70, 68, 60, 55, 75, 80, 74]
Output: [1,4,3,2,1,1,0,0]

Approach:
- Use a stack to track index of all days waiting to get a warmer day
- Initialize a result array with zeros
- For each element
    - While stack is not empty and top of stack day temp < current temp, pop the stack and update the result[stack.Pop()] = current temp
    - Push current index to the stack
- Rteurn result
**/
int[] temps1 = [65, 70, 68, 60, 55, 75, 80, 74];
int[] output1 = [1, 4, 3, 2, 1, 1, 0, 0];
int[] result1 = TemperatureTracker.DailyTemperatureWait(temps1);


int[] temps2 = [73, 74, 75, 71, 69, 72, 76, 73];
int[] output2 = [1, 1, 4, 2, 1, 1, 0, 0];
int[] result2 = TemperatureTracker.DailyTemperatureWait(temps2);

int[] temps3 = [30,40,50,60];
int[] output3 = [1,1,1,0];
int[] result3 = TemperatureTracker.DailyTemperatureWait(temps3);


Debug.Assert(result1.SequenceEqual(output1), "Test 1: Should return correct output");
Debug.Assert(result2.SequenceEqual(output2), "Test 2: Should return correct output");
Debug.Assert(result3.SequenceEqual(output3), "Test 3: Should return correct output");

Console.WriteLine("All tests passed!");
class TemperatureTracker
{
    public static int[] DailyTemperatureWait(int[] temps)
    {
        if (temps == null || temps.Length == 0)
            return Array.Empty<int>();

        int n = temps.Length;
        int[] result = new int[n];

        Array.Fill(result, 0);

        Stack<int> stack = new Stack<int>();

        for (int i = 0; i < n; i++)
        {
            while (stack.Count > 0 && temps[i] > temps[stack.Peek()])
            {
                int prevIndex = stack.Pop();
                result[prevIndex] = i - prevIndex;
            }
            stack.Push(i);
        }

        return result;
    }
}
