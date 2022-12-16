// Thanks to https://www.koderdojo.com/blog/breadth-first-search-and-shortest-path-in-csharp-and-net-core

namespace AdventOfCode
{

    public class Graph<T>
        where T : notnull
    {
        private readonly Dictionary<T, HashSet<T>> _adjacencyList;

        public Graph(Dictionary<T, HashSet<T>> adjacencyList)
        {
            _adjacencyList = adjacencyList;
        }

        public Dictionary<T, HashSet<T>> AdjacencyList => _adjacencyList;
    }


    public class Algorithms
    {
        public static Func<T, IEnumerable<T>?> ShortestPathFunction<T>(Graph<T> graph, T start)
            where T : notnull
        {
            var previous = new Dictionary<T, T>();

            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in graph.AdjacencyList[vertex])
                {
                    if (previous.ContainsKey(neighbor))
                        continue;

                    previous[neighbor] = vertex;
                    queue.Enqueue(neighbor);
                }
            }

            Func<T, IEnumerable<T>?> shortestPath = v => {
                var path = new List<T> { };

                var current = v;
                while (!current!.Equals(start))
                {
                    path.Add(current);
                    if (!previous.ContainsKey(current))
                    {
                        return null;
                    }
                    current = previous[current];
                };

                path.Add(start);
                path.Reverse();

                return path;
            };

            return shortestPath;
        }

        public static HashSet<T> BFS<T>(Graph<T> graph, T start)
            where T : notnull
        {
            var visited = new HashSet<T>();

            if (!graph.AdjacencyList.ContainsKey(start))
                return visited;

            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();

                if (visited.Contains(vertex))
                    continue;

                visited.Add(vertex);

                foreach (var neighbor in graph.AdjacencyList[vertex])
                    if (!visited.Contains(neighbor))
                        queue.Enqueue(neighbor);
            }

            return visited;
        }

        public static HashSet<T> DFS<T>(Graph<T> graph, T start)
            where T : notnull
        {
            var visited = new HashSet<T>();

            if (!graph.AdjacencyList.ContainsKey(start))
                return visited;

            var stack = new Stack<T>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                var vertex = stack.Pop();

                if (visited.Contains(vertex))
                    continue;

                visited.Add(vertex);

                foreach (var neighbor in graph.AdjacencyList[vertex])
                    if (!visited.Contains(neighbor))
                        stack.Push(neighbor);
            }

            return visited;
        }
    }
}