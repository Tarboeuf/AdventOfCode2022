using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace AoC.Day8
{
    public class LocalTest
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(2, Day8Puzzle2Test.GetScore( new byte[]{0, 5,1,2}, 4, 3, x => x - 1));
            Assert.Equal(2, Day8Puzzle2Test.GetScore( new byte[]{5,1,2}, 3, 2, x => x - 1));
            Assert.Equal(2, Day8Puzzle2Test.GetScore( new byte[]{5,1,2}, 3, 0, x => x + 1));
            Assert.Equal(1, Day8Puzzle2Test.GetScore( new byte[]{5,1,2}, 3, 1, x => x + 1));
            Assert.Equal(0, Day8Puzzle2Test.GetScore( new byte[]{5,1,2}, 3, 2, x => x + 1));
        }
    }
    
    [Day(ExpectedValue = "2")]
    public class Day8Puzzle2Test : IDay
    {
        public string GetPuzzle(string input)
        {
            byte[] bytes= new byte[]{0, 5, 1, 2};
            return GetScore(bytes, bytes.Length, 3, x => x - 1).ToString();
        }
        public static int GetScore(byte[] trees, int width, int x, Func<int, int> funcX)
        {
            int newX = funcX(x);
            
            if(newX < 0 || newX == width)
            {
                return 0;
            }
            if (trees[x] < trees[newX])
            {
                return 1;
            }
            if (trees[x] == trees[newX])
            {
                return 1 + GetScore(trees, width, newX, funcX);;
            }
            return 1 + GetScore(trees, width, newX, trees[newX], funcX);
        }
        
        public static int GetScore(byte[] trees, int width, int x, byte value, Func<int, int> funcX)
        {
            int newX = funcX(x);
            if (value > trees[newX])
            {
                return 0;
            }
            if(newX == 0 || newX == width - 1)
            {
                return 1;
            }
            return 1 + GetScore(trees, width, newX, value, funcX);
        }
    }
    [Day(ExpectedValue = "8")]
    public class Day8Puzzle2// : IDay
    {
        public string GetPuzzle(string input)
        {
            var trees = input.Split(Environment.NewLine)
                                .Select(s => s.Select(c => byte.Parse(c.ToString())).ToArray())
                                .ToArray();

            int height = trees.Length;
            int width = trees[0].Length;

            List<int> scores = new List<int>();
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    scores.Add(GetScore(trees, width, height, x, y,trees[x][y], x => x, y => y - 1)
                        * GetScore(trees, width, height, x, y,trees[x][y], x => x, y => y + 1)
                        * GetScore(trees, width, height, x, y,trees[x][y], x => x - 1, y => y)
                        * GetScore(trees, width, height, x, y,trees[x][y], x => x + 1, y => y));
                }
            }

            return (scores.Max()).ToString();
        }

        public int GetScore(byte[][] trees, int width, int height, int x, int y, byte value, Func<int, int> funcX, Func<int, int> funcY)
        {
            int newX = funcX(x);
            int newY = funcY(y);
            if (value > trees[newX][newY])
            {
                return 1;
            }
            if (newX == 0 || newX == width - 1 || newY == 0 || newY == height - 1)
            {
                return 0;
            }
            return 1 + GetScore(trees, width, height, newX, newY, value, funcX, funcY);
        }
    }

    [Day(ExpectedValue = "21")]
    public class Day8Puzzle1 : IDay
    {
        public string GetPuzzle(string input)
        {
            var trees = input.Split(Environment.NewLine)
                                .Select(s => s.Select(c => byte.Parse(c.ToString())).ToArray())
                                .ToArray();

            int height = trees.Length;
            int width = trees[0].Length;

            int nbVisibleTrees = 0;
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (IsVisible(trees, width, height, x, y,trees[x][y], x => x, y => y - 1)
                        || IsVisible(trees, width, height, x, y,trees[x][y], x => x, y => y + 1)
                        || IsVisible(trees, width, height, x, y,trees[x][y], x => x - 1, y => y)
                        || IsVisible(trees, width, height, x, y,trees[x][y], x => x + 1, y => y))
                    {
                        nbVisibleTrees++;
                    }
                }
            }

            return (height * 2 + width * 2 - 4 + nbVisibleTrees).ToString();
        }

        public bool IsVisible(byte[][] trees, int width, int height, int x, int y, byte value, Func<int, int> funcX, Func<int, int> funcY)
        {
            int newX = funcX(x);
            int newY = funcY(y);
            if (value <= trees[newX][newY])
            {
                return false;
            }
            if (newX == 0 || newX == width - 1 || newY == 0 || newY == height - 1)
            {
                return true;
            }
            return IsVisible(trees,  width, height, newX, newY, value, funcX, funcY);
        }
    }
}
