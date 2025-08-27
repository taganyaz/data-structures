// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Write a function to find the common free time for all employees from a list called schedule. Each employee's schedule is represented by a list of non-overlapping intervals sorted by start times. The function should return a list of finite, non-zero length intervals where all employees are free, also sorted in order.
Example
Input: schedule = [[[2,4],[7,10]],[[1,5]],[[6,9]]]
Output: [(5,6)]

Approach:
- Consolidate all the schedule and sort them
- Merge overlapping schedule
- Find gaps between merged intervals
**/
int[][][] schedule1 = [[[2, 4], [7, 10]], [[1, 5]], [[6, 9]]];
List<(int start, int end)> output1 = [(5, 6)];
var result1 = ScheduleManager.GetCommonFreeTime(schedule1);


Debug.Assert(result1.SequenceEqual(output1), "Test 1: Should return correct common free times");

Console.WriteLine("All test passed!");
class ScheduleManager
{
    public static List<(int start, int end)> GetCommonFreeTime(int[][][] schedules)
    {
        if (schedules == null || schedules.Length == 0)
        {
            return new List<(int start, int end)>();
        }

        List<(int start, int end)> allSchedules = new List<(int start, int end)>();
        List<(int start, int end)> merged = new List<(int start, int end)>();
        List<(int start, int end)> result = new List<(int start, int end)>();

        for (int i = 0; i < schedules.Length; i++)
        {
            for (int j = 0; j < schedules[i].Length; j++)
            {
                allSchedules.Add((schedules[i][j][0], schedules[i][j][1]));
            }
        }

        allSchedules.Sort((a, b) => a.start.CompareTo(b.start));
        var (nextStart, nextEnd) = allSchedules[0];

        for (int i = 1; i < allSchedules.Count; i++)
        {
            if (allSchedules[i].start <= nextEnd)
            {
                nextEnd = Math.Max(nextEnd, allSchedules[i].end);
            }
            else
            {
                merged.Add((nextStart, nextEnd));
                (nextStart, nextEnd) = allSchedules[i];
            }
        }

        merged.Add((nextStart, nextEnd));


        for (int i = 1; i < merged.Count; i++)
        {
            if (merged[i].start > merged[i - 1].end)
            {
                result.Add((merged[i - 1].end, merged[i].start));
            }
        }
        return result;
    }
}
