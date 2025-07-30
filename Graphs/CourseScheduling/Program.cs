// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
/*
Problem: Course Schedule (Leetcode 207)
Given numCourses courses labeled from 0 to numCourses-1 and a list of prerequisite pairs where
prerequisites[i] = [ai, bi] indicates that course bi must be completed before course ai.
Determine if it's possible to complete all courses.

Example 1:
Input: numCourses = 2, prerequisites = [[1,0]]
Output: true
Explanation: Course 0 must be taken before course 1. Valid order: [0, 1]

Example 2:
Input: numCourses = 2, prerequisites = [[1,0],[0,1]]
Output: false
Explanation: Circular dependency - course 0 requires 1, and course 1 requires 0.

Analysis:
This is a classic cycle detection problem in a directed graph. The courses form vertices and 
prerequisites form directed edges. If there's a cycle, it's impossible to complete all courses.

Approach: Topological Sort using Kahn's Algorithm (BFS)
We use topological sorting to detect cycles. If we can generate a valid topological ordering
that includes all vertices, the graph is acyclic (DAG) and all courses can be completed.

Why this approach?
1. Time Complexity: O(V + E) where V = numCourses, E = prerequisites.length
2. Space Complexity: O(V + E) for adjacency list and auxiliary arrays
3. More intuitive than DFS for this problem as it naturally processes courses in valid order
4. Easier to extend for follow-up problems (e.g., finding the actual course order)

Algorithm:
1. Build adjacency list representation where graph[prerequisite] contains dependent courses
2. Calculate in-degree (number of prerequisites) for each course
3. Initialize queue with all courses having zero prerequisites (no dependencies)
4. Process queue using BFS:
   - Dequeue a course and mark it as completed
   - Reduce in-degree for all dependent courses
   - Enqueue courses whose in-degree becomes zero
5. If processed courses equals total courses, all can be completed (no cycles)

Alternative Approaches:
- DFS with color marking (White-Gray-Black) - Same complexity but more complex implementation
- Union-Find - Not suitable as it's designed for undirected graphs

Edge Cases:
- No courses (numCourses = 0)
- No prerequisites (all courses are independent)
- Self-loops (course depends on itself)
- Duplicate prerequisites
*/

var graph1 = new CourseScheduler(new  List<(int course, int prerequisite)> {(1, 0)}, 2);
var graph2 = new CourseScheduler(new List<(int, int)> { (1, 0), (0, 1)}, 2);

Console.WriteLine($"Can take all set 1 courses? {graph1.CanCompleteAllCourses()}");
Console.WriteLine($"Can take all set 2 courses? {graph2.CanCompleteAllCourses()}");

public class CourseScheduler
{
    private readonly List<int>[] _adjacencyList;
    private readonly int _numCourses;

    public CourseScheduler(IReadOnlyList<(int course, int prerequisite)> prerequisites, int numCourses)
    {
        if (numCourses < 0)
        {
            throw new InvalidOperationException("Number of courses cannot be less than 0.");
        }

        if (prerequisites == null)
        {
            throw new ArgumentNullException(nameof(prerequisites));
        }

        _numCourses = numCourses;
        _adjacencyList = new List<int>[_numCourses];

        // Build graph
        BuildGraph(prerequisites);
    }

    private void BuildGraph(IReadOnlyList<(int course, int prerequisite)> prerequisites)
    {
        for (int i = 0; i < _numCourses; i++)
        {
            _adjacencyList[i] = new List<int>();
        }

        foreach (var (course, prerequisite) in prerequisites)
        {
            if (course < 0 || course >= _numCourses || prerequisite < 0 || prerequisite >= _numCourses)
            {
                throw new IndexOutOfRangeException($"Course indices should be between 0 and {_numCourses}");
            }
            _adjacencyList[prerequisite].Add(course);
        }
    }

    public bool CanCompleteAllCourses()
    {
        if (_numCourses == 0)
            return true;

        var inDegrees = new int[_numCourses];
        var queue = new Queue<int>(_numCourses);
        var completedCourses = 0;

        foreach (var dependentCourses in _adjacencyList)
        {
            foreach (var dependentCourse in dependentCourses)
            {
                inDegrees[dependentCourse]++;
            }
        }

        for (int courseIndex = 0; courseIndex < _numCourses; courseIndex++)
        {
            if (inDegrees[courseIndex] == 0)
            {
                queue.Enqueue(courseIndex);
            }
        }

        while (queue.Count > 0)
        {
            var currentCourse = queue.Dequeue();
            completedCourses++;

            foreach (var dependentCourse in _adjacencyList[currentCourse])
            {
                if (--inDegrees[dependentCourse] == 0)
                {
                    queue.Enqueue(dependentCourse);
                }
            }
        }

        return completedCourses == _numCourses;
    }
}
