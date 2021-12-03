public static class Extensions {
    public static IEnumerable<int> ToInts(this string str, int @base = 10, string separator = "\r\n") {
        return str.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToInts(@base, separator);
    }  
    public static IEnumerable<int> ToInts(this IEnumerable<string> str, int @base = 10, string separator = "\r\n") {
        return str.Select(x => Convert.ToInt32(x, @base));
    }
    public static IEnumerable<int> ToSortedInt(this string str, int @base = 10, string separator = "\r\n") {     
        return str.ToInts(@base, separator).OrderBy(x => x).ToArray();
    }    
    public static IEnumerable<int> ToSortedInt(this IEnumerable<string> str, int @base = 10, string separator = "\r\n") {
        return str.ToInts(@base, separator).OrderBy(x => x).ToArray();
    }
    public static IEnumerable<String> Lines(this string str) {
        return str.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    }

    // https://stackoverflow.com/questions/47815660/does-c-sharp-7-have-array-enumerable-destructuring
    public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest) {

        first = list.Count > 0 ? list[0] : default(T); // or throw
        rest = list.Skip(1).ToList();
    }

    public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest) {
        first = list.Count > 0 ? list[0] : default(T); // or throw
        second = list.Count > 1 ? list[1] : default(T); // or throw
        rest = list.Skip(2).ToList();
    }

    public static T Min<T>(this Span<T> value) where T:IComparable<T> {
        T min = value[0];
        for (int i = 1; i < value.Length; i++) {
            if (value[i].CompareTo(min) < 0) {
                min = value[i];
            }
        }
        return min;
    }
    public static T Max<T>(this Span<T> value) where T:IComparable<T>  {
        T max = value[0];
        for (int i = 1; i < value.Length; i++) {
            if (value[i].CompareTo(max) > 0 ) {
                max = value[i];
            }
        }
        return max;
    }
}