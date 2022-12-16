using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace AoC.Day6
{

    [Day(ExpectedValue = "19")]
    public class Day06Puzzle2 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            for(int i = 0; i < input.Length; i++)
            {
                if(input[i..(14+i)].Distinct().Count() == 14)
                {
                    return (i+14).ToString();
                }
            }
            return "";
        }
    }

    [Day(ExpectedValue = "7")]
    public class Day06Puzzle1 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            for(int i = 0; i < input.Length; i++)
            {
                if(input[i..(4+i)].Distinct().Count() == 4)
                {
                    return (i+4).ToString();
                }
            }
            return "";
        }
    }
}
