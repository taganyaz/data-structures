// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
/*
Problem: Topological Sort of a Directed Acyclic Graph (DAG)

Description:
Given a directed acyclic graph represented as an adjacency list, return a valid topological ordering
of the nodes. A topological ordering is a linear ordering of vertices such that for every directed
edge (u, v), vertex u comes before v in the ordering.

Example:
Input: adjacency list representation
{   
    0: [1, 3],    // Node 0 has edges to nodes 1 and 3
    1: [2],       // Node 1 has an edge to node 2
    2: [],        // Node 2 has no outgoing edges
    3: [1, 4, 5], // Node 3 has edges to nodes 1, 4, and 5
    4: [5],       // Node 4 has an edge to node 5
    5: []         // Node 5 has no outgoing edges
}

Output: [0, 3, 1, 4, 2, 5]
Note: Multiple valid topological orderings may exist. Any valid ordering is acceptable.

Algorithm: Kahn's Algorithm (BFS-based Topological Sort)

Overview:
The algorithm uses the concept of in-degree (number of incoming edges) to iteratively remove nodes
with no dependencies and build the topological ordering.

Detailed Steps:
1. Initialize Data Structures:
   - Create an in-degree map to track incoming edges for each node
   - Initialize a queue to store nodes with zero in-degree
   - Create a result list to store the topological ordering

2. Calculate In-degrees:
   - Traverse the adjacency list
   - For each edge (u → v), increment the in-degree of v
   - Ensure all nodes are represented in the in-degree map

3. Process Nodes:
   - Enqueue all nodes with in-degree 0 (no dependencies)
   - While the queue is not empty:
     a) Dequeue a node and add it to the result
     b) For each neighbor of the dequeued node:
        - Decrement the neighbor's in-degree
        - If the neighbor's in-degree becomes 0, enqueue it

4. Validate Result:
   - If the result size equals the number of nodes, return the topological order
   - Otherwise, the graph contains a cycle (not a valid DAG)

Complexity Analysis:
- Time Complexity: O(V + E)
  - V: number of vertices (nodes)
  - E: number of edges
  - We visit each vertex once and examine each edge once
  
- Space Complexity: O(V)
  - In-degree map: O(V)
  - Queue: O(V) in worst case
  - Result list: O(V)

Edge Cases:
- Empty graph: Returns empty list
- Single node: Returns list with that node
- Disconnected components: All components are processed correctly
- Cyclic graph: Throws InvalidOperationException

Applications:
- Build systems (dependency resolution)
- Course scheduling with prerequisites
- Task scheduling in project management
- Compilation order determination
- Data pipeline orchestration
*/

var graph = new Dictionary<int, HashSet<int>>
{
    {0, new() { 1, 3} },
    {1, new() { 2 }},
    {2, new() {}},
    {3, new () {1, 4, 5}},
    {4, new() { 5}},
    {5, new (){}}
};

var result = new Solution().GetTopologicalSort(graph);
Console.WriteLine($"Output: [{string.Join(", ", result)}]");
class Solution
{

    public List<int> GetTopologicalSort(Dictionary<int, HashSet<int>> graph)
    {
        if (graph == null)
        {
            throw new ArgumentNullException(nameof(graph));
        }

        if (graph.Count == 0)
        {
            return new List<int>();
        }

        var result = new List<int>(graph.Count);
        var inDegrees = new Dictionary<int, int>();

        foreach (var (node, neighbors) in graph)
        {
            if (!inDegrees.ContainsKey(node))
            {
                inDegrees.Add(node, 0);
            }

            foreach (var neighbor in neighbors)
            {
                inDegrees.TryAdd(neighbor, 0);
                inDegrees[neighbor]++;
            }
        }

        var queue = new Queue<int>(graph.Count);
        foreach (var (node, degree) in inDegrees)
        {
            if (degree == 0)
            {
                queue.Enqueue(node);
            }
        }

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            result.Add(node);

            if (graph.TryGetValue(node, out var neighbors))
            {
                foreach (var neighbor in neighbors)
                {
                    if (--inDegrees[neighbor] == 0)
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        if (result.Count != inDegrees.Count)
        {
            throw new InvalidOperationException("Graph contains a cycle");
        }
        return result;
    }
}
