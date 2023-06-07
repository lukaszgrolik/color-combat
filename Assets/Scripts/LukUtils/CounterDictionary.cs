using System.Collections;
using System.Collections.Generic;

public interface IReadOnlyCounterDictionary<TKey> : IReadOnlyDictionary<TKey, int>
{
    int GetValue(TKey resource);
    bool Contains(TKey key, int value);
    bool ContainsAll(IReadOnlyDictionary<TKey, int> otherAmounts);
    // string ToString();
}

public class CounterDictionary<TKey> : Dictionary<TKey, int>, IReadOnlyCounterDictionary<TKey>
{
    // public CounterDictionary(bool allowNonExistingKeys = true, bool allowNegative = true)
    // {

    // }

    // always returns int (returns 0 if key is not present in the dictionary)
    public int GetValue(TKey key)
    {
        this.TryGetValue(key, out var value);

        return value;
    }

    public bool Contains(TKey key, int value)
    {
        this.TryGetValue(key, out var val);

        return val >= value;
    }

    public bool ContainsAll(IReadOnlyDictionary<TKey, int> otherValues)
    {
        if (this.Count < otherValues.Count) return false;

        foreach (var item in otherValues)
        {
            var keyFound = this.TryGetValue(item.Key, out var foundValue);
            if (keyFound == false || foundValue < item.Value)
            {
                return false;
            }
        }

        return true;
    }

    public void AddAmount(TKey key, int value)
    {
        if (ContainsKey(key) == false)
        {
            Add(key, value);
        }
        else
        {
            this[key] += value;
        }
    }

    public void SubtractAmount(TKey key, int value)
    {
        // if (value < 1) throw new System.Exception("Value must be greater than zero");
        // if (value > amount) throw new System.Exception("Cannot subtract below zero");

        if (ContainsKey(key) == false)
        {
            Add(key, -value);
        }
        else
        {
            this[key] -= value;
        }
    }

    public void UpdateAmount(TKey key, int change)
    {
        if (change > 0)
        {
            AddAmount(key, change);
        }
        else if (change < 0)
        {
            SubtractAmount(key, -change);
        }
    }

    public void UpdateAmounts(IReadOnlyDictionary<TKey, int> values)
    {
        foreach (var item in values)
        {
            UpdateAmount(item.Key, item.Value);
        }
    }

    // public override string ToString()
    // {
    //     if (this.Count == 0) return "(empty)";

    //     var strArr = new List<string>();

    //     foreach (var res in this)
    //     {
    //         strArr.Add($"{res.Key.Name}: {res.Value}");
    //     }

    //     return $"ResAmounts ({this.Count}) " + System.String.Join(" | ", strArr);
    // }
}