using System;
using System.Collections;
using System.Collections.Generic;

namespace Suk.DictionaryExtension
{
	public class BiDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		private Dictionary<TKey, TValue> _forward = new Dictionary<TKey, TValue>();
		private Dictionary<TValue, TKey> _backward = new Dictionary<TValue, TKey>();

		public void Add(TKey key, TValue value)
		{
			if (_forward.ContainsKey(key) || _backward.ContainsKey(value))
				throw new ArgumentException("Duplicate key or value is not allowed.");

			_forward[key] = value;
			_backward[value] = key;
		}

		public bool TryGetByKey(TKey key, out TValue value)
		{
			return _forward.TryGetValue(key, out value);
		}

		public bool TryGetByValue(TValue value, out TKey key)
		{
			return _backward.TryGetValue(value, out key);
		}

		public TValue GetByKey(TKey key)
		{
			return _forward[key];
		}

		public TKey GetByValue(TValue value)
		{
			return _backward[value];
		}

		public void RemoveByKey(TKey key)
		{
			if (_forward.TryGetValue(key, out TValue value))
			{
				_forward.Remove(key);
				_backward.Remove(value);
			}
		}

		public void RemoveByValue(TValue value)
		{
			if (_backward.TryGetValue(value, out TKey key))
			{
				_backward.Remove(value);
				_forward.Remove(key);
			}
		}


		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _forward.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

}
