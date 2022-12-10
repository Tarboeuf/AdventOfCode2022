using AdventOfCode;
using System.Text;

namespace AoC.Day10
{
    [Day(ExpectedValue = @"##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......###.
#######.......#######.......#######.....
")]
    public class Day10Puzzle2 : IDay
    {
        public string GetPuzzle(string input)
        {
            int cycleNumber = 1;
            int value = 1;
            StringBuilder result = new StringBuilder();
            foreach(var line in input.Split(Environment.NewLine))
            {
                switch(line[0..4])
                {
                    case "noop":
                        result.Append(GetChar(value, ref cycleNumber));
                        break;
                    case "addx":
                        result.Append(GetChar(value, ref cycleNumber));
                        result.Append(GetChar(value, ref cycleNumber));
                        value += int.Parse(line[5..]);
                        break;
                    default:
                        throw new ArgumentException(line);
                }
            }
            return result.ToString();
        }

        private string GetChar(int value, ref int cycleNumber)
        {
            var currentLineCycleNumber = (cycleNumber)%40;
            cycleNumber++;
            string result = (currentLineCycleNumber >= value && currentLineCycleNumber <= value + 2) ? "#" : ".";
            if(currentLineCycleNumber == 0)
            {
                return result + Environment.NewLine;
            }
            return result;
        }
    }

    [Day(ExpectedValue = "13140")]
    public class Day10Puzzle1 : IDay
    {
        public string GetPuzzle(string input)
        {
            int cycleNumber = 1;
            long value = 1;
            long result = 0;
            foreach(var line in input.Split(Environment.NewLine))
            {
                switch(line[0..4])
                {
                    case "noop":
                        result += IncreaseResult(value, ref cycleNumber);
                        break;
                    case "addx":
                        result += IncreaseResult(value, ref cycleNumber);
                        value += int.Parse(line[5..]);
                        result += IncreaseResult(value, ref cycleNumber);
                        break;
                    default:
                        throw new ArgumentException(line);
                }
            }
            return result.ToString();
        }

        private long IncreaseResult(long value, ref int cycleNumber)
        {
            cycleNumber++;
            if(cycleNumber%40 == 20)
            {
                return value * cycleNumber;
            }
            return 0;
        }
    }
}
