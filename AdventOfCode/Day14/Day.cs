using AdventOfCode;
using System.Text;

namespace AoC.Day14
{
    [Day(ExpectedValue = "93")]
    public class Day14Puzzle2 : IDay
    {
        private int _height;
        private HashSet<(int x, int y)> _rocks = new HashSet<(int x, int y)>();
        private HashSet<(int x, int y)> _sands = new HashSet<(int x, int y)>();

        public string GetPuzzle(string input, bool isRealCase)
        {
            _rocks = input.GetLines()
                .Select(line => line.Split(" -> ")
                    .Select(c => c.Split(','))
                    .Select(c => (int.Parse(c[0]), int.Parse(c[1])))
                 )
                .SelectMany(GetAllCoord)
                .ToHashSet();

            _height = _rocks.Select(c => c.y).Max();
            _sands = new HashSet<(int x, int y)>();
            while (true)
            {
                int startY = 0;
                int startX = 500;

                while (true)
                {
                    var bottom = GetBottomPoint(startX, startY) + 1;
                    if (ArrivedToTop(bottom))
                    {
                        Print();
                        return _sands.Count.ToString();
                    }
                    if (HasToRest(startX, bottom))
                    {
                        _sands.Add((startX, bottom - 1));
                        break;
                    }
                    if (!IsCoordUsed((startX - 1, bottom)))
                    {
                        startX--;
                        startY = bottom;
                        continue;
                    }
                    else
                    {
                        startX++;
                        startY = bottom;
                        continue;
                    }
                }
            }
        }

        private static bool ArrivedToTop(int bottom)
        {
            return bottom == 0;
        }

        private bool HasToRest(int x, int y)
        {
            var left = (x - 1, y);
            var right = (x + 1, y);
            return y == _height + 2 || IsCoordUsed(left) && IsCoordUsed(right);
        }

        private bool IsCoordUsed((int x, int y) point)
        {
            return _rocks.Contains(point) || _sands.Contains(point);
        }

        public void Print()
        {
            int minWidth = _rocks.Select(x => x.x).Min();
            int maxWidth = _rocks.Select(x => x.x).Max();

            StringBuilder sb = new StringBuilder();
            for (int y = 0; y <= _height + 1; y++)
            {
                for (int x = minWidth; x <= maxWidth; x++)
                {
                    sb.Append(_rocks.Contains((x, y)) ? _sands.Contains((x, y)) ? "o" : "#" : ".");
                }
                sb.AppendLine();
            }
            for (int x = minWidth; x <= maxWidth; x++)
            {
                sb.Append("#");
            }
            sb.AppendLine();
            Console.WriteLine(sb.ToString());
        }

        private int GetBottomPoint(int startX, int startY)
        {
            for (int y = startY; y <= _height + 2; y++)
            {
                if (IsCoordUsed((startX, y)) || y == _height + 2)
                {
                    return y - 1;
                }
            }

            return _height;
        }

        private IEnumerable<(int, int)> GetAllCoord(IEnumerable<(int x, int y)> line)
        {
            var l = line.ToArray();
            for (int i = 0; i < l.Length - 1; i++)
            {
                var current = l[i];
                var next = l[i + 1];
                for (int x = Math.Min(current.x, next.x); x <= Math.Max(current.x, next.x); x++)
                {
                    for (int y = Math.Min(current.y, next.y); y <= Math.Max(current.y, next.y); y++)
                    {
                        yield return (x, y);
                    }
                }
            }
        }
    }

    [Day(ExpectedValue = "24")]
    public class Day14Puzzle1 : IDay
    {
        int height;
        HashSet<(int x, int y)> rocks = new HashSet<(int x, int y)>();
        HashSet<(int x, int y)> sands = new HashSet<(int x, int y)>();
        HashSet<(int, int)> testedPoints = new HashSet<(int, int)>();

        public string GetPuzzle(string input, bool isRealCase)
        {
            rocks = input.GetLines()
                .Select(line => line.Split(" -> ")
                    .Select(c => c.Split(','))
                    .Select(c => (int.Parse(c[0]), int.Parse(c[1])))
                 )
                .SelectMany(GetAllCoord)
                .ToHashSet();

            height = rocks.Select(c => c.y).Max();
            sands = new HashSet<(int x, int y)>();

            while (true)
            {
                int startY = 0;

                int x = 500;
                int startX = x;
                testedPoints = new HashSet<(int x, int y)>();
                while (true)
                {
                    var newY = GetBottomPoint(rocks, height, x, startY) + 1;
                    if(testedPoints.Contains((x, startY)))
                    {
                        Print();
                        throw new Exception("Infinite loop");
                    }
                    testedPoints.Add((x, startY));
                    if (newY > height)
                    {
                        Print();
                        return sands.Count.ToString();
                    }
                    if(rocks.Contains((x-1, newY)) && rocks.Contains((x + 1, newY)))
                    {
                        rocks.Add((x, newY - 1));
                        sands.Add((x, newY - 1));
                        break;
                    }
                    if (!rocks.Contains((x - 1, newY)))
                    {
                        x--;
                        startY = newY;
                        continue;
                    }
                    else
                    {
                        x++;
                        startY = newY;
                        continue;
                    }
                }
            }
        }

        public void Print()
        {
            int minWidth = rocks.Select(x => x.x).Min();
            int maxWidth = rocks.Select(x => x.x).Max();

            StringBuilder sb = new StringBuilder();
            for (int y = 0; y <= height; y++)
            {
                for (int x = minWidth; x <= maxWidth; x++)
                {
                    sb.Append(rocks.Contains((x, y)) ? sands.Contains((x,y)) ? "o" : "#" : ".");
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
        }

        private static int GetBottomPoint(HashSet<(int x, int y)> coord, int height, int x, int startY)
        {
            for (int y = startY; y <= height; y++)
            {
                if (coord.Contains((x, y)))
                {
                    return y - 1;
                }
            }

            return height;
        }

        private IEnumerable<(int, int)> GetAllCoord(IEnumerable<(int x, int y)> line)
        {
            var l = line.ToArray();
            for (int i = 0; i < l.Length - 1; i++)
            {
                var current = l[i];
                var next = l[i + 1];
                for (int x = Math.Min(current.x, next.x); x <= Math.Max(current.x, next.x); x++)
                {
                    for (int y = Math.Min(current.y, next.y); y <= Math.Max(current.y, next.y); y++)
                    {
                        yield return (x, y);
                    }
                }
            }
        }
    }
}
