namespace Day08;


class Day : BaseDay
{
    int str(string s)
    {
        Trace.WriteLine(s);
        return 1;
    }
    public override string Run(int part, string rawData)
    {
        if (part == 2)
        {
            return Part2(rawData);
        }
        var lines =
        rawData.Lines().Select(line => line.Split("|")).Select(x => new { patterns = x[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray(), output = x[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray() });

        int count = 0;
        foreach (var line in lines)
        {
            foreach (var digit in line.output)
            {
                count += digit.Length switch
                {
                    2 => 1,
                    4 => 1,
                    3 => 1,
                    7 => 1,
                    _ => 0
                };
            }
        }
        return count.ToString();
    }

    private string Part2(string rawData)
    {
        var lines =
          rawData.Lines().Select(line => line.Split("|"));


        int total = 0;
        foreach (var line in lines)
        {
            total += Solve(line[0], line[1]);
        }

        return total.ToString();
    }

    void SetUnmapped(int[] mapped, string mapping, int index)
    {
        foreach (var c in mapping)
        {
            if (mapped[c] == -1)
            {
                mapped[c] = index;
                return;
            }
        }
        Debug.Assert(false);
    }


    const int A = 0; //8
    const int B = 1; //6
    const int C = 2; //8
    const int D = 3; //7
    const int E = 4; //4
    const int F = 5; //9
    const int G = 6; //7

    void SetUnmapped(string pattern, int[] mapping, int index)
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            if (mapping[pattern[i] - 'a'] == -1)
            {
                mapping[pattern[i] - 'a'] = index;
                return;
            }
        }
    }
    void SetUnmapped(int[] mapping, int index)
    {
        for (int i = 0; i < mapping.Length; i++)
        {
            if (mapping[i] == -1)
            {
                mapping[i] = index;
                return;
            }
        }
    }

    int Solve(string patterns, string digits)
    {
        int[] mapping = Enumerable.Repeat(-1, 7).ToArray();
        int[] frequency = new int[7];
        string one = null;
        string four = null;
        string seven = null;
        foreach (var pattern in patterns.Split(" ", StringSplitOptions.RemoveEmptyEntries))
        {
            switch (pattern.Length)
            {
                case 2: one = pattern; break;
                case 3: seven = pattern; break;
                case 4: four = pattern; break;
            }
            foreach (var c in pattern)
            {
                frequency[c - 'a']++;
            }
        }

        for (int i = 0; i < frequency.Length; i++)
        {
            switch (frequency[i])
            {
                case 6:
                    mapping[i] = B;
                    break;
                case 4:
                    mapping[i] = E;
                    break;
                case 9:
                    mapping[i] = F;
                    break;
            }
        }


        SetUnmapped(one, mapping, C);
        SetUnmapped(seven, mapping, A);
        SetUnmapped(four, mapping, D);
        SetUnmapped(mapping, G);

        int total = 0;
        foreach(var digit in digits.Split(" ", StringSplitOptions.RemoveEmptyEntries))
        {
            total *= 10;
            int mask = 0;
            foreach (var c in digit)
            {
                mask |= 1 << mapping[c-'a'];
            }
            total += digitMask[mask];
        }


        return total;
    }


    readonly static int[] masks = new int[]
    {
        (1<<A)|(1<<B)|(1<<C)|       (1<<E)|(1<<F)|(1<<G), // 0
                      (1<<C)|              (1<<F)       , // 1
        (1<<A)|       (1<<C)|(1<<D)|(1<<E)|       (1<<G), // 2
        (1<<A)|       (1<<C)|(1<<D)|       (1<<F)|(1<<G), // 3
               (1<<B)|(1<<C)|(1<<D)|       (1<<F)       , // 4
        (1<<A)|(1<<B)|       (1<<D)|       (1<<F)|(1<<G), // 5
        (1<<A)|(1<<B)|       (1<<D)|(1<<E)|(1<<F)|(1<<G), // 6
        (1<<A)|       (1<<C)|              (1<<F)       , // 7
        (1<<A)|(1<<B)|(1<<C)|(1<<D)|(1<<E)|(1<<F)|(1<<G), // 8
        (1<<A)|(1<<B)|(1<<C)|(1<<D)|       (1<<F)|(1<<G), // 9
    };

    readonly static Dictionary<int, int> digitMask = new()
    {
        { masks[0], 0 },
        { masks[1], 1 },
        { masks[2], 2 },
        { masks[3], 3 },
        { masks[4], 4 },
        { masks[5], 5 },
        { masks[6], 6 },
        { masks[7], 7 },
        { masks[8], 8 },
        { masks[9], 9 },
    };
}
