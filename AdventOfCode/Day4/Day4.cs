namespace AoC.Day4
{

    [Day(ExpectedValue = "4")]
    public class Day04Puzzle2 : IDay
    {

        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(GetAllValues).ToArray())
                .Where(v => v[0].Intersect(v[1]).Count() >= 1)
                .Count().ToString();
        }


        private HashSet<int> GetAllValues(string range)
        {
            var split = range.Split('-');
            HashSet<int> result = new HashSet<int>();
            for(int i = int.Parse(split[0]); i <= int.Parse(split[1]); i++)
            {
                result.Add(i);
            }
            return result;
        }
    }

    [Day(ExpectedValue = "2")]
    public class Day04Puzzle1 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(GetAllValues).ToArray())
                .Where(v => v[0].Intersect(v[1]).Count() == Math.Min(v[0].Count, v[1].Count))
                .Count().ToString();
        }

        private HashSet<int> GetAllValues(string range)
        {
            var split = range.Split('-');
            HashSet<int> result = new HashSet<int>();
            for(int i = int.Parse(split[0]); i <= int.Parse(split[1]); i++)
            {
                result.Add(i);
            }
            return result;
        }
    }
}
