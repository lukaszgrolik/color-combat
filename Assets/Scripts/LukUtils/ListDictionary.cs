using System.Collections;
using System.Collections.Generic;

public interface IReadOnlyListDictionary<K, V> : IReadOnlyDictionary<K, List<V>>
{
    bool IsEmpty(K key);
}

public class ListDictionary<K, V> : Dictionary<K, List<V>>, IReadOnlyListDictionary<K, V>
{
    public bool IsEmpty(K key)
    {
        return this.ContainsKey(key) == false;
    }

    public void AddListItem(K key, V value)
    {
        if (ContainsKey(key) == false)
        {
            Add(key, new List<V>(){ value });
        }
        else
        {
            this[key].Add(value);
        }
    }

    public void AddListItems(K key, IReadOnlyList<V> list)
    {
        if (ContainsKey(key) == false)
        {
            Add(key, new List<V>(list));
        }
        else
        {
            this[key].AddRange(list);
        }
    }

    public void RemoveListItem(K key, V value)
    {
        this[key].Remove(value);

        if (this[key].Count == 0)
        {
            Remove(key);
        }
    }
}