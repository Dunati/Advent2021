using System.Reflection;

public static class Runner
{

    private static void RunAllDays()
    {
        foreach (Type t in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Name.StartsWith("Day")).OrderBy(x => x.Name))
        {
            Run(t, 0);
        }
        foreach (Type t in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Name.StartsWith("_Day")).OrderBy(x => x.Name))
        {
            Run(t, 0);
        }
    }

    public static void Main(string[] args)
    {
        if (!Debugger.IsAttached)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }
        Type type;
        int day = 0;
        int part = 0;
        bool newDay = false;
        if (args.Length > 0)
        {
            if (args[0] == "all")
            {
                RunAllDays();
                return;
            }
            else if (args[0] == "NewDay")
            {
                newDay = true;
            }
            else
            {
                if (!int.TryParse(args[0], out day))
                {
                    Trace.WriteLine($"Cannot parse {args[0]} as a day number");
                    return;
                }
            }
            if (args.Length > 1 && !int.TryParse(args[1], out part))
            {
                Trace.WriteLine($"Cannot parse {args[1]} as a part number");
                return;
            }

            if (part < 0 || part > 2)
            {
                Trace.WriteLine($"part {part} is not a valid part number");
                return;
            }
        }
        if (day == 0)
        {
            type = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Name.StartsWith("Day")).OrderBy(x => x.Namespace).Last();
        }
        else
        {
            type = Assembly.GetExecutingAssembly().GetType($"Day{day:00}");
            if (type == null)
            {
                Trace.WriteLine($"Cannot find class Day{day:00} specified on the command line");
                return;
            }
        }

        if (newDay)
        {
            if (type.Namespace == null)
            {
                day = 1;
            }
            else
            {
                day = int.Parse(type.Namespace[^2..]) + 1;
            }
            Directory.CreateDirectory($"Source/Days/Day{day:00}");
            string template = File.ReadAllText("Source/Days/Day00/Day.cs");
            using var stream = new StreamWriter($"Source/Days/Day{day:00}/Day.cs");
            stream.WriteLine($"namespace Day{day:00};");
            stream.WriteLine();
            stream.WriteLine(template);

            File.Create($"Source/Days/Day{day:00}/input1.txt").Dispose();
            File.Create($"Source/Days/Day{day:00}/result1-1.txt").Dispose();
            File.Create($"Source/Days/Day{day:00}/result2-1.txt").Dispose();
            File.Create($"Source/Days/Day{day:00}/test1-1.txt").Dispose();
        }
        else
        {
            Run(type, part);
        }
    }

    static void Run(Type type, int part)
    {
        BaseDay day = (BaseDay)Activator.CreateInstance(type);

        if (part != 0)
        {
            RunPart(day, part);
        }
        else
        {
            if (day.Part == 0)
            {
                RunPart(day, 1);
                RunPart(day, 2);
            }
            else
            {
                RunPart(day, day.Part);
            }
        }
    }


    static void RunPart(BaseDay day, int part)
    {
        Trace.Write($"Running {day.Name}-{part} ");
        if (day.RunTests(part))
        {
            Trace.WriteLine($"  result: {day.Run(part)}\n");
        }
    }


}