// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
// Problem: Given an integer n which represents the number of nodes in a graph, and a list of edges edges, where edges[i] = [ui, vi] represents a bidirectional edge between nodes ui and vi, write a function to return the adjacency list representation of the graph as a dictionary. The keys of the dictionary should be the nodes, and the values should be a list of the nodes each node is connected to.
// Example:
// n = 4
// edges = [[0, 1], [1, 2], [2, 3], [3, 0], [0, 2]]

// Approach: Build an adjacency list representation using a Dictionary
// Time Complexity: O(E) where E is the number of edges
// Space Complexity: O(V + E) where V is the number of vertices
//
// Algorithm:
// 1. Initialize an empty dictionary to store the adjacency list
// 2. Iterate through each edge (u, v) in the input
// 3. For each edge:
//    - Ensure both vertices exist in the dictionary (lazy initialization)
//    - Add v to u's adjacency list and u to v's list (bidirectional edge)
//
// Design Considerations:
// - Using Dictionary<int, List<int>> for O(1) vertex lookup
// - Handling bidirectional edges by adding connections in both directions
// - Edge case handling: null or empty edge list returns empty graph
// - Note: Parameter 'n' is not used as we derive vertices from edges
//
// Potential Optimizations:
// - Pre-allocate lists if n is known to avoid list resizing
// - Use HashSet<int> instead of List<int> if duplicate edges need to be avoided
// - Consider using adjacency matrix for dense graphs (E ≈ V²)

// ...existing code...

List<(int, int)> edges = new() { (0, 1), (1, 2), (2, 3), (3, 0), (0, 2) };
var graph = new Solution().GetAdjacencyList(edges);

foreach (int u in graph.Keys)
{
    Console.WriteLine($"{u}: {string.Join(", ", graph[u])}");
}

class Solution
{
    public Dictionary<int, List<int>> GetAdjacencyList(List<(int, int)> edges)
    {
        var adjacencyList = new Dictionary<int, List<int>>();
        if (edges == null || edges.Count == 0)
            return adjacencyList;

        foreach (var (u, v) in edges)
        {
            if (!adjacencyList.ContainsKey(u))
            {
                adjacencyList[u] = new List<int>();
            }

            if (!adjacencyList.ContainsKey(v))
            {
                adjacencyList[v] = new List<int>();
            }

            adjacencyList[u].Add(v);
            adjacencyList[v].Add(u);
        }

        return adjacencyList;
    }
}

