using System.IO;

public class BaseDay
{
    public virtual string Run(int part, string rawData) => "Not Implemented";

    protected Stopwatch Stopwatch;
    public string Run(int part)
    {
        Stopwatch = Stopwatch.StartNew();
        string partName = $"Source/Days/{Name}/input{part}.txt";
        if (!File.Exists(partName))
        {
            partName = $"Source/Days/{Name}/input1.txt";

            if (!File.Exists(partName))
            {
                return $"input {partName} not found";
            }
        }

        string result = "";
        result = Run(part, File.ReadAllText(partName), false);
        if (Stopwatch != null)
        {
            Trace.WriteLine($"\n  Solution: {Stopwatch.Elapsed.TotalMilliseconds}ms");
            Stopwatch = null;
        }
        return result;
    }

    protected void DoneParsing()
    {
        if (Stopwatch != null)
        {
            Trace.Write($"\n  Parsing:  {Stopwatch.Elapsed.TotalMilliseconds}ms");
            Stopwatch.Restart();
        }
    }

    public string TestInput(int part, int testNum)
    {
        string testName = $"Source/Days/{Name}/test{part}-{testNum}.txt";
        if (!File.Exists(testName))
        {
            if (part != 1)
            {
                return TestInput(1, testNum);
            }
            if (testNum != 1)
            {
                return TestInput(part, 1);
            }
            return null;
        }

        return File.ReadAllText(testName);
    }

    public string TestResult(int part, int testNum)
    {
        string testName = $"Source/Days/{Name}/result{part}-{testNum}.txt";
        if (!File.Exists(testName))
        {
            return null;
        }

        return File.ReadAllText(testName);
    }

    public virtual string Run(int part, string rawData, bool isTest) {InTest=isTest; return Run(part, rawData); }

    public bool RunTests(int part)
    {
        int testNum = Test == 0 ? 1 : Test;
        bool passed = true;
        while (true)
        {
            string expected = TestResult(part, testNum);
            if (expected == null)
            {
                return passed;
            }

            try
            {
                string input = TestInput(part, testNum);
                string result = Run(part, input, true);
                if (result != expected)
                {
                    throw new Exception($"Test Failed: expected '{expected}', received '{result}'");
                }
                Trace.Write($" ");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"\nTest {part}-{testNum} failed: {e.Message}\n  ");
                passed = false;
            }
            if (Test != 0)
            {
                return passed;
            }
            testNum++;
        }
    }

    public string Name => this.GetType().Namespace.TrimStart('_');

    public int Number => int.Parse(Name[3..]);

    public int Part { get => 0; }

    public int Test { get => 0; }

    public bool InTest { get;set; }
}