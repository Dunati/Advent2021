namespace Day11;



class Day : BaseDay
{
    public override string Run(int part, string rawData)
    {
        int[][] grid = rawData.Lines().Select(x => x.ToCharArray().Select(x => x - '0').ToArray()).ToArray();


        int maxY = grid.Length;
        int maxX = grid[0].Length;

        HashSet<Point> boomed = new HashSet<Point>();
        HashSet<Point> boom = new();
        var Increment = (Point p) =>
        {
            if (p.X >= 0 && p.Y >= 0 && p.X < maxX && p.Y < maxY)
            {
                if (++grid[p.Y][p.X] > 9)
                {
                    if (!boomed.Contains(p) && !boom.Contains(p))
                    {
                        boom.Add(p);
                    }
                }
            }
        };

        var Print = () =>
        {
            Trace.WriteLine("");
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    Trace.Write((char)(grid[y][x] + '0'));
                }
                Trace.WriteLine("");
            }
        };

        int steps = 100;

        if(part == 2)
        {
            steps = int.MaxValue - 1;
        }

        int flashes = 0;
        for (int i = 1; i <= steps; i++)
        {
         //   Print();
            boomed.Clear();
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    Increment(new System.Drawing.Point(x, y));
                }
            }

            while (boom.Count > 0)
            {
                Point p = boom.First();

                Increment(new Point(p.X - 1, p.Y - 1));
                Increment(new Point(p.X - 1, p.Y - 0));
                Increment(new Point(p.X - 1, p.Y + 1));
                Increment(new Point(p.X - 0, p.Y - 1));
                Increment(new Point(p.X - 0, p.Y + 1));
                Increment(new Point(p.X + 1, p.Y - 1));
                Increment(new Point(p.X + 1, p.Y - 0));
                Increment(new Point(p.X + 1, p.Y + 1));

                boomed.Add(p);
                boom.Remove(p);
            }

            foreach (var p in boomed)
            {
                grid[p.Y][p.X] = 0;
                flashes++;
            }

            if(part == 2 && boomed.Count == maxX* maxY)
            {
                Print();
                //421 wrong
                //420 wrong
                return i.ToString();
            }

        }
        //1964 wrong
        return flashes.ToString();
    }
}
