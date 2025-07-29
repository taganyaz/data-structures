// See https://aka.ms/new-console-template for more information
// Problem: You are given an array routes representing bus routes where routes[i] is a bus route that the ith bus repeats forever.
// For example, if routes[0] = [1, 5, 7], this means that the 0th bus travels in the sequence 1 -> 5 -> 7 -> 1 -> 5 -> 7 -> 1 -> ... forever.
// You will start at the bus stop source (You are not on any bus initially), and you want to go to the bus stop target. You can travel between bus stops by buses only.
// Return the least number of buses you must take to travel from source to target. Return -1 if it is not possible.

// Approach: Route-Based BFS with Level Tracking
// 
// Key Insight: Instead of traversing stop-by-stop, we traverse route-by-route. Each BFS level 
// represents taking one additional bus, naturally giving us the minimum bus count.
//
// Why This Works:
// - BFS guarantees shortest path in unweighted graphs
// - Each route (bus) is a node in our implicit graph
// - Routes are connected if they share at least one stop
// - The level in BFS = number of buses taken
//
// Data Structure Design:
// - Graph: Dictionary<int, HashSet<int>> maps each stop to all routes passing through it
// - This enables O(1) lookup of connecting routes at each stop
// - Using HashSet for routes prevents duplicate route processing
//
// Algorithm:
// 1. Build reverse index: stop → [routes containing this stop]
// 2. Initialize BFS with all routes containing the source stop
// 3. For each BFS level (representing one bus ride):
//    - Check if any route in current level contains target
//    - If yes, return current bus count
//    - Otherwise, explore all unvisited connecting routes
// 4. Return -1 if target unreachable
//
// Complexity Analysis:
// - Time: O(N × S²) where N = routes, S = avg stops per route
//   - Building graph: O(N × S)
//   - BFS: Each route processed once, checking S stops, each with up to S connecting routes
// - Space: O(N × S) for the graph structure
//
// Edge Cases Handled:
// - Source equals target (0 buses needed)
// - Source or target doesn't exist in any route
// - No path exists between source and target
// - Multiple routes from source (BFS explores all simultaneously)

var routes1 = new List<List<int>> { new() { 1, 2, 7 }, new() { 3, 6, 7 } };
var routes2 = new List<List<int>> { new() { 7, 12 }, new() { 4, 5, 15 }, new() { 6 }, new() { 15, 19 }, new() { 9, 12, 13 } };

Console.WriteLine("Route 1 Bus Count: " + new Solution().GetBusCount(routes1, 1, 6));
Console.WriteLine("Route 2 Bus Count: " + new Solution().GetBusCount(routes2, 15, 12));
class Solution
{
    public int GetBusCount(List<List<int>> routes, int source, int target)
    {
        if (source == target)
            return 0;

        var graph = new Dictionary<int, HashSet<int>>();
        var queue = new Queue<int>();
        var visited = new HashSet<int>();
        int busCount = 0;

        for (int i = 0; i < routes.Count; i++)
        {
            foreach (var stop in routes[i])
            {
                if (!graph.ContainsKey(stop))
                {
                    graph[stop] = new HashSet<int>();
                }
                graph[stop].Add(i);
            }
        }

        if (!graph.ContainsKey(target) || !graph.ContainsKey(source))
            return -1;

        foreach (int routeIndex in graph[source])
        {
            queue.Enqueue(routeIndex);
            visited.Add(routeIndex);
        }

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;
            busCount++;

            for (int i = 0; i < levelSize; i++)
            {
                var routeIndex = queue.Dequeue();

                if (routes[routeIndex].Contains(target))
                    return busCount;

                foreach (int stop in routes[routeIndex])
                {
                    foreach (int nextRoute in graph[stop])
                    {
                        if (!visited.Contains(nextRoute))
                        {
                            queue.Enqueue(nextRoute);
                            visited.Add(nextRoute);
                        }
                    }
                }
            }
        }

        return -1;
    }
}

