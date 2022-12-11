using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using AdventOfCode;

namespace AoC.Day8
{
    public class Day08PuzzleBase
    {
        protected int _height;
        protected int _width;
        protected byte[][] _trees = new byte[0][];

        protected void InitInput(string input)
        {
            _trees = input.Split(Environment.NewLine)
                                .Select(s => s.Select(c => byte.Parse(c.ToString())).ToArray())
                                .ToArray()!;


            _height = _trees.Length;
            _width = _trees[0].Length;
        }
    }

    [Day(ExpectedValue = "8")]
    public class Day08Puzzle2Test : Day08PuzzleBase, IDay
    {
        public string GetPuzzle(string input)
        {
            InitInput(input);

            return MyEnumerable.GetTableValues(_height, _width)
                .Select(v => VisibleCount(v.x, v.y))
                .Max().ToString();
        }

        private int VisibleCount(int x, int y)
        {
            if (x == 0 || x == _width - 1 || y == 0 || y == _height - 1) { return 0; }

            int tree = _trees[y][x];
            return GetScore(tree, x, y + 1, 0, 1) *
                GetScore(tree, x, y - 1, 0, -1) *
                GetScore(tree, x + 1, y, 1, 0) *
                GetScore(tree, x - 1, y, -1, 0);
        }

        private int GetScore(int tree, int x, int y, int xd, int yd)
        {
            int count = 0;
            while (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                count++;
                if (tree <= _trees[y][x])
                {
                    return count;
                }
                x += xd;
                y += yd;
            }
            return count;
        }
    }

    [Day(ExpectedValue = "21")]
    public class Day08Puzzle1 : Day08PuzzleBase, IDay
    {
        public string GetPuzzle(string input)
        {
            InitInput(input);

            int nbVisibleTrees = MyEnumerable.GetTableValues(_width - 1, _height - 1, 1, 1).Where(v =>
                        IsVisible(v.x, v.y, _trees[v.x][v.y], 0, -1)
                        || IsVisible(v.x, v.y, _trees[v.x][v.y], 0, 1)
                        || IsVisible(v.x, v.y, _trees[v.x][v.y], -1, 0)
                        || IsVisible(v.x, v.y, _trees[v.x][v.y], 1, 0))
                .Count();

            return (_height * 2 + _width * 2 - 4 + nbVisibleTrees).ToString();
        }

        public bool IsVisible(int x, int y, byte value, int xd, int yd)
        {
            int newX = x + xd;
            int newY = y + yd;
            if (value <= _trees[newX][newY])
            {
                return false;
            }
            if (newX == 0 || newX == _width - 1 || newY == 0 || newY == _height - 1)
            {
                return true;
            }
            return IsVisible(newX, newY, value, xd, yd);
        }
    }
}
