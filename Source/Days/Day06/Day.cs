

namespace Day06;



class Day : BaseDay
{


    public override string Run(int part, string rawData)
    {

        Int64[] fishCount = new Int64[7];
        Int64[] fryCount = new Int64[9];
        foreach (var f in rawData.ToInts(10, ",\n\r"))
        {
            fishCount[f]++;
        }

        int totalDays = 80;

        if (part == 2)
        {
            totalDays = 256;
        }
        int day = 0;
        for (; day < totalDays; day++)
        {
            int index = day % fishCount.Length;
            int spawnIndex = (day + 8) % fryCount.Length;

            Int64 fry = fryCount[spawnIndex];
            fryCount[spawnIndex] = fishCount[index] + fry;
            fishCount[index] += fry;

        }
        //692265565 too low
        Int64 total = CountFish(fishCount, fryCount);
        return total.ToString();
    }

    private static Int64 CountFish(Int64[] fishCount, Int64[] fryCount)
    {
        Int64 total = 0;
        for (int i = 0; i < fishCount.Length; i++)
        {
            total += fishCount[i];
        }
        for (int i = 0; i < fryCount.Length; i++)
        {
            total += fryCount[i];
        }
        return total;
    }
}
