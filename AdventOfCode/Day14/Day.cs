using AdventOfCode;
using System;
using System.Text;

namespace AoC.Day14
{
    [Day(ExpectedValue = "93")]
    public class Day14Puzzle2 : IDay
    {
        int height;
        HashSet<(int x, int y)> coord = new HashSet<(int x, int y)>();
        HashSet<(int x, int y)> addedPoints = new HashSet<(int x, int y)>();
        HashSet<(int, int)> testedPoints = new HashSet<(int, int)>();

        public string GetPuzzle(string input)
        {
            coord = input.GetLines()
                .Select(line => line.Split(" -> ")
                    .Select(c => c.Split(','))
                    .Select(c => (int.Parse(c[0]), int.Parse(c[1])))
                 )
                .SelectMany(GetAllCoord)
                .ToHashSet();

            height = coord.Select(c => c.y).Max();
            addedPoints = new HashSet<(int x, int y)>();
            while (true)
            {
                int startY = 0;

                int x = 500;
                int startX = x;
                testedPoints = new HashSet<(int x, int y)>();
                while (true)
                {
                    var newY = GetBottomPoint(coord, height, x, startY) + 1;
                    if (testedPoints.Contains((x, startY)))
                    {
                        Print();
                        throw new Exception("Infinite loop");
                    }
                    testedPoints.Add((x, startY));
                    if (newY == 0)
                    {
                        Print();
                        return addedPoints.Count.ToString();
                    }
                    if ((coord.Contains((x - 1, newY)) && coord.Contains((x + 1, newY))) || newY == height + 2)
                    {
                        coord.Add((x, newY - 1));
                        addedPoints.Add((x, newY - 1));
                        break;
                    }
                    if (!coord.Contains((x - 1, newY)))
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
            return;
            int minWidth = coord.Select(x => x.x).Min();
            int maxWidth = coord.Select(x => x.x).Max();

            StringBuilder sb = new StringBuilder();
            for (int y = 0; y <= height + 1; y++)
            {
                for (int x = minWidth; x <= maxWidth; x++)
                {
                    sb.Append(coord.Contains((x, y)) ? addedPoints.Contains((x, y)) ? "o" : "#" : ".");
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

        private static int GetBottomPoint(HashSet<(int x, int y)> coord, int height, int x, int startY)
        {
            for (int y = startY; y <= height + 2; y++)
            {
                if (coord.Contains((x, y)) || y == height + 2)
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

    [Day(ExpectedValue = "24")]
    public class Day14Puzzle1 : IDay
    {
        int height;
        HashSet<(int x, int y)> coord = new HashSet<(int x, int y)>();
        HashSet<(int x, int y)> addedPoints = new HashSet<(int x, int y)>();
        HashSet<(int, int)> testedPoints = new HashSet<(int, int)>();

        public string GetPuzzle(string input)
        {
            coord = input.GetLines()
                .Select(line => line.Split(" -> ")
                    .Select(c => c.Split(','))
                    .Select(c => (int.Parse(c[0]), int.Parse(c[1])))
                 )
                .SelectMany(GetAllCoord)
                .ToHashSet();

            height = coord.Select(c => c.y).Max();
            addedPoints = new HashSet<(int x, int y)>();

            while (true)
            {
                int startY = 0;

                int x = 500;
                int startX = x;
                testedPoints = new HashSet<(int x, int y)>();
                while (true)
                {
                    var newY = GetBottomPoint(coord, height, x, startY) + 1;
                    if(testedPoints.Contains((x, startY)))
                    {
                        Print();
                        throw new Exception("Infinite loop");
                    }
                    testedPoints.Add((x, startY));
                    if (newY > height)
                    {
                        Print();
                        return addedPoints.Count.ToString();
                    }
                    if(coord.Contains((x-1, newY)) && coord.Contains((x + 1, newY)))
                    {
                        coord.Add((x, newY - 1));
                        addedPoints.Add((x, newY - 1));
                        break;
                    }
                    if (!coord.Contains((x - 1, newY)))
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
            int minWidth = coord.Select(x => x.x).Min();
            int maxWidth = coord.Select(x => x.x).Max();

            StringBuilder sb = new StringBuilder();
            for (int y = 0; y <= height; y++)
            {
                for (int x = minWidth; x <= maxWidth; x++)
                {
                    sb.Append(coord.Contains((x, y)) ? addedPoints.Contains((x,y)) ? "o" : "#" : ".");
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
