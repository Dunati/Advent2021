

namespace Day16;


class Day : BaseDay
{

    Dictionary<char, BitArray> digits = null;


    class BitStream
    {
        public BitStream Clone() => new BitStream() { bits = bits, Position = Position };
        public BitArray bits { get; init; }

        public int Position { get; set; } = 0;

        public uint ReadInt(int count)
        {
            uint result = bits.Extract(Position, count);
            Position += count;
            return result;
        }

        public ulong ReadLiteral()
        {
            ulong result = 0;
            ulong value = 0;
            do
            {
                Debug.Assert(result <= (result << 4));
                result <<= 4;
                value = ReadInt(5);
                result |= value & 0b0_1111;
            } while ((value & 0b1_0000) != 0);
            return result;
        }

    }


    const int OpSum = 0;
    const int OpProduct = 1;
    const int OpMin = 2;
    const int OpMax = 3;
    const int OpLiteral = 4;
    const int OpGreater = 5;
    const int OpLess = 6;
    const int OpEqual = 7;

    uint VersionSum = 0;

    decimal ReadPacket(BitStream stream)
    {
        int startPosition = stream.Position;
        uint version = stream.ReadInt(3);
        VersionSum += version;
        uint type = stream.ReadInt(3);

        string opString = $"V:{version} ";

        decimal value = 0;


        if (type == OpLiteral)
        {
            value = stream.ReadLiteral();
            if (InTest)
            {
                opString += $"LIT:{value}";
            }
        }
        else
        {
            List<decimal> args = new();

            uint lengthType = stream.ReadInt(1);
            if (lengthType == 0)
            {
                uint end = stream.ReadInt(15);
                opString += $"LEN:{end} H:{stream.Position - startPosition}";
                end += (uint)stream.Position;

                while (stream.Position < end)
                {
                    args.Add(ReadPacket(stream));
                }
                Debug.Assert(stream.Position == end);
            }
            else
            {
                uint subPackets = stream.ReadInt(11);
                opString += $"CNT:{subPackets} H:{stream.Position - startPosition}";
                for (uint i = 0; i < subPackets; i++)
                {
                    args.Add(ReadPacket(stream));
                }
            }

            switch (type)
            {
                case OpSum:
                    value = args.Aggregate((x, y) => x + y);
                    break;
                case OpProduct:
                    value = args.Aggregate((x, y) => x * y);
                    break;
                case OpMin:
                    value = args.Aggregate((x, y) => x < y ? x : y);
                    break;
                case OpMax:
                    value = args.Aggregate((x, y) => x > y ? x : y);
                    break;
                case OpLess:
                    value = args[0] < args[1] ? 1 : 0;
                    break;
                case OpGreater:
                    value = args[0] > args[1] ? 1 : 0;
                    break;
                case OpEqual:
                    value = args[0] == args[1] ? 1 : 0;
                    break;
                default:
                    Debug.Assert(type == uint.MaxValue);break;
            }
        }
        if (InTest)
        {
            if (startPosition >= opString.Length)
            {
                opString += " ";
                Trace.WriteLine(opString.PadLeft(startPosition) + stream.bits.ToBinaryString()[startPosition..stream.Position]);
            }
            else
            {
                Trace.WriteLine(opString);
                Trace.WriteLine(stream.bits.ToBinaryString()[startPosition..stream.Position]);
            }
        }
        return value;
    }

    public override string Run(int part, string rawData)
    {
        InTest = true;
        VersionSum = 0;
        rawData = rawData.Lines().First();
        digits = new()
        {
            { '0', new BitArray(new int[] { 00 }).ZeroExtend(rawData.Length * 4) },
            { '1', new BitArray(new int[] { 01 }).ZeroExtend(rawData.Length * 4) },
            { '2', new BitArray(new int[] { 02 }).ZeroExtend(rawData.Length * 4) },
            { '3', new BitArray(new int[] { 03 }).ZeroExtend(rawData.Length * 4) },
            { '4', new BitArray(new int[] { 04 }).ZeroExtend(rawData.Length * 4) },
            { '5', new BitArray(new int[] { 05 }).ZeroExtend(rawData.Length * 4) },
            { '6', new BitArray(new int[] { 06 }).ZeroExtend(rawData.Length * 4) },
            { '7', new BitArray(new int[] { 07 }).ZeroExtend(rawData.Length * 4) },
            { '8', new BitArray(new int[] { 08 }).ZeroExtend(rawData.Length * 4) },
            { '9', new BitArray(new int[] { 09 }).ZeroExtend(rawData.Length * 4) },
            { 'A', new BitArray(new int[] { 10 }).ZeroExtend(rawData.Length * 4) },
            { 'B', new BitArray(new int[] { 11 }).ZeroExtend(rawData.Length * 4) },
            { 'C', new BitArray(new int[] { 12 }).ZeroExtend(rawData.Length * 4) },
            { 'D', new BitArray(new int[] { 13 }).ZeroExtend(rawData.Length * 4) },
            { 'E', new BitArray(new int[] { 14 }).ZeroExtend(rawData.Length * 4) },
            { 'F', new BitArray(new int[] { 15 }).ZeroExtend(rawData.Length * 4) },
        };
        BitArray bits = new BitArray(rawData.Length * 4);
        foreach (var c in rawData)
        {
            bits.LeftShift(4);
            bits = bits.Or(digits[c]);
        }

        if (InTest)
        {
            Trace.WriteLine($"------------\n{rawData}\n-----------");
        }

        BitStream stream = new BitStream() { bits = bits };

        var value =ReadPacket(stream);


        //15006 too high
        //335 too low
        //866
        if (part == 1)
        {
            return VersionSum.ToString();
        }
        //1391978883022 too low
        //1392637195518
        return value.ToString();
    }
}
