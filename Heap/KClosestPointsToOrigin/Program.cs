// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Given a list of points in the form [[x1, y1], [x2, y2], ... [xn, yn]] and an integer k, find the k closest points to the origin (0, 0) on the 2D plane.
The distance between two points (x, y) and (a, b) is calculated using the formula:
√(x - a)2 + (y - b)2
Return the k closest points in any order.

Example 1:
Inputs: points = [[3,4],[2,2],[1,1],[0,0],[5,5]], k = 3
Output:[[2,2],[1,1],[0,0]]

Example 2:
Input: points = [[3,3],[5,-1],[-2,4]], k = 2
Output: [[3,3],[-2,4]]
Explanation: The answer [[-2,4],[3,3]] would also be accepted.

Approach:
- Use a max heap to store the k closet points
- Initialize a maxHeap (use x2 + y2 as priority)
**/
Debug.Assert(KClosestPoint.GetKClosest([[3, 4], [2, 2], [1, 1], [0, 0], [5, 5]], 3)
        .OrderBy(p => p.x).ThenBy(p => p.y)
    .SequenceEqual(((List<(int x, int y)>)[(2, 2), (1, 1), (0, 0)])
        .OrderBy(p => p.x).ThenBy(p => p.y)));

Debug.Assert(KClosestPoint.GetKClosest([[3,3],[5,-1],[-2,4]], 2)
        .OrderBy(p => p.x).ThenBy(p => p.y)
    .SequenceEqual(((List<(int x, int y)>)[(3, 3), (-2, 4)])
        .OrderBy(p => p.x).ThenBy(p => p.y)));

Console.WriteLine("All tests have passed!");
class KClosestPoint
{
    public static List<(int x, int y)> GetKClosest(int[][] points, int k)
    {
        if (points == null || points.Length == 0)
            return [];

        PriorityQueue<int[], int> maxHeap = new PriorityQueue<int[], int>();
        List<int[]> result = new List<int[]>();

        foreach (var point in points)
        {
            maxHeap.Enqueue(point, -Distance(point));

            if (maxHeap.Count > k)
            {
                maxHeap.Dequeue();
            }
        }

        while (maxHeap.Count > 0)
        {
            result.Add(maxHeap.Dequeue());
        }
        return result.Select(p => (p[0], p[1])).ToList();
    }

    private static int Distance(int[] point)
    {
        return point[0] * point[0] + point[1] * point[1];
    }
}
