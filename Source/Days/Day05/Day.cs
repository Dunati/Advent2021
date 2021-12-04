namespace Day05;


class Day : BaseDay
{
    public override string Run(int part, string rawData)
    {
        var lines = rawData.Lines().Select(x => x.Split(", ->".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToInts().ToList()).ToArray();

        DefaultDictionary<(int, int), int> map = new();

        int maxX = 9;
        int maxY = 9;
        foreach (var line in lines)
        {
            var (x1, y1, x2, y2, _) = line;
            if (maxX < x1)
                maxX = x1;
            if (maxY < y1)
                maxY = y1;
            if (maxX < x2)
                maxX = x2;
            if (maxY < y2)
                maxY = y2;

            if (part == 1 && ((x1 != x2) == (y1 != y2)))
                continue;

            var step = (Sign(x2 - x1), Sign(y2 - y1));
            var steps = Max(Abs(x2 - x1), Abs(y2 - y1));

            var position = (x1, y1);
            for (int i = 0; i <= steps; i++)
            {
                map[position]++;
                position = (position.x1 + step.Item1, position.y1 + step.Item2);
            }

        }

        // 19174 too high
        return map.Values.Where(x => x > 1).Count().ToString();

    }

    private static void DrawMap(DefaultDictionary<(int, int), int> map, int maxX, int maxY)
    {
        Trace.WriteLine("");
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                int value = map[(x, y)];
                if (value == 0)
                {
                    Trace.Write('.');
                }
                else
                {
                    Trace.Write(value);
                }
            }
            Trace.WriteLine("");
        }
    }
}
