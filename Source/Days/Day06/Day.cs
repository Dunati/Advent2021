

namespace Day06;



class Day : BaseDay
{


    public override string Run(int part, string rawData)
    {

        decimal[] fishCount = new decimal[7];
        decimal[] fryCount = new decimal[9];
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

            decimal fry = fryCount[spawnIndex];
            fryCount[spawnIndex] = fishCount[index] + fry;
            fishCount[index] += fry;

        }
        //692265565 too low
        decimal total = CountFish(fishCount, fryCount);
        return total.ToString();
    }

    private static decimal CountFish(decimal[] fishCount, decimal[] fryCount)
    {
        decimal total = 0;
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
