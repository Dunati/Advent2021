

public class DefaultDictionary<TKey, TValue> {

    public bool ContainsKey(TKey index) {
        return entries.ContainsKey(index);
    }

    public Dictionary<TKey, TValue>.ValueCollection Values => entries.Values;

    public TValue this[TKey index] {
        get {
            TValue value = default(TValue);
            entries.TryGetValue(index, out value);
            return value;
        }
        set {
            if (value.Equals(default(TValue))) {
                entries.Remove(index);
            }
            else {
                entries[index] = value;
            }
        }
    }

    private Dictionary<TKey, TValue> entries = new Dictionary<TKey, TValue>();
}