using AdventOfCode;

namespace AoC.Day12
{

    [Day(ExpectedValue = "29")]
    public class Day12Puzzle2 : IDay
    {
        private int _width;
        private int _height;
        private char[][]? _square;

        public string GetPuzzle(string input, bool isRealCase)
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

            var values = startPosition.Select(p => Algorithms.ShortestPathFunction(gr, p)(endPosition));


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

        public string GetPuzzle(string input, bool isRealCase)
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

            var function = Algorithms.ShortestPathFunction(gr, startPosition);
            

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
}
