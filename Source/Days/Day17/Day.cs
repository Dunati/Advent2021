namespace Day17;


class Day : BaseDay
{
    enum State
    {
        Moving,
        Hit,
        Under,
        Over,
        Miss
    }

    public override string Run(int part, string rawData)
    {
        var matches = Regex.Matches(rawData, "-?[0-9]+");

        int x1 = int.Parse(matches[0].Value);
        int x2 = int.Parse(matches[1].Value);
        int y1 = int.Parse(matches[2].Value);
        int y2 = int.Parse(matches[3].Value);
        if (y2 > y1)
        {
            y2 = y1;
            y1 = int.Parse(matches[3].Value);
        }

        var Step = (ref Point position, ref Point velocity, ref int maxHeight) =>
        {

            position.X += velocity.X;
            position.Y += velocity.Y;

            if (position.Y > maxHeight)
            {
                maxHeight = position.Y;
            }
            velocity.Y -= 1;

            if (velocity.X > 0) velocity.X -= 1;
            else if (velocity.X < 0) velocity.X += 1;

            if (position.Y < y2 && velocity.Y < 0)
                return State.Miss;
            if (position.X > x2 && velocity.X >= 0)
                return State.Over;
            if (position.X < x1 && velocity.X <= 0)
                return State.Under;

            if (position.X >= x1 && position.X <= x2 && position.Y <= y1 && position.Y >= y2)
            {
                return State.Hit;
            }

            return State.Moving;
        };


        var TestFire = (Point velocity, ref int MaxHeight) =>
        {
            Point v0 = velocity;
            Point position = new();
            int maxHeight = 0;

            State state;
            do
            {
                state = Step(ref position, ref velocity, ref maxHeight);

            }
            while (state == State.Moving);
            //   Trace.WriteLine($"{v0.X},{v0.Y} = {state} {position.X},{position.Y},{maxHeight}");
            if (state == State.Hit && maxHeight > MaxHeight)
            {
                MaxHeight = maxHeight;
            }
            return state;
        };

        int MaxHeight = 0;
        Point velocity = new(0, 0);

        int maxX = (int)Sqrt(2 * x2);
        int minX = (int)Sqrt(2 * x1) - 1;

        if(part == 2)
        {
            maxX = x2;
        }
        int hits = 0;

        for (int x = minX; x <= maxX; x++)
        {
            velocity.X = x;

            for (int y = y2; y < 1000; y++)
            {
                velocity.Y = y;
                State s = TestFire(velocity, ref MaxHeight);
                if (s == State.Hit && part == 2)
                {
                    if(!InTest)
                    Trace.WriteLine($"{velocity.X},{velocity.Y}");
                    hits++;
                }
            }
        }

        if (part == 1)
        {
            // 3160 too low
            // 13401 too high
            return MaxHeight.ToString();
        }

        return hits.ToString();
    }
}
