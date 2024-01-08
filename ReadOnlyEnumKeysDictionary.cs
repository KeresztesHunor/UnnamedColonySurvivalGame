using System;
using System.Collections.Generic;

public class ReadOnlyEnumKeysDictionary<TKey, TValue> where TKey : struct, Enum
{
    Dictionary<TKey, TValue> dict { get; }

    public ReadOnlyEnumKeysDictionary()
    {
        dict = new Dictionary<TKey, TValue>();
        TKey[] keys = Enum.GetValues<TKey>();
        foreach (TKey key in keys)
        {
            dict.Add(key, default(TValue));
        }
    }

    public TValue this[TKey key]
    {
        get => dict[key];
        set => dict[key] = value;
    }
}
