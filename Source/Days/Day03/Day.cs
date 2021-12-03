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
        ArraySegment<int> aux = new int[values.Count];
        int mask = 1 << (bits);
        while (values.Count > 1)
        {
            var original = values;
            int zeros = 0;
            int ones = 0;
            mask >>= 1;

            foreach (int value in values)
            {
                if ((value & mask) == 0)
                {
                    aux[zeros++] = value;
                }
                else
                {
                    aux[^++ones] = value;
                }
            }

            if (ones >= zeros)
            {
                values = aux[^ones..];
            }
            else
            {
                values = aux[0..zeros];
            }
            aux = original;
            
        }
        return values[0];
    }

    private int CO2(ArraySegment<int> values, int bits)
    {
        ArraySegment<int> aux = new int[values.Count];
        int mask = 1 << (bits);
        while (values.Count > 1)
        {
            var original = values;
            int zeros = 0;
            int ones = 0;
            mask >>= 1;

            foreach (int value in values)
            {
                if ((value & mask) == 0)
                {
                    aux[zeros++] = value;
                }
                else
                {
                    aux[^++ones] = value;
                }
            }

            if (ones >= zeros)
            {
                values = aux[0..zeros];
            }
            else
            {
                values = aux[^ones..];
            }
            aux = original;

        }
        return values[0];
    }
    // in 1s    ms per op
    // 3758     0.2660989888238425      // old
    // 53913    0.01854840205516295     // new

    private string Part2(string rawData)
    {
        var lines = rawData.Lines();

        int total = lines.Count();
        int bits = lines.First().Length;
        ArraySegment<int> values = rawData.ToInts(2).ToArray();



        var o2 = O2(values.ToArray(), bits);
        var co2 = CO2(values.ToArray(), bits);

        //4425732 too low
        return (o2 * co2).ToString();
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
