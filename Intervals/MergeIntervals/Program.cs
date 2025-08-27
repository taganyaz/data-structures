// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Write a function to consolidate overlapping intervals within a given array intervals, where each interval intervals[i] consists of a start time starti and an end time endi.
Two intervals are considered overlapping if they share any common time, including if one ends exactly when another begins (e.g., [1,4] and [4,5] overlap and should be merged into [1,5]).
The function should return an array of the merged intervals so that no two intervals overlap and all the intervals collectively cover all the time ranges in the original input.
Example
Input: intervals = [[3,5],[1,4],[7,9],[6,8]]
Output: [[1,5],[6,9]]

Appraoch:
- Sort the intervals by start time
- Initialize nextStart and nextEnd to the start and end of the first interval respectively
- Traverse the intervals from second position
    - If start of current interval is greater that the nextEnd time
        a) add new interval to the result with nextStart and nextEnd as as start and end time respectively
        b) update the nextStart and nextEnd to current interval start and end
    - Else, current iterval overlap with previous, so update nextEnd to max(nextEnd, currentIntervalEnd)
**/
int[][] intervals = [[3, 5], [1, 4], [7, 9], [6, 8]];
List<(int start, int end)> output1 = [(1, 5), (6, 9)];
var result1 = IntervalManager.Merge(intervals);

int[][] intervals2 = [[1, 3], [2, 6], [8, 10], [15, 18]];
List<(int start, int end)> output2 = [(1, 6), (8, 10), (15, 18)];
var result2 = IntervalManager.Merge(intervals2);

int[][] intervals3 = [[1, 4], [4, 5]];
List<(int start, int end)> output3 = [(1, 5)];
var result3 = IntervalManager.Merge(intervals3);

Debug.Assert(result1.SequenceEqual(output1), "Test 1: Should return correct number of intervals after merge");
Debug.Assert(result2.SequenceEqual(output2), "Test 2: Should return correct number of intervals after merge");
Debug.Assert(result3.SequenceEqual(output3), "Test 3: Should return correct number of intervals after merge");

Console.WriteLine("All tests have passed.");
class IntervalManager
{

    public static List<(int start, int end)> Merge(int[][] intervals)
    {
        if (intervals == null || intervals.Length == 0)
        {
            return new List<(int start, int end)>();
        }

        if (intervals.Length == 1)
        {
            return new List<(int start, int end)> { (intervals[0][0], intervals[0][1]) };
        }

        int n = intervals.Length;
        Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0]));

        int nextStart = intervals[0][0];
        int nextEnd = intervals[0][1];
        List<(int start, int end)> result = new List<(int start, int end)>();

        for (int i = 1; i < n; i++)
        {
            if (intervals[i][0] <= nextEnd)// merge
            {
                nextEnd = Math.Max(nextEnd, intervals[i][1]);
            }
            else
            {
                result.Add((nextStart, nextEnd));
                nextStart = intervals[i][0];
                nextEnd = intervals[i][1];
            }
        }

        result.Add((nextStart, nextEnd));

        return result;

    }
}
