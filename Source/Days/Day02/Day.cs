namespace Day02;


class Day : BaseDay
{
    public override string Run(int part, string rawData)
    {
        if (part == 1)
            return Part1(rawData);

        int aim = 0;
        int depth = 0;
        int distance = 0;
        foreach (var instruction in rawData.Lines())
        {
            var decomposed =
            instruction.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int value = int.Parse(decomposed[1]); 
            switch (decomposed[0])
            {
                case "forward":
                    distance += value;
                    depth += aim* value;
                    break;
                case "up":
                    aim -= value;
                    break;
                case "down":
                    aim += value;
                    break;
            }
        }
        return (depth * distance).ToString();
    }

    private static string Part1(string rawData)
    {
        int depth = 0;
        int distance = 0;
        foreach (var instruction in rawData.Lines())
        {
            var decomposed =
            instruction.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            switch (decomposed[0])
            {
                case "forward":
                    distance += int.Parse(decomposed[1]);
                    break;
                case "up":
                    depth -= int.Parse(decomposed[1]);
                    break;
                case "down":
                    depth += int.Parse(decomposed[1]);
                    break;
            }
        }

        return (depth * distance).ToString();
    }
}
