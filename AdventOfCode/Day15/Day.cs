using AdventOfCode;
using Clipper2Lib;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace AoC.Day15
{
    [Day(ExpectedValue = "56000011")]
    public class Day15Puzzle2 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            List<((int x, int y) s, (int x, int y) b, int dist)> coupleSensorsBeacons = input.GetLines()
                .Select(GetCouples)
                .Select(c => (c.s, c.b, GetDistance(c.b, c.s)))
                .ToList();

            int maxHeight = coupleSensorsBeacons.Select(c => new[] { c.b.y, c.s.y + c.dist }.Max())
                .Max();

            var beacons = coupleSensorsBeacons.Select(c => c.b).ToHashSet();

            int max = isRealCase ? 4000000 : 20;
            ExportImage(coupleSensorsBeacons, maxHeight, max);

            Paths64 pathes = new Paths64();
            foreach (var item in coupleSensorsBeacons)
            {
                var p = new List<Path64> { new Path64(new[] { new Point64(item.s.x - item.dist, item.s.y), new Point64(item.s.x, item.s.y - item.dist), new Point64(item.s.x + item.dist, item.s.y), new Point64(item.s.x, item.s.y + item.dist) }) };
                pathes = Clipper.Union(new Paths64(p), pathes, FillRule.EvenOdd);
            }

            var area = new List<Path64> { new Path64(new[] { new Point64(0, 0), new Point64(max, 0), new Point64(max, max), new Point64(0, max) }) };
            var result = Clipper.Difference(new Paths64(area), pathes, FillRule.NonZero);
            var point = result.First(r => r.Count == 4);
            var x = point.Select(v => v.X).GroupBy(p => p).Where(g => g.Count() == 2).First().Key;
            var y = point.Select(v => v.Y).GroupBy(p => p).Where(g => g.Count() == 2).First().Key;
            return ((x * 4000000) + y).ToString();
        }

        private void ExportImage(List<((int x, int y) s, (int x, int y) b, int dist)> coupleSensorsBeacons, int maxHeight, int max)
        {
            //int coef = maxHeight > 10000 ? 1000 : 1;
            //var losange = new[] { RotatePointF(0, 0, coef), RotatePointF(0, max, coef), RotatePointF(max, max, coef), RotatePointF(max, 0, coef) };
            //var imageHeight = (int)losange.Select(p => p.Y).Max();
            //var delta = (int)losange.Select(p => p.X).Min();
            //var imageWidth = (int)losange.Select(p => p.X).Max() - delta;
            //Bitmap bitmap = new Bitmap((int)imageWidth, (int)imageHeight); // or some other format

            //using (Graphics g = Graphics.FromImage(bitmap))
            //{
            //    // Modify the image using g here... 
            //    // Create a brush with an alpha value and use the g.FillRectangle function
            //    using (System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White))
            //    {
            //        Pen pen = new Pen(myBrush);
            //        foreach (var item in coupleSensorsBeacons)
            //        {
            //            var center = RotatePoint(item.s);

            //            g.FillRectangle(myBrush, new Rectangle(
            //                (int)(center.x - item.dist) / coef - delta,
            //                (int)(center.y - item.dist) / coef,
            //                item.dist * 2 / coef,
            //                item.dist * 2 / coef));
            //        }
            //    }
            //    using (System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black))
            //    {
            //        Pen pen = new Pen(myBrush);

            //        foreach (var item in coupleSensorsBeacons)
            //        {
            //            var center = RotatePoint(item.s);

            //            g.DrawRectangle(pen, new Rectangle(
            //                (int)(center.x - item.dist) / coef - delta,
            //                (int)(center.y - item.dist) / coef,
            //                item.dist * 2 / coef,
            //                item.dist * 2 / coef));
            //        }
            //    }
            //    using (System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue))
            //    {
            //        Pen pen = new Pen(myBrush);



            //        g.DrawPolygon(pen, losange.Select(p => p - new SizeF(delta, 0)).ToArray());

            //    }
            //    string filename = maxHeight > 10000 ? "real.bmp" : "test.bmp";
            //    bitmap.Save(filename);
            //}
        }

        private int GetDistance((int x, int y) s, (int x, int y) b)
        {
            return Math.Abs(s.x - b.x) + Math.Abs(s.y - b.y);
        }

        private ((int x, int y) s, (int x, int y) b) GetCouples(string line)
        {
            line = line.Replace("Sensor at ", "").Replace(" closest beacon is at ", "");
            var couple = line.Split(':');
            return (GetCoord(couple[0]), GetCoord(couple[1]));
        }
        public (int x, int y) GetCoord(string coord)
        {
            var split = coord.Split(", ");
            return (int.Parse(split[0][2..]), int.Parse(split[1][2..]));
        }

        public (double x, double y) RotatePoint((int x, int y) point)
        {
            var a = 45d * System.Math.PI / 180.0;
            double cosa = Math.Cos(a), sina = Math.Sin(a);
            return (point.x * cosa - point.y * sina, point.x * sina + point.y * cosa);
        }
        public PointF RotatePointF(int x, int y, int coef)
        {
            var a = 45d * System.Math.PI / 180.0;
            float cosa = (float)Math.Cos(a), sina = (float)Math.Sin(a);
            return new PointF((x * cosa - y * sina) / coef, (x * sina + y * cosa) / coef);
        }

    }

    [Day(ExpectedValue = "26")]
    public class Day15Puzzle1 : IDay
    {

        public string GetPuzzle(string input, bool isRealCase)
        {
            List<((int x, int y) s, (int x, int y) b, int dist)> coupleSensorsBeacons = input.GetLines()
                .Select(GetCouples)
                .Select(c => (c.s, c.b, GetDistance(c.b, c.s)))
                .ToList();

            int minWidth = coupleSensorsBeacons.Select(c => new[] { c.b.x, c.s.x - c.dist }.Min())
                .Min();
            int maxWidth = coupleSensorsBeacons.Select(c => new[] { c.b.x, c.s.x + c.dist }.Max())
                .Max();

            int minHeight = coupleSensorsBeacons.Select(c => new[] { c.b.y, c.s.y - c.dist }.Min())
                .Min();
            int maxHeight = coupleSensorsBeacons.Select(c => new[] { c.b.y, c.s.y + c.dist }.Max())
                .Max();

            var beacons = coupleSensorsBeacons.Select(c => c.b).ToHashSet();


            int nbCoveredPositions = 0;
            int y = isRealCase ? 2000000 : 10;
            for (int x = minWidth; x <= maxWidth; x++)
            {
                if (beacons.Contains((x, y)))
                {
                    continue;
                }
                foreach (var item in coupleSensorsBeacons)
                {
                    if (item.dist >= GetDistance(item.s, (x, y)))
                    {
                        nbCoveredPositions++;
                        break;
                    }
                }
            }

            return nbCoveredPositions.ToString();
        }

        private int GetDistance((int x, int y) s, (int x, int y) b)
        {
            return Math.Abs(s.x - b.x) + Math.Abs(s.y - b.y);
        }

        private ((int x, int y) s, (int x, int y) b) GetCouples(string line)
        {
            line = line.Replace("Sensor at ", "").Replace(" closest beacon is at ", "");
            var couple = line.Split(':');
            return (GetCoord(couple[0]), GetCoord(couple[1]));

        }
        public (int x, int y) GetCoord(string coord)
        {
            var split = coord.Split(", ");
            return (int.Parse(split[0][2..]), int.Parse(split[1][2..]));
        }
    }
}
