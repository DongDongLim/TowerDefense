using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();
    [SerializeField]
    private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();
        if (keys.Count > values.Count)
        {
            for (int i = values.Count; i < keys.Count; ++i)
            {
                values.Add(default(TValue));
            }
        }
        else if (keys.Count < values.Count)
        {
            for (int i = keys.Count; i < values.Count; ++i)
            {
                values.RemoveAt(i);
            }
        }
        for (int i = 0; i < keys.Count; i++)
        {
            if (this.ContainsKey(keys[i]) == true)
                keys[i] = default(TKey);
            this.Add(keys[i], values[i]);
        }
    }
}

