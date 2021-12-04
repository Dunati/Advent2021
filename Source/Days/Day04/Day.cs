namespace Day04;


class Day : BaseDay
{

    DefaultDictionary<int, int> position = new();

    IEnumerable<int[]> ReadBoard(IEnumerable<string> str)
    {
        for (IEnumerator<string> s = str.GetEnumerator(); ;)
        {
            int[] board = new int[26];
            int i = 0;
            while (i < 25)
            {
                if (!s.MoveNext()) { yield break; }
                foreach (var number in s.Current.ToInts(10, " \r\n"))
                {
                    board[25] += number;
                    board[i++] = position[number];
                }
            }
            yield return board;

        }
    }

    int PV(int value)
    {
        return position.Keys.Skip(value - 1).First();
    }

    int ScoreBoard(int[] board, int last)
    {
        int lastValue = PV(last);
        int sum = 0;

        for (int i = 0; i < 25; i++)
        {
            int marked = PV(board[i]);
            if (board[i] > last)
            {
                sum += marked;
            }
            else
            {
                sum += 0;
            }
        }

        return sum * lastValue;
    }

    public override string Run(int part, string rawData)
    {
        position = new();
        var lines = rawData.Lines();

        int[] input = lines.First().ToInts(10, ",\r\n").ToArray();

        for (int i = 0; i < input.Length; i++)
        {
            position[input[i]] = i + 1;
        }

        int worstBoardTime = 0;
        int worstBoardScore = int.MaxValue;
        int bestBoardTime = int.MaxValue;
        int bestBoardScore = 0;
        int boardNum = 0;
        int worstBoard;
        foreach (int[] board in ReadBoard(lines.Skip(1)))
        {
            boardNum++;
            int winTurn = int.MaxValue;

            int[] rowMax = new int[5];
            for (int x = 0; x < 5; x++)
            {
                int columnMax = 0;
                for (int y = 0; y < 5; y++)
                {
                    int item = board[x + 5 * y];

                    if (item > columnMax)
                    {
                        columnMax = item;
                    }
                    if (item > rowMax[y])
                    {
                        rowMax[y] = item;
                    }
                }
                if (columnMax < winTurn)
                {
                    winTurn = columnMax;
                }
            }
            for (int y = 0; y < 5; y++)
            {
                if (rowMax[y] < winTurn)
                {
                    winTurn = rowMax[y];
                }
            }

            int score = ScoreBoard(board, winTurn);


            if (winTurn < bestBoardTime)
            {
                bestBoardScore = score;
                bestBoardTime = winTurn;
            }
            if (winTurn >= worstBoardTime)
            {
                if (score < worstBoardScore || winTurn > worstBoardTime)
                {
                    worstBoardScore = score;
                }
                worstBoardTime = winTurn;
                worstBoard = boardNum;
            }
        }
        //23883 too low

        if (part == 1)
            return bestBoardScore.ToString();

        // too high 14937
        // 6527 too high
        // 1352 too low
        return worstBoardScore.ToString();

    }
}
