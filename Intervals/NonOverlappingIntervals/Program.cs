// See https://aka.ms/new-console-template for more information


using System.Diagnostics;

/**
Problem:
Write a function to return the minimum number of intervals that must be removed from a given array intervals, where intervals[i] consists of a starting point starti and an ending point endi, to ensure that the remaining intervals do not overlap.
Note that intervals which only touch at a point are non-overlapping. For example, [1, 2] and [2, 3] are non-overlapping.
Example
Input: intervals = [[1,3],[5,8],[4,10],[11,13]]
Output: 1

Approach:
- Find max number of non-overlapping intervals
- Sort intervals by ending time
- Use greedy algorithm to add earliest ending intervals to list of non-overlapping to give more room for other intervals
- Min no of intervals to remove = total - no. of non-overlapping
- Keep track of ending and count variable to track last ending for interval and number of non-overlapping intervals

**/

int[][] intervals = [[1, 3], [5, 8], [4, 10], [11, 13]];
int output = 1;

int[][] intervals2 = [[1, 2], [1, 2], [1, 2]];
int output2 = 2;

int[][] intervals3 = [[1,2],[2,3]];
int output3 = 0;

Debug.Assert(IntervalManager.EraseOverlapIntervals(intervals) == output, "Intervals: Should return correct number of intervals to remove");
Debug.Assert(IntervalManager.EraseOverlapIntervals(intervals2) == output2, "Intervals2: Should return correct number of intervals to remove");
Debug.Assert(IntervalManager.EraseOverlapIntervals(intervals3) == output3, "Intervals3: Should return correct number of intervals to remove");


Console.WriteLine("All tests passed");
class IntervalManager
{
    public static int EraseOverlapIntervals(int[][] intervals)
    {
        if (intervals == null || intervals.Length == 0)
        {
            return 0;
        }

        Array.Sort(intervals, (a, b) => a[1].CompareTo(b[1]));
        int n = intervals.Length;
        int end = intervals[0][1];
        int count = 1;

        for (int i = 1; i < n; i++)
        {
            if (intervals[i][0] >= end)
            {
                end = intervals[i][1];
                count++;
            }
        }

        return n - count;
    }
}
