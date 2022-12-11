namespace AoC.Day1
{
    [Day(ExpectedValue = "45000")]
    public class Day01Puzzle2 : IDay
    {
        public string GetPuzzle(string input)
        {
            var elves = input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split($"{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries).Select(v => int.Parse(v)).Sum());

            return elves.OrderByDescending(e => e).Take(3).Sum().ToString();
        }
    }

    [Day(ExpectedValue = "24000")]
    public class Day01Puzzle1 : IDay
    {
        public string GetPuzzle(string input)
        {
            var elves = input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split($"{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries).Select(v => int.Parse(v)).Sum());

            return elves.Max().ToString();
        }
    }
}
