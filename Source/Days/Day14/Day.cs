namespace Day14;


class Day : BaseDay
{
    public override string Run(int part, string rawData)
    {
        var lines = rawData.Lines();

        string template = rawData.Lines().First();

        decimal[,] pairs = new decimal[26, 26];

        for(int i = 0; i < template.Length-1; i++)
        {
            pairs[template[i] - 'A', template[i + 1] - 'A']++;
        }
        
        var Score = () =>
        {
            decimal common = 0;
            decimal rare = decimal.MaxValue;

            char commonLetter = 'A';
            char rareLetter = 'A';

            for (int i = 0; i < 26; i++)
            {
                decimal total = 0;
                for (int j = 0; j < 26; j++)
                {
                    total += pairs[i, j];
                }
                if ((i+'A') == template[^1])
                {
                    total++;
                }

                if (total > common)
                {
                    common = total;
                    commonLetter = (char) (i + 'A');
                }
                if (total != 0 && total < rare)
                {
                    rare = total;
                    rareLetter = (char)(i + 'A');
                }
            }

            Trace.WriteLine($"\n{commonLetter}:{common}  {rareLetter}:{rare}");
            return (common, rare);
        };
        Score();

        int limit = 10;
        if(part == 2)
        {
            limit = 40;
        }
        for (int i = 0; i < limit; i++)
        {
            decimal[,] result = new decimal[26, 26];

            foreach (var line in lines.Skip(1))
            {
                int a = line[0] - 'A';
                int b = line[1] - 'A';
                int x = line[6] - 'A';
                result[a, x] += pairs[a, b];
                result[x, b] += pairs[a, b];
            }

            pairs = result;

            Score();
        }

        var (common, rare) = Score();
        return (common - rare).ToString();
    }
}
