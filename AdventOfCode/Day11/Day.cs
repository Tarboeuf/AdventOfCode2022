using AdventOfCode;
using System.Text;

namespace AoC.Day11
{
    [Day(ExpectedValue = "2713310158")]
    public class Day11Puzzle2 : IDay
    {
        public string GetPuzzle(string input)
        {
            List<Monkey> monkeys = new List<Monkey>();
            foreach(var group in input.Split(Environment.NewLine + Environment.NewLine))
            {
                var monkeyDefinition = group.Split(Environment.NewLine);
                Monkey monkey = new Monkey()
                {
                    Id = monkeyDefinition[0][7] - (int)'0',
                    StartingItems = new Queue<ulong>(monkeyDefinition[1][18..].Split(',').Select(v =>v.Trim()).Select(v => ulong.Parse(v))),
                    WorryOpertation = GetOperation(monkeyDefinition[2][19..]),
                    Test = (worry => worry % ulong.Parse(monkeyDefinition[3][21..]) == 0),
                    TrueId = int.Parse(monkeyDefinition[4][29..]),
                    FalseId = int.Parse(monkeyDefinition[5][30..]),
                    Diviser = int.Parse(monkeyDefinition[3][21..]),
                };
                monkeys.Add(monkey);
            }
            ulong modulo = monkeys.Select(m => m.Diviser).Product();

            var lookUp = monkeys.ToDictionary(m => m.Id, m => m);
            for(int round = 0; round < 10000; round++)
            {
                foreach(var monkey in monkeys)
                {
                    while(monkey.StartingItems.TryDequeue(out ulong item))
                    {
                        monkey.NbInspectedItems++;
                        item = monkey.WorryOpertation(item) % modulo;
                        int throwId = monkey.Test(item) ? monkey.TrueId : monkey.FalseId;
                        lookUp[throwId].StartingItems.Enqueue(item);
                    }
                }
            }
            var orederMonkeys = monkeys.Select(m => (ulong)m.NbInspectedItems).OrderByDescending(i => i).ToArray();
            return (orederMonkeys[0] * orederMonkeys[1]).ToString();
        }

        private Func<ulong, ulong> GetOperation(string v)
        {
            var plus = v.Split(" + ");
            if(plus.Length == 2)
            {
                var plusOperand = ulong.Parse(plus[1]);
                return (old => old + plusOperand);
            }
            var mult = v.Split(" * ");
            if(mult[1] == "old")
            {
                return (old => old * old);
            }
            var multOperand = ulong.Parse(mult[1]);
            return (old => old * multOperand);
        }
    }

    [Day(ExpectedValue = "10605")]
    public class Day11Puzzle1 : IDay
    {
        public string GetPuzzle(string input)
        {
            List<Monkey> monkeys = new List<Monkey>();
            foreach(var group in input.Split(Environment.NewLine + Environment.NewLine))
            {
                var monkeyDefinition = group.Split(Environment.NewLine);
                Monkey monkey = new Monkey()
                {
                    Id = monkeyDefinition[0][7] - (int)'0',
                    StartingItems = new Queue<ulong>(monkeyDefinition[1][18..].Split(',').Select(v =>v.Trim()).Select(v => ulong.Parse(v))),
                    WorryOpertation = GetOperation(monkeyDefinition[2][19..]),
                    Test = (worry => worry % ulong.Parse(monkeyDefinition[3][21..]) == 0),
                    TrueId = int.Parse(monkeyDefinition[4][29..]),
                    FalseId = int.Parse(monkeyDefinition[5][30..]),
                };
                monkeys.Add(monkey);
            }
            var lookUp = monkeys.ToDictionary(m => m.Id, m => m);
            for(int round = 0; round < 20; round++)
            {
                foreach(var monkey in monkeys)
                {
                    while(monkey.StartingItems.TryDequeue(out ulong item))
                    {
                        monkey.NbInspectedItems++;
                        item = monkey.WorryOpertation(item) / 3;
                        int throwId = monkey.Test(item) ? monkey.TrueId : monkey.FalseId;
                        lookUp[throwId].StartingItems.Enqueue(item);
                    }
                }
            }
            var orederMonkeys = monkeys.Select(m => m.NbInspectedItems).OrderByDescending(i => i).ToArray();
            return (orederMonkeys[0] * orederMonkeys[1]).ToString();
        }

        private Func<ulong, ulong> GetOperation(string v)
        {
            var plus = v.Split(" + ");
            if(plus.Length == 2)
            {
                var plusOperand = ulong.Parse(plus[1]);
                return (old => old + plusOperand);
            }
            var mult = v.Split(" * ");
            if(mult[1] == "old")
            {
                return (old => old * old);
            }
            var multOperand = ulong.Parse(mult[1]);
            return (old => old * multOperand);
        }
    }

    public class Monkey
    {
        public int Id { get; set; }
        public Queue<ulong> StartingItems { get; set; } = new Queue<ulong>();
        public Func<ulong, ulong> WorryOpertation { get; set; }

        public Func<ulong, bool> Test { get; set; }
        public int TrueId { get; set; }
        public int FalseId { get; set; }
        public int Diviser { get; set; }
        public int NbInspectedItems { get; set; } = 0;
    }
}
