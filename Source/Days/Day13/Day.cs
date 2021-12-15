namespace Day13;


class Day : BaseDay
{

    delegate Point Transform(ref Point p);
    public override string Run(int part, string rawData)
    {
        var lines = rawData.Split("\n").ToArray();
        int count = 0;
        for (count = 0; count < lines.Length; count++)
        {
            if (lines[count] == "")
            {
                break;
            }
        }

        var instructions = lines[(count + 1)..];

        if (instructions.Last() == "")
        {
            instructions = instructions[0..^1];
        }
        var dots = lines[0..count];

        Transform transforms = null;


        foreach (var instruction in instructions)
        {
            int value = int.Parse(instruction[13..]);
            switch (instruction[11])
            {
                case 'y':
                    transforms += (ref Point p) =>
                    {
                        if (p.Y > value)
                        {
                            p = new System.Drawing.Point(p.X, 2 * value - p.Y);
                        }
                        return p;
                    };
                    break;

                case 'x':
                    transforms += (ref Point p) =>
                    {
                        if (p.X > value)
                        {
                            p = new System.Drawing.Point(2 * value - p.X, p.Y);
                        }
                        return p;
                    };
                    break;
                default:
                    throw new Exception("wtf");

            }

            if (part == 1)
                break;
        }

        var points = new HashSet<Point>();


        foreach (var line in dots)
        {
            var dot = line.Split(',').Select(x => int.Parse(x));
            Point p = new System.Drawing.Point(dot.First(), dot.Skip(1).First());
            transforms(ref p);

            if (!points.Contains(p))
            {
                points.Add(p);
            }
        }

        if(part == 2)
        Print(points);

        //96 wrong
        //97 wrong
        //95 wrong
        //94 wrong
        return points.Count.ToString();
    }


    void Print(HashSet<Point> points)
    {

        int maxx = 0;
        int maxy = 6;

        foreach (var point in points)
        {
            if (point.X >= maxx)
            {
                maxx = point.X + 1;
            }
            if (point.Y >= maxy)
            {
                maxy = point.Y + 1;
            }
        }

        char[,] map = new char[maxx, maxy];
        foreach (var point in points)
        {
            map[point.X, point.Y] = '█';
        }

        for (int y = 0; y < maxy; y++)
        {
            Trace.WriteLine("");
            for (int x = 0; x < maxx; x++)
            {
                if (map[x, y] != 0)
                {
                    Trace.Write(map[x, y]);
                }
                else
                {
                    Trace.Write(' ');
                }
            }
        }

        Trace.WriteLine("");
    }


}
