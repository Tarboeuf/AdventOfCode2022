

using AdventOfCode;
using System.Text;

namespace AoC.Day9
{
    [Day(ExpectedValue = "1")]
    public class Day9Puzzle2 : IDay
    {
        [Fact]
        public void Test2()
        {
            var input = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20";
            Assert.Equal("36", GetPuzzle(input));
        }

        public string GetPuzzle(string input)
        {
            var maxItemInQueue = 10;
            (int x, int y)[] tailPositions = new (int x, int y)[maxItemInQueue];
            (int x, int y) currentHead = (0, 0);
            for (int i = 0; i < maxItemInQueue; i++)
            {
                tailPositions[i] = currentHead;
            }

            HashSet<(int x, int y)> lastKnotHistory = new HashSet<(int x, int y)>();

            foreach (var item in input.Split(Environment.NewLine))
            {
                int delta = int.Parse(item[2..]);
                (int xd, int yd) vector = item[0] switch
                {
                    'R' => (1, 0),
                    'L' => (-1, 0),
                    'U' => (0, 1),
                    'D' => (0, -1),
                    _ => throw new NotImplementedException(),
                };


                for (int inc = 0; inc < delta; inc++)
                {
                    currentHead = (currentHead.x + vector.xd, currentHead.y + vector.yd);
                    tailPositions[0] = currentHead;
                    for (int index = 1; index < maxItemInQueue; index++)
                    {
                        var current = tailPositions[index];
                        var previous = tailPositions[index - 1];
                        if (AreFar(previous, current))
                        {
                            if (previous.x == current.x)
                            {
                                tailPositions[index] = (current.x, (current.y + previous.y) / 2);
                            }
                            else if (previous.y == current.y)
                            {
                                tailPositions[index] = ((current.x + previous.x) / 2, current.y);
                            }
                            else
                            {
                                (int xd, int yd) d = (current.x > previous.x ? -1 : 1, current.y > previous.y ? -1 : 1);
                                tailPositions[index] = (current.x + d.xd, current.y + d.yd);
                            }
                        }
                    }

                    lastKnotHistory.Add(tailPositions[maxItemInQueue - 1]);
                }
                Print(tailPositions);
            }

            Print(lastKnotHistory);
            return lastKnotHistory.Count().ToString();
        }

        private static void Print((int x, int y)[] tailPositions)
        {
            int minWidth = tailPositions.Select(c => c.x).Min();
            int minHeight = tailPositions.Select(c => c.y).Min();

            int width = tailPositions.Select(c => c.x).Max();
            int height = tailPositions.Select(c => c.y).Max();
            StringBuilder output = new StringBuilder();
            for (int y = minHeight; y <= height; y++)
            {
                for (int x = minWidth; x <= width; x++)
                {
                    var index = Array.IndexOf(tailPositions, (x, y));
                    output.Append((x == 0 && y == 0) ? "s" : (index != -1 ? index.ToString() : "."));
                }
                output.Append(Environment.NewLine);
            }
        }

        private static void Print(HashSet<(int x, int y)> lastKnotHistory)
        {
            int minWidth = lastKnotHistory.Select(c => c.x).Min();
            int minHeight = lastKnotHistory.Select(c => c.y).Min();

            int width = lastKnotHistory.Select(c => c.x).Max();
            int height = lastKnotHistory.Select(c => c.y).Max();
            StringBuilder output = new StringBuilder();
            for (int y = minHeight; y <= height; y++)
            {
                for (int x = minWidth; x <= width; x++)
                {
                    output.Append(lastKnotHistory.Contains((x, y)) ? "#" : (x == 0 && y == 0) ? "s" : ".");
                }
                output.Append(Environment.NewLine);
            }
        }

        private static bool AreFar((int x, int y) previous, (int x, int y) current)
        {
            return Math.Abs(previous.x - current.x) > 1 || Math.Abs(previous.y - current.y) > 1;
        }
    }

    [Day(ExpectedValue = "13")]
    public class Day9Puzzle1 : IDay
    {
        public string GetPuzzle(string input)
        {
            HashSet<(int x, int y)> tailPositions = new HashSet<(int x, int y)>();

            (int x, int y) currentHead = (0, 0);
            (int x, int y) currentTail = (0, 0);
            tailPositions.Add(currentTail);
            foreach (var item in input.Split(Environment.NewLine))
            {
                int delta = int.Parse(item[2..]);

                (int x, int y) vector = item[0] switch
                {
                    'R' => (1, 0),
                    'L' => (-1, 0),
                    'U' => (0, 1),
                    'D' => (0, -1),
                    _ => throw new NotImplementedException(),
                };

                for (int i = 0; i < delta; i++)
                {
                    currentHead = (currentHead.x + vector.x, currentHead.y + vector.y);
                    if (Math.Abs(currentHead.x - currentTail.x) > 1 || Math.Abs(currentHead.y - currentTail.y) > 1)
                    {
                        currentTail = (currentHead.x - vector.x, currentHead.y - vector.y);
                        tailPositions.Add(currentTail);
                    }
                }
            }

            return tailPositions.Distinct().Count().ToString();
        }
    }
}
