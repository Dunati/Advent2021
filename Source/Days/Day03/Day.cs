namespace Day03;


class Day : BaseDay
{
    public override string Run(int part, string rawData)
    {
        if (part == 1)
            return Part1(rawData);

        return Part2(rawData);
    }

    int O2(ArraySegment<int> values, int bits)
    {
        int mask = 1 << (bits);
        while (values.Count > 1)
        {
            mask >>= 1;
            values = values.OrderBy(x => x & mask).ToArray();
            int count = 0;

            while (count < values.Count && ((values[count] & mask) == 0))
            {
                count++;
            }

            if (count <= (values.Count - count))
            {
                values = values[count..];
            }
            else
            {
                values = values[0..count];
            }
        }
        return values[0];
    }

    private int CO2(ArraySegment<int> values, int bits)
    {
        int mask = 1 << (bits);
        while (values.Count > 1)
        {
            mask >>= 1;
            values = values.OrderBy(x => x & mask).ToArray();
            int count = 0;

            while (count < values.Count && ((values[count] & mask) == 0))
            {
                count++;
            }

            if (count <= (values.Count - count))
            {
                values = values[0..count];
            }
            else
            {
                values = values[count..];
            }
        }
        return values[0];
    }

    private string Part2(string rawData)
    {
        var lines = rawData.Lines();

        int total = lines.Count();
        int bits = lines.First().Length;
        ArraySegment<int> values = rawData.ToInts(2).ToArray();


        var o2 = O2(values, bits);
        var co2 = CO2(values, bits);


        //4425732 too low
        return (o2*co2).ToString();
    }

    private static string Part1(string rawData)
    {
        int[] counts = new int[rawData.Lines().First().Length];
        int total = 0;
        foreach (var line in rawData.Lines())
        {
            total++;
            for (int i = 0; i < line.Length; i++)
            {
                counts[i] += line[i] - '0';
            }
        }
        int gamma = 0;
        for (int i = 0; i < counts.Length; i++)
        {
            gamma <<= 1;
            gamma |= counts[i] > total / 2 ? 1 : 0;
        }

        int beta = ((1 << counts.Length) - 1) & (~gamma);

        return (gamma * beta).ToString();
    }
}
