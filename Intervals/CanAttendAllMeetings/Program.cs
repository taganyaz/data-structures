// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Write a function to check if a person can attend all the meetings scheduled without any time conflicts. Given an array intervals, where each element [s1, e1] represents a meeting starting at time s1 and ending at time e1, determine if there are any overlapping meetings. If there is no overlap between any meetings, return true; otherwise, return false.
Note that meetings ending and starting at the same time, such as (0,5) and (5,10), do not conflict.
Example 1
Input: intervals = [(1,5),(3,9),(6,8)]
Output: false

Example 2
Input: intervals = [(10,12),(6,9),(13,15)]
Output: true

Approach:
- Sort meeting by end time
- Traverse each meeting in the list from second meeting
- If current meeting start is less than prev meeting end return false
- When we reach end of array return true
**/
RunTests();

static void RunTests()
{
    var conflictingMeetings = new List<(int start, int end)> { (1, 5), (3, 9), (6, 8) };
    var nonConflictingMeetings = new List<(int start, int end)> { (10, 12), (6, 9), (13, 15) };
    
    Debug.Assert(!MeetingScheduler.CanAttendAll(conflictingMeetings), "Should detect conflicts");
    Debug.Assert(MeetingScheduler.CanAttendAll(nonConflictingMeetings), "Should allow non-conflicting meetings");
    
    // Edge case tests
    Debug.Assert(MeetingScheduler.CanAttendAll(new List<(int, int)>()), "Empty list should return true");
    Debug.Assert(MeetingScheduler.CanAttendAll(new List<(int, int)> { (1, 2) }), "Single meeting should return true");
    Debug.Assert(MeetingScheduler.CanAttendAll(new List<(int, int)> { (0, 5), (5, 10) }), "Adjacent meetings should not conflict");
    
    Console.WriteLine("All tests passed!");
}

class MeetingScheduler
{
    public static bool CanAttendAll(List<(int start, int end)> intervals)
    {
        if (intervals == null || intervals.Count < 2)
        {
            return true;
        }

        intervals.Sort((a, b) => a.start - b.start);

        for (int i = 1; i < intervals.Count; i++)
        {
            if (intervals[i].start < intervals[i - 1].end)
            {
                return false;
            }
        }

        return true;
    }

    public static bool CanAttendAll2(List<(int start, int end)> intervals)
    {
        if (intervals == null || intervals.Count < 2)
        {
            return true;
        }

        List<Interval> meetings = new List<Interval>();

        foreach (var interval in intervals)
        {
            meetings.Add(new(interval.start, interval.end));
        }

        meetings.Sort();

        for (int i = 1; i < meetings.Count; i++)
        {
            if (meetings[i].Start < meetings[i - 1].End)
            {
                return false;
            }
        }
        return true;
    }

    public static bool CanAttendAll3(List<(int start, int end)> intervals)
    {
        if (intervals == null || intervals.Count < 2)
        {
            return true;
        }

        intervals.Sort((a, b) => a.start.CompareTo(b.start));

        for (int i = 1; i < intervals.Count; i++)
        {
            if (intervals[i].start < intervals[i - 1].end)
            {
                return false;
            }
        }
        return true;
    }
}

class Interval : IComparable<Interval>
{
    public Interval(int start, int end)
    {
        Start = start;
        End = end;
    }

    public int Start { get; private set; }
    public int End { get; private set; }
    public int CompareTo(Interval? other)
    {
        return Start.CompareTo(other?.Start);
    }
}
