using System.Collections.Generic;
using UnityEngine;

public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	[SerializeField] private List<TKey> keys = new List<TKey>();
	[SerializeField] private List<TValue> value = new List<TValue>();

	public void OnBeforeSerialize()
	{
		keys.Clear();
		value.Clear();
		foreach (KeyValuePair<TKey, TValue> kvp in this)
		{
			keys.Add(kvp.Key);
			value.Add(kvp.Value);
		}
	}
	public void OnAfterDeserialize()
	{
		this.Clear();
		for (int i = 0; i < keys.Count; i++)
		{
			this.Add(keys[i], value[i]);
		}
	}
}
