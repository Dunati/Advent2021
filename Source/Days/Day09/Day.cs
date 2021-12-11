namespace Day09;

using System;

class Day : BaseDay
{
    public override string Run(int part, string rawData)
    {
        int[][] floor = rawData.Lines().Select(line => line.ToCharArray().Select(x => x - '0').ToArray()).ToArray();

        if (part == 2)
        {
            return Part2(floor);
        }
        int lowest = 0;

        foreach (var lowpoint in LowPoints(floor))
        {
            lowest += floor[lowpoint.Y][lowpoint.X] + 1;
        }

        return lowest.ToString();
    }

    public IEnumerable<Point> LowPoints(int[][] floor)
    {
        for (int y = 0; y < floor.Length; y++)
        {
            for (int x = 0; x < floor[y].Length; x++)
            {

                int left = y > 0 ? floor[y - 1][x] : int.MaxValue;
                int right = y < floor.Length - 1 ? floor[y + 1][x] : int.MaxValue;
                int up = x > 0 ? floor[y][x - 1] : int.MaxValue;
                int down = x < floor[y].Length - 1 ? floor[y][x + 1] : int.MaxValue;

                int value = floor[y][x];

                if (value < left && value < right && value < up && value < down)
                {
                    yield return new Point(x, y);
                }

            }
        }
    }

    int BasinSize(int[][] floor, Point lowpoint)
    {
        List<Point> remaining = new() { lowpoint };
        HashSet<Point> visited = new();
        while (remaining.Count > 0)
        {
            lowpoint = remaining.Pop();
            visited.Add(lowpoint);

            if (lowpoint.Y > 0 && floor[lowpoint.Y - 1][lowpoint.X] != 9)
            {
                var newPoint = new Point(lowpoint.X, lowpoint.Y - 1);
                if (!visited.Contains(newPoint))
                {
                    remaining.Push(newPoint);
                }
            }
            if (lowpoint.Y < floor.Length - 1 && floor[lowpoint.Y + 1][lowpoint.X] != 9)
            {
                var newPoint = new Point(lowpoint.X, lowpoint.Y + 1);
                if (!visited.Contains(newPoint))
                {
                    remaining.Push(newPoint);
                }
            }

            if (lowpoint.X > 0 && floor[lowpoint.Y][lowpoint.X - 1] != 9)
            {
                var newPoint = new Point(lowpoint.X - 1, lowpoint.Y);
                if (!visited.Contains(newPoint))
                {
                    remaining.Push(newPoint);
                }
            }
            if (lowpoint.X < floor[0].Length - 1 && floor[lowpoint.Y][lowpoint.X + 1] != 9)
            {
                var newPoint = new Point(lowpoint.X+1, lowpoint.Y);
                if (!visited.Contains(newPoint))
                {
                    remaining.Push(newPoint);
                }
            }
        }
        return visited.Count;
    }

    private string Part2(int[][] floor)
    {
        List<int> basinSizes = new();
        foreach (var lowpoint in LowPoints(floor))
        {
            basinSizes.Add(BasinSize(floor, lowpoint));
        }

        return basinSizes.OrderByDescending(x => x).Take(3).Aggregate((x, y) => x * y).ToString();
    }
}
