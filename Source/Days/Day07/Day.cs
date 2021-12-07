namespace Day07;


class Day : BaseDay
{

    string Part1(string rawData)
    {

        int[] crabs = rawData.ToInts(10, ",\r\n").OrderBy(x => x).ToArray();

        int median = crabs[crabs.Length / 2];
        int median2 = crabs[crabs.Length / 2 + 1];
        int m = 0;
        int m2 = 0;


        for (int i = 0; i < crabs.Length; i++)
        {
            m += Abs(crabs[i] - median);
            m2 += Abs(crabs[i] - median2);
        }

        return Min(m, m2).ToString();
    }
    public override string Run(int part, string rawData)
    {
        if (part == 1) return Part1(rawData);

        int[] crabs = rawData.ToInts(10, ",\r\n").ToArray();

        int total = crabs.Aggregate((x, y) => x + y);

        int average = total/crabs.Length;

        int target = average;
        int target1 = average + 1;

        int fuel1 = 0;
        int fuel2 = 0;

        for (int i =0; i < crabs.Length; i++)
        {
            fuel1 += Fuel(Abs(crabs[i] - target));
            fuel2 += Fuel(Abs(crabs[i] - target1));
        }

        return Min(fuel1,fuel2).ToString();

    }

    private static int Fuel(int distance)
    {
        int fuel = 0;
        if (distance < 2)
        {
            fuel += distance;
        }
        else
        {
            fuel += (distance) * (distance + 1) / 2;
        }

        return fuel;
    }
}
