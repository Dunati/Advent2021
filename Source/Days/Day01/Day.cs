namespace Day01;


class Day : BaseDay
{
    public override string Run(int part, string rawData)
    {
        if (part == 1)
            return Part1(rawData);
        var depth = rawData.ToInts().ToArray();
        int sum = depth[0] + depth[1] + depth[2];
        int greater = 0;
        for (int i = 3; i < depth.Length; i++)
        {
            int newsum = sum - depth[i - 3] + depth[i];
            if (newsum > sum)
            {
                greater++;

            }
            sum = newsum;
        }
        return greater.ToString();

    }

    private static string Part1(string rawData)
    {
        var depth = rawData.ToInts().ToArray();
        int greater = 0;
        for (int i = 1; i < depth.Length; i++)
        {
            if (depth[i] > depth[i - 1])
            {
                greater++;

            }
        }
        return greater.ToString();
    }
}
