// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Given a list of intervals intervals and an interval newInterval, write a function to insert newInterval into a list of existing, non-overlapping, and sorted intervals based on their starting points. The function should ensure that after the new interval is added, the list remains sorted without any overlapping intervals, merging them if needed.
Example
Input:
intervals = [[1,3],[6,9]]
newInterval = [2,5]
Output: [[1,5],[6,9]]

Approach:
- Traverse through the intervals from first interval
- Add all intervals whose end < newInterval start to result
- Set the start of next interval to the start of first overlapping interval
- Skipp (merge) all the intervals whose end < newinterval end
- Set the end of the next interval as end of next overlapping interval
- Add all intervals whose start > new interval end
**/
List<(int, int)> intervals = [(1, 3), (6, 9)];
(int, int) newInterval = (2, 5);
List<(int, int)> output = [(1, 5), (6, 9)];
var result = IntervalManager.Insert(intervals, newInterval);
Debug.Assert(result[0] == output[0], "Should insert new interval correctly");
Debug.Assert(result[1] == output[1], "Should insert new interval correctly");

Console.WriteLine("All test passed!");

class IntervalManager
{
    public static List<(int start, int end)> Insert(List<(int start, int end)> intervals, (int start, int end) newInterval)
    {
        // Edge case
        if (intervals == null || intervals.Count == 0)
        {
            return new List<(int start, int end)> { (newInterval.start, newInterval.end) };
        }

        int n = intervals.Count;
        int i = 0;
        List<(int start, int end)> result = new List<(int start, int end)>();

        // Add all intervals ending before newinterval start

        while (i < n && intervals[i].end < newInterval.start)
        {
            result.Add((intervals[i].start, intervals[i].end));
            i++;
        }

        // Find start and end of merged
        int mergedStart = newInterval.start;
        int mergedEnd = newInterval.end;

        while (i < n && intervals[i].start <= newInterval.end)
        {
            mergedStart = Math.Min(mergedStart, intervals[i].start);
            mergedEnd = Math.Max(mergedEnd, intervals[i].end);
            i++;
        }
        result.Add((mergedStart, mergedEnd));

        // Add all intervals starting after newinterval end
        while (i < n)
        {
            result.Add((intervals[i].start, intervals[i].end));
            i++;
        }

        return result;
    }
}
