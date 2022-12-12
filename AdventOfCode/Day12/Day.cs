using AdventOfCode;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AoC.Day12
{

    [Day(ExpectedValue = "29")]
    public class Day12Puzzle2 : IDay
    {
        private int _width;
        private int _height;
        private char[][]? _square;

        public string GetPuzzle(string input)
        {
            _square = input.Split(Environment.NewLine)
                .Select(l => l.ToCharArray()).ToArray();
            _width = _square.Length;
            _height = _square[0].Length;

            var graph = MyEnumerable.GetTableValues(_width, _height).ToDictionary(v => v, v => GetNeighboor(v).ToHashSet());
            
            var startPosition = MyEnumerable.GetTableValues(_width, _height)
                .Where(v => _square[v.x][v.y] == 'a')
                .Where(v => graph.ContainsKey(v) && graph[v].Count > 0);
            (int x, int y) endPosition = MyEnumerable.GetTableValues(_width, _height)
                .First(v => _square[v.x][v.y] == 'E');



            var gr = new Graph<(int x, int y)>(graph);

            var algorithms = new Algorithms();
            var values = startPosition.Select(p => algorithms.ShortestPathFunction(gr, p)(endPosition));


            return values.Where(v => v != null).Select(v => v!.Count() - 1).Min().ToString();
        }

        private IEnumerable<(int x, int y)> GetNeighboor((int x, int y) position)
        {
            (int x, int y)[] neighboors = new[] { (position.x - 1, position.y), (position.x, position.y - 1), (position.x + 1, position.y), (position.x, position.y + 1) };
            var current = GetChar(position);
            foreach (var neighboor in neighboors)
            {
                if (neighboor.x >= 0 && neighboor.y >= 0 && neighboor.x < _width && neighboor.y < _height)
                {
                    var next = GetChar(neighboor);
                    if (next - current <= 1)
                    {
                        yield return neighboor;
                    }
                }
            }
        }

        private char GetChar((int x, int y) newPosition)
        {
            var c = _square![newPosition.x][newPosition.y];
            return c == 'E' ? (char)('z' + 1) : c == 'S' ? (char)('a' - 1) : c;
        }
    }



    [Day(ExpectedValue = "31")]
    public class Day12Puzzle1 : IDay
    {
        private int _width;
        private int _height;
        private char[][]? _square;

        public string GetPuzzle(string input)
        {
            _square = input.Split(Environment.NewLine)
                .Select(l => l.ToCharArray()).ToArray();
            _width = _square.Length;
            _height = _square[0].Length;

            (int x, int y) startPosition = MyEnumerable.GetTableValues(_width, _height)
                .First(v => _square[v.x][v.y] == 'S');
            (int x, int y) endPosition = MyEnumerable.GetTableValues(_width, _height)
                .First(v => _square[v.x][v.y] == 'E');


            var graph = MyEnumerable.GetTableValues(_width, _height).ToDictionary(v => v, v => GetNeighboor(v).ToHashSet());

            var gr = new Graph<(int x, int y)>(graph);

            var algorithms = new Algorithms();
            var function = algorithms.ShortestPathFunction(gr, startPosition);
            

            return (function(endPosition)!.Count() - 1).ToString();
        }

        private IEnumerable<(int x, int y)> GetNeighboor((int x, int y) position)
        {
            (int x, int y)[] neighboors = new []{ (position.x - 1, position.y), (position.x, position.y - 1),  (position.x + 1, position.y), (position.x, position.y + 1) };
            var current = GetChar(position);
            foreach (var neighboor in neighboors)
            {
                if (neighboor.x >= 0 && neighboor.y >= 0 && neighboor.x < _width && neighboor.y < _height)
                {
                    var next = GetChar(neighboor);
                    if (next - current <= 1)
                    {
                        yield return neighboor;
                    }
                }
            }
        }
        private char GetChar((int x, int y) newPosition)
        {
            var c = _square![newPosition.x][newPosition.y];
            return c == 'E' ? (char)('z' + 1) : c == 'S' ? (char)('a' - 1) : c;
        }
    }


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
        public Func<T, IEnumerable<T>?> ShortestPathFunction<T>(Graph<T> graph, T start)
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
                    if(!previous.ContainsKey(current))
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
    }
}
