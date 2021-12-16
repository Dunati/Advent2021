using System.Diagnostics.CodeAnalysis;

namespace Day15;


class Day : BaseDay
{

    struct Point
    {
        public int X { get; init; }
        public int Y { get; init; }
        public int Cost { get; init; }

        public override string ToString()
        {
            return $"{X},{Y}={Cost}";
        }


        public static Point operator +(Point a, Point p)
        {
            return new Point { X = a.X + p.X, Y = a.Y + p.Y, Cost = a.Cost + p.Cost };
        }

        public override int GetHashCode()
        {
            return (X << 16) | Y;
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            if (obj is Point)
            {
                Point p = (Point)obj;
                return p.X == this.X && p.Y == this.Y;
            }
            return false;
        }
    }

    readonly static Point Up = new Point { X = 0, Y = -1 };
    readonly static Point Down = new Point { X = 0, Y = 1 };
    readonly static Point Left = new Point { X = -1, Y = 0 };
    readonly static Point Right = new Point { X = 1, Y = 0 };

    void Print(int[][] grid, HashSet<Point> visited)
    {
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (visited.TryGetValue(new Point() { X = x, Y = y }, out Point p))
                {
                    Trace.Write(p.Cost.ToString().PadLeft(4));
                }
                else
                {
                    // Trace.Write((char)(grid[y][x] + '0'));
                    Trace.Write("    ");
                }
            }
            Trace.WriteLine("");
        }
    }

    int[][] growGrid(int[][] grid, int mult)
    {
        int W = grid[0].Length;
        int width = W * mult;
        int H = grid.Length;
        int height = H * mult;
        int[][] n = new int[height][];

        for (int y = 0; y < n.Length; y++)
        {
            int[] row = new int[width];
            n[y] = row;

            for (int x = 0; x < width; x++)
            {
                int value = grid[y % H][x % H] + y / H + x / W;
                row[x] = 1 + ((value - 1) % 9);

            }
        }

        return n;
    }


    public override string Run(int part, string rawData) 
    {
        int[][] grid = rawData.Lines().Select(x => x.Select(x => x - '0').ToArray()).ToArray();

        if (part == 2)
        {
            grid = growGrid(grid, 5);
        }

        Point[,] previous = new Point[grid[0].Length, grid.Length];

        int[][] distance = Enumerable.Range(0, grid.Length).Select(_ => Enumerable.Repeat(int.MaxValue, grid[0].Length).ToArray()).ToArray();

        PriorityQueue<Point, float> priorityQueue = new();


        var Queue = (Point p, Point prev) =>
        {
            if (p.X < 0 || p.Y < 0 || p.X >= grid[0].Length || p.Y >= grid.Length) return;
            int cost = p.Cost + grid[p.Y][p.X];
            if (distance[p.Y][p.X] <= cost)
            {
                return;
            }

            previous[p.X, p.Y] = prev;
            distance[p.Y][p.X] = cost;
            priorityQueue.Enqueue(p, cost);
        };

        var Visit = (Point p) =>
        {
            Point leaving = new Point { X = p.X, Y = p.Y, Cost = distance[p.Y][p.X] };

            Queue(leaving + Up, p);
            Queue(leaving + Down, p);
            Queue(leaving + Left, p);
            Queue(leaving + Right, p);
        };

        Queue(new Point(), new Point());

        Point position = default;

        while (priorityQueue.Count > 0)
        {
            position = priorityQueue.Dequeue();
            Visit(position);
        }


        var pp = () =>
        {

            int[,] path = new int[grid[0].Length, grid.Length];

            path[0, 0] = 1;
            int x = grid[0].Length - 1;
            int y = grid.Length - 1;

            while (x > 0 || y > 0)
            {
                path[x, y] = grid[y][x];
                var p = previous[x, y];
                x = p.X;
                y = p.Y;
            }

            Trace.WriteLine("");
            for (y = 0; y < grid.Length; y++)
            {
                for (x = 0; x < grid.Length; x++)
                {
                    if (path[x, y] != 0)
                    {
                        Trace.Write(path[x, y]);
                    }
                    else
                    {
                        Trace.Write(' ');
                    }
                }
                Trace.WriteLine("");
            }
            Trace.WriteLine("");
        };

        // p2 2856 too low

        return (distance.Last().Last() -grid[0][0]).ToString();
    }
}
