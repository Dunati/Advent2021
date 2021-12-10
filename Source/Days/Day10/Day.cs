namespace Day10;


class Day : BaseDay
{
    public override string Run(int part, string rawData)
    {
        List<char> stack = new();

        var ScoreError = (char c) =>
        {
            if (stack.Count != 0)
            {
                return c switch
                {
                    ')' => stack.Last() == '(' ? 0 : 3,
                    ']' => stack.Last() == '[' ? 0 : 57,
                    '}' => stack.Last() == '{' ? 0 : 1197,
                    '>' => stack.Last() == '<' ? 0 : 25137,
                    _ => throw new Exception("unexpected: " + c)
                };
            }
            return 0;
        };

        List<decimal> autocomplete = new();
        int total = 0;
        foreach (var line in rawData.Lines())
        {
            foreach (var c in line)
            {
                switch (c)
                {
                    case '(': stack.Push(c); break;
                    case '<': stack.Push(c); break;
                    case '[': stack.Push(c); break;
                    case '{': stack.Push(c); break;
                    default:
                        {
                            int error = ScoreError(c);
                            if (error == 0)
                            {
                                stack.Pop();
                                break;
                            }
                            total += error;
                            goto done;
                        }
                }
            }

            decimal completionScore = 0;
            while(stack.Count > 0)
            {
                completionScore *= 5;
                completionScore += stack.Pop() switch
                {
                    '(' => 1,
                    '[' => 2,
                    '{' => 3,
                    '<' => 4,
                    var n => throw new Exception($"Unexpected {n}")
                };
            }

            autocomplete.Add(completionScore);

        done:
            stack.Clear();
        }
        // 589338 too high
        // 464991

        if (part == 1)
            return total.ToString();

        // 35579505 too low
        // 3662008566 with decimal
        return autocomplete.OrderBy(x=>x).Skip(autocomplete.Count/2).First().ToString();
    }
}
