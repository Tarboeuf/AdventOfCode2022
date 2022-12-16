namespace AoC.Day2
{

    [Day(ExpectedValue = "12")]
    public class Day02Puzzle2 : IDay
    {
        private static Dictionary<char, Chifoumi> Cyphers = new Dictionary<char, Chifoumi>()
            {
                {'A', Chifoumi.Rock },
                {'B', Chifoumi.Paper },
                {'C', Chifoumi.Scissors },
            };

        private static Dictionary<char, RoundStatus> RoundStatuses = new Dictionary<char, RoundStatus>()
            {
                {'X', RoundStatus.Lose },
                {'Y', RoundStatus.Draw},
                {'Z', RoundStatus.Win},
            };



        public string GetPuzzle(string input, bool isRealCase)
        {
            (Chifoumi,RoundStatus) GetLine(string[] values)
            {
                return (Cyphers[values[0][0]], RoundStatuses[values[1][0]]);
            }
            var combinations = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => GetLine(s.Split(" ", StringSplitOptions.RemoveEmptyEntries)));

            return combinations.Select(ComputeScore).Sum().ToString();
        }

        private int ComputeScore((Chifoumi, RoundStatus) values)
        {
            var myValue = values switch
            {
                (Chifoumi.Paper, RoundStatus.Win) => Chifoumi.Scissors,
                (Chifoumi.Rock, RoundStatus.Win) => Chifoumi.Paper,
                (Chifoumi.Scissors, RoundStatus.Win) => Chifoumi.Rock,
                (Chifoumi.Paper, RoundStatus.Lose) => Chifoumi.Rock,
                (Chifoumi.Rock, RoundStatus.Lose) => Chifoumi.Scissors,
                (Chifoumi.Scissors, RoundStatus.Lose) => Chifoumi.Paper,
                (Chifoumi.Paper, RoundStatus.Draw) => Chifoumi.Paper,
                (Chifoumi.Rock, RoundStatus.Draw) => Chifoumi.Rock,
                (Chifoumi.Scissors, RoundStatus.Draw) => Chifoumi.Scissors,
                _ => throw new NotImplementedException(),
            };

            return (int)myValue + (int)values.Item2;
        }
    }

    [Day(ExpectedValue = "15")]
    public class Day02Puzzle1 : IDay
    {
        private static Dictionary<char, Chifoumi> Cyphers = new Dictionary<char, Chifoumi>()
            {
                {'A', Chifoumi.Rock },
                {'B', Chifoumi.Paper },
                {'C', Chifoumi.Scissors },
                {'X', Chifoumi.Rock },
                {'Y', Chifoumi.Paper },
                {'Z', Chifoumi.Scissors },
            };

        public string GetPuzzle(string input, bool isRealCase)
        {
            var combinations = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(v => Cyphers[v[0]]).ToArray());

            return combinations.Select(ComputeScore).Sum().ToString();
        }

        private int ComputeScore(Chifoumi[] values)
        {
            var loseValue = values[1] switch
            {
                Chifoumi.Rock => Chifoumi.Paper,
                Chifoumi.Paper => Chifoumi.Scissors,
                Chifoumi.Scissors => Chifoumi.Rock,
                _ => throw new NotImplementedException()
            };

            return (int)values[1] + GetScore(values[0], values[1], loseValue);
        }

        private int GetScore(Chifoumi chifoumi, Chifoumi draw, Chifoumi lose)
        {
            return chifoumi == draw ? 3 : chifoumi == lose ? 0 : 6;
        }
    }

    public enum Chifoumi
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    public enum RoundStatus
    {
        Lose = 0,
        Draw = 3,
        Win = 6,
    }
}
