namespace Day18;


class Day : BaseDay
{

    class SnailNum
    {
        public SnailNum Left { get; set; }
        public SnailNum Right { get; set; }

        public SnailNum Parent { get; set; }

        int _value = 0;
        public int Value
        {
            get => _value;
            set { _value = value; Left = null; Right = null; }
        }

        public static SnailNum Parse(string s)
        {
            using var reader = new StringReader(s);
            return Parse(reader);
        }


        bool IsRegular => Left == null && Right == null;


        public static SnailNum operator +(SnailNum a, SnailNum b)
        {
            var result = new SnailNum() { Left = a, Right = b };
            a.Parent = result;
            b.Parent = result;

            result.Reduce();
            return result;
        }

        public static SnailNum Parse(StringReader s)
        {
            var snail = new SnailNum();
            Debug.Assert(s.Peek() == '['); s.Read();


            if (s.Peek() == '[')
            {
                snail.Left = Parse(s);
                snail.Left.Parent = snail;
            }
            else
            {
                snail.Left = new SnailNum() { Value = s.Read() - '0', Parent = snail };
                Debug.Assert(snail.Left.Value >= 0 && snail.Left.Value <= 9);
            }

            Debug.Assert(s.Peek() == ','); s.Read();

            if (s.Peek() == '[')
            {
                snail.Right = Parse(s);
                snail.Right.Parent = snail;
            }
            else
            {
                snail.Right = new SnailNum() { Value = s.Read() - '0', Parent = snail };
                Debug.Assert(snail.Right.Value >= 0 && snail.Right.Value <= 9);
            }
            Debug.Assert(s.Peek() == ']'); s.Read();

            return snail;
        }


        public override string ToString()
        {
            if (Left == null && Right == null)
            {
                return Value.ToString();
            }

            return $"[{Left},{Right}]";
        }

        public bool Explode(int depth = 0)
        {
            if (IsRegular)
            {
                return false;
            }

            if (depth == 4)
            {
                var p = Parent;
                var c = this;
                while (p.Parent != null && p.Right == c)
                {
                    c = p;
                    p = p.Parent;
                }

                if (p.Right != c)
                {
                    c = p.Right;
                    while (!c.IsRegular)
                    {
                        c = c.Left;
                    }
                    c.Value += Right.Value;
                }

                p = Parent;
                c = this;

                while (p.Parent != null && p.Left == c)
                {
                    c = p;
                    p = p.Parent;
                }

                if (p.Left != c)
                {
                    c = p.Left;
                    while (!c.IsRegular)
                    {
                        c = c.Right;
                    }
                    c.Value += Left.Value;
                }

                Value = 0;
                return true;
            }
            else
            {
                return Left.Explode(depth + 1) || Right.Explode(depth + 1);
            }
        }

        public void Reduce()
        {
            while (Explode() || Split()) { }
        }


        bool Split()
        {
            if (IsRegular)
            {
                if (Value > 9)
                {
                    int value = Value / 2;
                    Left = new SnailNum() { Value = value, Parent = this };
                    Right = new SnailNum() { Value = Value - value, Parent = this };
                    return true;
                }
                return false;
            }
            return Left.Split() || Right.Split();
        }

        public int Magnitude()
        {
            if (IsRegular)
            {
                return Value;
            }

            return 3 * Left.Magnitude() + 2 * Right.Magnitude();

        }

    }




    void CheckExplode(string s, string r)
    {
        var result = SnailNum.Parse(s);
        result.Explode();
        Debug.Assert(result.ToString() == r, $"{s}\n{r}\n{result.ToString()}");
    }


    public override string Run(int part, string rawData)
    {
        var lines = rawData.Lines().ToArray();
        if (part == 1)
        {

            var result = lines.Select(x => SnailNum.Parse(x)).Aggregate((x, y) => x + y);
            return result.Magnitude().ToString();
        }

        int largest = 0;

        for (int i = 0; i < lines.Length - 1; i++)
        {
            for (int j = i + 1; j < lines.Length; j++)
            {
                int m = (SnailNum.Parse(lines[i]) + SnailNum.Parse(lines[j])).Magnitude();
                if (m > largest)
                {
                    largest = m;
                }
                m = (SnailNum.Parse(lines[j]) + SnailNum.Parse(lines[i])).Magnitude();
                if (m > largest)
                {
                    largest = m;
                }
            }
        }

        return largest.ToString();
    }


    public string RunT(int part, string rawData)
    {
        var lines = rawData.Lines().Select(x => SnailNum.Parse(x)).ToArray();


        CheckExplode("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]");
        CheckExplode("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]");
        CheckExplode("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]");
        CheckExplode("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]");
        CheckExplode("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]");

        var result = SnailNum.Parse("[[[[4,3],4],4],[7,[[8,4],9]]]") + SnailNum.Parse("[1,1]");

        Debug.Assert(result.ToString() == "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]");

        Debug.Assert(SnailNum.Parse("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]").Magnitude() == 3488);
#if XXX
        var result = SnailNum.Parse("[1,1]") +
        SnailNum.Parse("[2,2]") +
        SnailNum.Parse("[3,3]") +
        SnailNum.Parse("[4,4]") +
        SnailNum.Parse("[5,5]");

        Trace.WriteLine(result.ToString());


        result = SnailNum.Parse("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]");

        result += SnailNum.Parse("[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]");
        Debug.Assert(result.ToString() == "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]");

        result += SnailNum.Parse("[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]");
        Debug.Assert(result.ToString() == "[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]");

        result += SnailNum.Parse("[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]");
        Debug.Assert(result.ToString() == "[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]");

        result += SnailNum.Parse("[7,[5,[[3,8],[1,4]]]]");
        Debug.Assert(result.ToString() == "[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]");

        result += SnailNum.Parse("[[2,[2,2]],[8,[8,1]]]");
        Debug.Assert(result.ToString() == "[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]");

        result += SnailNum.Parse("[2,9]");
        Debug.Assert(result.ToString() == "[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]");

        result += SnailNum.Parse("[1,[[[9,3],9],[[9,0],[0,7]]]]");
        Debug.Assert(result.ToString() == "[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]");

        result += SnailNum.Parse("[[[5,[7,4]],7],1]");
        Debug.Assert(result.ToString() == "[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]");

        result += SnailNum.Parse("[[[[4,2],2],6],[8,7]]");
        Debug.Assert(result.ToString() == "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]");
        Debug.Assert(result.Magnitude() == 3488);

#endif

        result = lines.Aggregate((x, y) => x + y);



        return result.Magnitude().ToString();


    }
}
