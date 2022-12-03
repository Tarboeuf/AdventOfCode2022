namespace AoC.Day3
{

    [Day(ExpectedValue = "70")]
    public class Day3Puzzle2 : IDay
    {

        public string GetPuzzle(string input)
        {
            return GetThreeItems(input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                .Select(s => s[0].Intersect(s[1]).Intersect(s[2]).First())
                .Select(c => char.IsUpper(c) ? (c - 'A' + 27) : (c - 'a' + 1))
                .Sum().ToString();
        }

        private IEnumerable<List<string>> GetThreeItems(IEnumerable<string> lines)
        {
            List<string> group= new List<string>();
            foreach(var line in lines)
            {
                group.Add(line);
                if(group.Count == 3)
                {
                    yield return group;
                    group = new List<string>();
                }
            }
        }
    }

    [Day(ExpectedValue = "157")]
    public class Day3Puzzle1 : IDay
    {
        public string GetPuzzle(string input)
        {
            return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s[..(s.Length/2)].Intersect(s[(s.Length/2)..]).Single())
                .Select(c => char.IsUpper(c) ? (c - 'A' + 27) : (c - 'a' + 1))
                .Sum().ToString();
        }
    }
}
