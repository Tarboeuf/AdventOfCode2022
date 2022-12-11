using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace AoC.Day5
{

    [Day(ExpectedValue = "MCD")]
    public class Day05Puzzle2 : IDay
    {
        public string GetPuzzle(string input)
        {
            var part = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var stacks = GetStacks(part[0]);
            var moves = GetMoves(part[1]);

            foreach (var move in moves)
            {
                List<char> pops = new List<char>();
                for (int i = 0; i < move.Quantity; i++)
                {
                    pops.Add(stacks[move.Origin].Pop());
                }
                foreach(var c in pops.Reverse<char>())
                {
                    stacks[move.Target].Push(c);
                }
            }
            return string.Concat(stacks.Values.Select(s => s.Pop()));
        }


        public Dictionary<int, Stack<char>> GetStacks(string input)
        {
            var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var result = lines.Last()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v[0] - '0')
                .ToDictionary(v => v, _ => new Stack<char>());

            foreach (var line in lines.Reverse().Skip(1))
            {
                for (int i = 1; i <= result.Count; i++)
                {
                    var container = line[(i - 1) * 4 + 1];
                    if(container != ' ')
                    {
                        result[i].Push(container);
                    }
                }
            }
            return result;
        }

        static readonly Regex _moveRegex = new Regex(@"move\s(?<qty>\d+)\sfrom\s(?<from>\d+)\sto\s(?<to>\d+)", RegexOptions.Compiled);

        public IEnumerable<Move> GetMoves(string input)
        {
            foreach (var item in input.Split(Environment.NewLine))
            {
                var match = _moveRegex.Match(item);
                yield return new Move
                {
                    Quantity = int.Parse(match.Groups["qty"].Value),
                    Origin = int.Parse(match.Groups["from"].Value),
                    Target = int.Parse(match.Groups["to"].Value)
                };
            }
        }
    }

    [Day(ExpectedValue = "CMZ")]
    public class Day05Puzzle1 : IDay
    {
        public string GetPuzzle(string input)
        {
            var part = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var stacks = GetStacks(part[0]);
            var moves = GetMoves(part[1]);

            foreach (var move in moves)
            {
                for (int i = 0; i < move.Quantity; i++)
                {
                    stacks[move.Target].Push(stacks[move.Origin].Pop());
                }
            }
            return string.Concat(stacks.Values.Select(s => s.Pop()));
        }


        public Dictionary<int, Stack<char>> GetStacks(string input)
        {
            var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var result = lines.Last()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v[0] - '0')
                .ToDictionary(v => v, _ => new Stack<char>());

            foreach (var line in lines.Reverse().Skip(1))
            {
                for (int i = 1; i <= result.Count; i++)
                {
                    var container = line[(i - 1) * 4 + 1];
                    if(container != ' ')
                    {
                        result[i].Push(container);
                    }
                }
            }
            return result;
        }

        static readonly Regex _moveRegex = new Regex(@"move\s(?<qty>\d+)\sfrom\s(?<from>\d+)\sto\s(?<to>\d+)", RegexOptions.Compiled);

        public IEnumerable<Move> GetMoves(string input)
        {
            foreach (var item in input.Split(Environment.NewLine))
            {
                var match = _moveRegex.Match(item);
                yield return new Move
                {
                    Quantity = int.Parse(match.Groups["qty"].Value),
                    Origin = int.Parse(match.Groups["from"].Value),
                    Target = int.Parse(match.Groups["to"].Value)
                };
            }
        }

    }

    public class Move
    {
        public int Quantity { get; set; }
        public int Origin { get; set; }
        public int Target { get; set; }
    }
}
