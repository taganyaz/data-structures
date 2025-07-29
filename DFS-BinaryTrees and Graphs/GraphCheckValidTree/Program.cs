// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
// Problem: Given n nodes labeled from 0 to n - 1 and a list of undirected edges (each edge is a pair of nodes), write a function to check whether these edges make up a valid tree.
// Example 1:
// Input: n = 5 edges = [[0, 1], [0, 2], [0, 3], [1, 4]]
// Output: true.
// Example 2:
// Input: n = 5 edges = [[0, 1], [1, 2], [2, 3], [1, 3], [1, 4]]
// Output: false.

// Approach: A valid tree must satisfy two conditions:
// 1. It must be connected (all nodes reachable from any node)
// 2. It must be acyclic (no cycles/loops in the graph)
//
// Algorithm:
// 1. Early validation: A tree with n nodes must have exactly (n-1) edges
//    - If edges < (n-1): disconnected components exist
//    - If edges > (n-1): cycles must exist
//    - If edges = (n-1): need to verify connectivity and acyclicity
//
// 2. Build an undirected graph using adjacency list representation
//    - Use Dictionary<int, HashSet<int>> for O(1) neighbor lookups
//    - Add edges bidirectionally since the graph is undirected
//
// 3. Perform DFS to detect cycles:
//    - Track visited nodes to avoid revisiting
//    - Track parent to distinguish between:
//      a) Back edge to parent (expected in undirected graph)
//      b) Back edge to ancestor (indicates cycle)
//    - If we encounter a visited node that's not our parent, we found a cycle
//
// 4. Verify connectivity:
//    - After DFS completes, check if all nodes were visited
//    - If any node remains unvisited, the graph is disconnected
//
// Time Complexity: O(V + E) where V = number of vertices, E = number of edges
// Space Complexity: O(V + E) for the adjacency list and visited array

var edges = new List<(int, int)>() { (0, 1), (0, 2), (0, 3), (1, 4) };
Console.WriteLine("Is valid Tree:" + new Solution().IsValidTree(edges, 5));
Console.WriteLine("Is valid Tree Iter:" + new Solution().IsValidTreeIterative(edges, 5));

var edges2 = new List<(int, int)>() { (0, 1), (1, 2), (2, 3), (1, 3), (1, 4) };
Console.WriteLine("Is valid Tree:" + new Solution().IsValidTree(edges2, 5));
Console.WriteLine("Is valid Tree Iter:" + new Solution().IsValidTreeIterative(edges2, 5));

class Solution
{
    public bool IsValidTree(List<(int, int)> edges, int n)
    {
        if (edges.Count != n - 1)
        {
            return false;
        }
        if (n == 1)
        {
            return true;
        }

        // Get graph
        var graph = GetGraph(edges, n);
        var visited = new bool[n];

        if (HasACycle(graph, 0, visited, -1))
        {
            return false;
        }

        for (int i = 0; i < n; i++)
        {
            if (!visited[i])
            {
                return false;
            }
        }
        return true;
    }

    private bool HasACycle(Dictionary<int, HashSet<int>> graph, int vertex, bool[] visited, int parent)
    {
        visited[vertex] = true;

        foreach (int neighbor in graph[vertex])
        {
            if (!visited[neighbor])
            {
                if (HasACycle(graph, neighbor, visited, vertex))
                {
                    return true;
                }
            }
            else if (neighbor != parent)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsValidTreeIterative(List<(int, int)> edges, int n)
    {
        if (edges.Any(edge => edge.Item1 == edge.Item2))
            return false;

        if (n <= 1)
            return true;

        if (edges.Count != n - 1)
            return false;

        var graph = GetGraph(edges, n);
        var visited = new bool[n];

        if (HasACycleIterative(graph, 0, visited))
            return false;

        for (int i = 0; i < n; i++)
            if (!visited[i])
                return false;

        return true;
    }

    private bool HasACycleIterative(Dictionary<int, HashSet<int>> graph, int start, bool[] visited)
    {
        var stack = new Stack<(int node, int parent)>();
        stack.Push((start, -1));

        while (stack.Count > 0)
        {
            var (node, parent) = stack.Pop();

            if (visited[node])
                continue;

            visited[node] = true;

            foreach (int neighbor in graph[node])
            {
                if (!visited[neighbor])
                    stack.Push((neighbor, node));
                else if (neighbor != parent)
                    return true;

            }
        }
        return false;
    }

    private Dictionary<int, HashSet<int>> GetGraph(List<(int, int)> edges, int n)
    {
        var graph = new Dictionary<int, HashSet<int>>();
        for (int i = 0; i < n; i++)
        {
            graph[i] = new HashSet<int>();
        }

        foreach (var (u, v) in edges)
        {
            graph[u].Add(v);
            graph[v].Add(u);
        }
        return graph;
    }
}