using AdventOfCode;
using System.Text;

namespace AoC.Day16
{
    [Day(ExpectedValue = "1707")]
    public class Day16Puzzle2 : Day16PuzzleBase
    {
        protected override string GetResult(Node start)
        {
            int best = 0;

            for (int mask = 0; mask < allMask; mask++)
            {
                best = Math.Max(best, Process(start, 26, mask) + Process(start, 26, allMask - mask));
            }
            return best.ToString();
        }
    }

    [Day(ExpectedValue = "1651")]
    public class Day16Puzzle1 : Day16PuzzleBase
    {
        protected override string GetResult(Node start)
        {
            return Process(start, 30, 0).ToString();
        }
    }

    public abstract class Day16PuzzleBase : IDay
    {
        private Dictionary<Node, HashSet<Node>>? graph;
        private Dictionary<(Node current, int timeLeft, long releaseMask), int> cache = new Dictionary<(Node current, int timeLeft, long releaseMask), int>();
        protected long allMask = 0;

        public string GetPuzzle(string input, bool isRealCase)
        {
            var nodes = input.GetLines()
                .Select(GetCouples)
                .ToList();
            cache.Clear();
            allMask = 0;
            var dictionary = nodes.ToDictionary(c => c.Name!, c => c);
            graph = nodes.ToDictionary(c => c, c => c.Edges!.Select(t => dictionary[t]).ToHashSet());
            int inc = 0;
            foreach (var node in nodes)
            {
                if (node.Rate > 0)
                {
                    allMask += 1 << inc;
                    node.PressureNode = inc++;
                }
            }
            var start = dictionary["AA"];
            return GetResult(start);
        }

        protected abstract string GetResult(Node start);

        public int Process(Node current, int timeLeft, long processedRatesMask)
        {
            if (cache.ContainsKey((current, timeLeft, processedRatesMask))) return cache[(current, timeLeft, processedRatesMask)];
            if (timeLeft == 1 || processedRatesMask == allMask)
            {
                return 0;
            }
            int result = 0;
            long currentPressureFlag = (1 << current.PressureNode);
            bool isAlreadyProcessed = (currentPressureFlag & processedRatesMask) != 0;
            if (current.Rate > 0 && !isAlreadyProcessed)
            {
                result = current.Rate * (timeLeft - 1) + Process(current, timeLeft - 1, processedRatesMask + currentPressureFlag);
            }
            foreach (var node in graph![current])
            {
                result = Math.Max(result, Process(node, timeLeft - 1, processedRatesMask));
            }
            cache.Add((current, timeLeft, processedRatesMask), result);
            return result;
        }

        private Node GetCouples(string line)
        {
            line = line.Replace("Valve ", "").Replace(" has flow rate", "").Replace(" tunnels lead to valves ", "").Replace(" tunnel leads to valve ", "");
            var couple = line[3..].Split(';');
            return new Node
            {
                Name = line[0..2],
                Rate = int.Parse(couple[0]),
                Edges = couple[1].Split(',').Select(v => v.Trim()).ToList()
            };
        }
    }
    public class Node
    {
        public string? Name { get; set; }
        public int Rate { get; set; }
        public List<string>? Edges { get; set; }
        public int PressureNode { get; set; } = -1;

        public override string ToString()
        {
            return $"{Name}";
        }
    }



    class Comparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[]? left, int[]? right)
        {
            if (left == null) return right == null;
            if (right == null) return false;
            if (left.Length != right.Length) return false;

            return left.SequenceEqual(right);
        }

        public int GetHashCode(int[] value)
        {
            int hash = 1;
            for (int i = 0; i < value.Length; i++)
            {
                hash += value[i] ^ (7 * i);
            }
            return hash;
        }
    }

}
