// See https://aka.ms/new-console-template for more information
/*
Problem: Course Schedule II (Topological Sort)
You have to take a total of numCourses courses, which are labeled from 0 to numCourses - 1. You are given a list of prerequisite pairs, where prerequisites[i] = [a, b] indicates that you must complete course b before course a.
Given the total number of courses and a list of prerequisite pairs, write a function to return the ordering of courses you should take to finish all courses.
If there are multiple valid orderings, return any valid ordering. If it is impossible to finish all courses, return an empty array.

Example 1:
Input:
numCourses = 4
prerequisites = [[1,0], [2,0], [3,1], [3,2]]
Output: [0, 1, 2, 3] or [0, 2, 1, 3]
Explanation: There are a couple of ways to complete all courses, one possible order is [0, 1, 2, 3] and another is [0, 2, 1, 3].

Example 2:
Input:
numCourses = 2
prerequisites = [[1, 0], [0, 1]]
Output: []
Explanation: There's a cycle in prerequisites, making it impossible to complete all courses.

Approach: Kahn's Algorithm (BFS-based Topological Sort)

Key Insights:
1. This is a classic topological sorting problem on a Directed Acyclic Graph (DAG)
2. Courses represent vertices, prerequisites represent directed edges
3. A valid schedule exists if and only if the graph is acyclic
4. We use in-degree counting to detect cycles and build the ordering

Graph Representation:
- Adjacency List: prerequisite -> [dependent courses]
- Example: If course 1 requires course 0, then adjacencyList[0] contains 1

Algorithm Steps:
1. Build Graph Construction: O(P) where P = number of prerequisites
   - Initialize adjacency list for all courses
   - For each prerequisite pair (a, b): add edge from b to a

2. Calculate In-Degrees: O(V + E) where V = courses, E = prerequisites
   - In-degree[i] = number of prerequisites for course i
   - Courses with in-degree 0 have no prerequisites

3. BFS Traversal: O(V + E)
   - Initialize queue with all courses having in-degree 0
   - Process each course:
     a) Add to result list
     b) Reduce in-degree for all dependent courses
     c) Enqueue dependent courses that reach in-degree 0

4. Cycle Detection:
   - If result size < numCourses, a cycle exists
   - This means some courses still have unmet prerequisites

Time Complexity: O(V + E) where V = numCourses, E = prerequisites.length
Space Complexity: O(V + E) for adjacency list and auxiliary data structures

Edge Cases:
- No courses (numCourses = 0): Return empty list
- No prerequisites: Return any valid ordering (e.g., [0, 1, 2, ..., n-1])
- Self-loops: Course depends on itself
- Multiple valid orderings: Algorithm returns one valid ordering

Alternative Approaches:
1. DFS with cycle detection using colors (White-Gray-Black)
2. DFS with stack for post-order traversal (reverse gives topological order)
*/

var prerequisites1 = new List<(int, int)> { (1, 0), (2, 0), (3, 1), (3, 2) };
var prerequisites2 = new List<(int, int)> { (1, 0), (0, 1) };

var courseScheduler1 = new CourseScheduler(prerequisites1, 4);
var courseScheduler2 = new CourseScheduler(prerequisites2, 2);

Console.WriteLine($"Prerequisites 1 schedules: [{string.Join(", ", courseScheduler1.GetCoursesSchedule())}]");
Console.WriteLine($"Prerequisites 2 schedules: [{string.Join(", ", courseScheduler2.GetCoursesSchedule())}]");

public class CourseScheduler
{
    private readonly List<int>[] _adjacencyList;
    private readonly int _numCourses;

    public CourseScheduler(IReadOnlyList<(int, int)> prerequisites, int numCourses)
    {
        if (numCourses < 0)
        {
            throw new InvalidOperationException("Number of courses cannot be less than 0");
        }

        if (prerequisites == null)
        {
            throw new ArgumentNullException(nameof(prerequisites), "Prerequisites is required");
        }

        _numCourses = numCourses;
        _adjacencyList = new List<int>[_numCourses];

        BuildGraph(prerequisites);
    }

    private void BuildGraph(IReadOnlyList<(int, int)> prerequisites)
    {
        for (int i = 0; i < _numCourses; i++)
        {
            _adjacencyList[i] = [];
        }

        foreach (var (course, prerequisite) in prerequisites)
        {
            _adjacencyList[prerequisite].Add(course);
        }
    }

    public List<int> GetCoursesSchedule()
    {
        if (_numCourses == 0)
        {
            return [];
        }

        var inDegrees = new int[_numCourses];
        var result = new List<int>(_numCourses);
        var queue = new Queue<int>(_numCourses);

        foreach (var dependentCourses in _adjacencyList)
        {
            foreach (var dependentCourse in dependentCourses)
            {
                inDegrees[dependentCourse]++;
            }
        }

        for (int i = 0; i < _numCourses; i++)
        {
            if (inDegrees[i] == 0)
            {
                queue.Enqueue(i);
            }
        }

        while (queue.Count > 0)
        {
            var currentCourse = queue.Dequeue();
            result.Add(currentCourse);

            foreach (var dependentCourse in _adjacencyList[currentCourse])
            {
                if (--inDegrees[dependentCourse] == 0)
                {
                    queue.Enqueue(dependentCourse);
                }
            }
        }

        return (result.Count == _numCourses) ? result : [];
    }
}
