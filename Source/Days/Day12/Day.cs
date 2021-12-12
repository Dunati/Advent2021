namespace Day12;


class Day : BaseDay
{
    class Node
    {
        public string Name { get; init; }

        public List<string> Paths { get; init; }
    }
    public override string Run(int part, string rawData)
    {
        Dictionary<string, Node> nodes = new();

        var lines = rawData.Lines().Select(x => x.Split('-', StringSplitOptions.RemoveEmptyEntries));

        foreach (var line in lines)
        {
            Node node = null;
            if (nodes.TryGetValue(line[0], out node))
            {
                node.Paths.Add(line[1]);
            }
            else
            {
                nodes[line[0]] = new Node() { Name = line[0], Paths = new() { line[1] } };
            }

            if (nodes.TryGetValue(line[1], out node))
            {
                node.Paths.Add(line[0]);
            }
            else
            {
                nodes[line[1]] = new Node() { Name = line[1], Paths = new() { line[0] } };
            }
        }

        var bob = new DefaultDictionary<string, int>();

        bob["start"]++;

        return CountPaths("start", "end", nodes, bob, part).ToString();
    }

    private int CountPaths(string current, string destination, Dictionary<string, Node> nodes, DefaultDictionary<string, int> visited, int maxVisitCount)
    {
        int count = 0;

        if (current == destination)
        {
            return 1;
        }
        if (current[0] >= 'a')
        {
            visited[current]++;
        }

        foreach (string path in nodes[current].Paths)
        {
            int visitCount = visited[path];
            if (visitCount < maxVisitCount)
            {
                count += CountPaths(path, destination, nodes, visited.Clone(), visitCount > 0 ? 1 : maxVisitCount);
            }
        }

        return count;
    }
}
