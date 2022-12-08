using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace AoC.Day8
{
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

            var visibilities = new bool?[width,height];
            int nbVisibleTrees = 0;
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (IsVisible(trees, visibilities, width, height, x, y, x => x, y => y - 1)
                        || IsVisible(trees, visibilities, width, height, x, y, x => x, y => y + 1)
                        || IsVisible(trees, visibilities, width, height, x, y, x => x - 1, y => y)
                        || IsVisible(trees, visibilities, width, height, x, y, x => x + 1, y => y))
                    {
                        nbVisibleTrees++;
                    }
                }
            }

            return (height * 2 + width * 2 - 4 + nbVisibleTrees).ToString();
        }

        public bool IsVisible(byte[][] trees, bool?[,] visibilities, int width, int height, int x, int y, Func<int, int> funcX, Func<int, int> funcY, bool needHigher = true)
        {
            int newX = funcX(x);
            int newY = funcY(y);
            if (trees[x][y] < trees[newX][newY])
            {
                visibilities[x,y] = false;
                return false;
            }
            // if(visibilities[newX, newY] != null)
            // {
            //     return visibilities[newX, newY]!.Value;
            // }
            needHigher &= (trees[x][y] == trees[newX][newY]);
            if (newX == 0 || newX == width - 1 || newY == 0 || newY == height - 1)
            {
                if(!needHigher)
                {
                    visibilities[newX, newY] = true;
                }
                return !needHigher;
            }
            visibilities[x,y] = IsVisible(trees, visibilities, width, height, newX, newY, funcX, funcY, needHigher);
            return visibilities[x,y]!.Value;
        }
    }
}
