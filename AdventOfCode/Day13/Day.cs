using AdventOfCode;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace AoC.Day13
{
    [Day(ExpectedValue = "140")]
    public class Day13Puzzle2 : Day13PuzzleBase, IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var packet2 = GetPacket("[[2]]");
            var packet6 = GetPacket("[[6]]");
            var orderedList = input.GetLines()
                .Select(GetPacket)
                .Include(packet2)
                .Include(packet6)
                .OrderBy(p => p, new PacketComparer())
                .ToList();

            return ((orderedList.IndexOf(packet2) + 1) * (orderedList.IndexOf(packet6) + 1)).ToString();
        }
    }


    [Day(ExpectedValue = "13")]
    public abstract class Day13PuzzleBase
    {
        protected PacketBase GetPacket(string value)
        {
            if (value[0] == '[' && value[^1] == ']')
            {
                List<int> commaOutsideBrackets = new List<int>();
                int nest = 0;
                for (int i = 1; i < value.Length - 2; i++)
                {
                    if (value[i] == ',' && nest == 0)
                    {
                        commaOutsideBrackets.Add(i);
                    }
                    if (value[i] == '[')
                    {
                        nest++;
                    }
                    if (value[i] == ']')
                    {
                        nest--;
                    }
                }
                Packet packet = new Packet();
                if (value.Length == 2)
                {
                    return packet;
                }

                int previous = 1;
                foreach (int i in commaOutsideBrackets)
                {
                    packet.Packets.Add(GetPacket(value[previous..i]));
                    previous = i + 1;
                }
                packet.Packets.Add(GetPacket(value[previous..^1]));

                return packet;
            }
            return new PacketInt
            {
                Value = int.Parse(value),
            };
        }
    }

    [Day(ExpectedValue = "13")]
    public class Day13Puzzle1 : Day13PuzzleBase, IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.GetLines(Environment.NewLine)
                .Select(g => g.GetLines().Select(GetPacket).ToList())
                .Select((v, i) => (v, i + 1))
                .Where(v => IsRightOrder(v.v))
                .Select(c => c.Item2)
                .Sum().ToString();
        }


        private bool IsRightOrder(List<PacketBase> packets)
        {
            PacketBase left = packets[0];
            PacketBase right = packets[1];

            var result = left.CompareTo(right) < 0;

            return result;
        }
    }

    public class PacketComparer : IComparer<PacketBase>
    {
        public int Compare(PacketBase? x, PacketBase? y)
        {
            return x?.CompareTo(y) ?? (y == null ? 0 : 1);
        }
    }

    public abstract class PacketBase : IComparable<PacketBase>
    {
        public abstract int CompareTo(PacketBase? other);
    }

    public class PacketInt : PacketBase
    {
        public int Value { get; set; }

        public override int CompareTo(PacketBase? other)
        {
            switch (other)
            {
                case PacketInt packetInt: return Value.CompareTo(packetInt.Value);
                case Packet packet:
                    return Packet.Compare(new List<PacketBase> { this }, packet.Packets);
                default:
                    throw new InvalidOperationException();
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Packet : PacketBase
    {
        public List<PacketBase> Packets { get; } = new List<PacketBase>();

        public override int CompareTo(PacketBase? other)
        {
            switch (other)
            {
                case PacketInt packetInt: return Compare(Packets, new List<PacketBase> { packetInt });
                case Packet packet:
                    return Compare(Packets, packet.Packets);
                default:
                    throw new InvalidOperationException();
            }
        }

        public static int Compare(List<PacketBase> left, List<PacketBase> right)
        {
            int commonValues = Math.Min(left.Count, right.Count);
            for (int i = 0; i < commonValues; i++)
            {
                var comparison = left[i].CompareTo(right[i]);
                if(comparison != 0)
                    return comparison;
            }
            return  left.Count - right.Count;
        }


        public override string ToString()
        {
            return $"[{string.Join(',', Packets)}]";
        }
    }
}
