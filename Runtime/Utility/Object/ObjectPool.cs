using System.Collections.Generic;
using UnityEngine;

namespace Suk.Utility
{
	public class ObjectPool<T> where T : MonoBehaviour
	{
		readonly List<T> _activeItemList = new List<T>();
		readonly Transform _activeParent;
		readonly Transform _inactiveParent;
		readonly T _itemPrefab;
		readonly UnityEngine.Pool.ObjectPool<T> _objectPool;

		// Constructor to initialize the pool with parameters
		public ObjectPool(T prefab, Transform activeParent, Transform inactiveParent)
		{
			_itemPrefab = prefab;
			_activeParent = activeParent;
			_inactiveParent = inactiveParent;

			_objectPool = new UnityEngine.Pool.ObjectPool<T>(
				CreateItem,     // Creates a new pooled item
				OnTakeFromPool, // Called when an item is requested (active)
				OnReturnToPool, // Called when an item is returned (inactive)
				OnDestroyItem   // Called when an item is destroyed
			);
		}

		// Number of active items in the pool
		public int CountActive => _objectPool.CountActive;

		// Number of inactive items in the pool
		public int CountInactive => _objectPool.CountInactive;

		// Total number of items in the pool
		public int CountAll => _objectPool.CountAll;

		// 생성 또는 활성화상태로 전환
		public T Get()
		{
			T item = _objectPool.Get();
			_activeItemList.Add(item); // Track active items
			return item;
		}

		// 대상 비활성화
		public void Release(T item)
		{
			if (!_activeItemList.Contains(item))
				return;
			_activeItemList.Remove(item); // Remove from active list
			_objectPool.Release(item);    // Return to pool
		}

		public void ReleaseAll()
		{
			List<T> activeItems = new List<T>(_activeItemList);
			foreach (T item in activeItems)
				Release(item);
		}

		public List<T> GetActiveItemList() { return _activeItemList; }


		// 1. Create a new item in the pool
		T CreateItem()
		{
			T item = Object.Instantiate(_itemPrefab, _activeParent);
			item.gameObject.SetActive(false);
			return item;
		}

		// 2. When taking from pool, activate and re-parent the item
		void OnTakeFromPool(T item)
		{
			item.gameObject.SetActive(true);
			item.transform.SetParent(_activeParent);
		}

		// 3. When returning to pool, deactivate and re-parent the item
		void OnReturnToPool(T item)
		{
			item.gameObject.SetActive(false);
			if (_inactiveParent != null)
				item.transform.SetParent(_inactiveParent);
		}

		// 4. Destroy item when removed from pool
		void OnDestroyItem(T item) { Object.Destroy(item); }
	}

	public class ObjectPool
	{
		readonly List<GameObject> _activeItemList = new List<GameObject>();
		readonly Transform _activeParent;
		readonly Transform _inactiveParent;
		readonly GameObject _itemPrefab;
		readonly UnityEngine.Pool.ObjectPool<GameObject> _objectPool;

		// Constructor to initialize the pool with parameters
		public ObjectPool(GameObject prefab, Transform activeParent, Transform inactiveParent)
		{
			_itemPrefab = prefab;
			_activeParent = activeParent;
			_inactiveParent = inactiveParent;

			_objectPool = new UnityEngine.Pool.ObjectPool<GameObject>(
				CreateItem,     // Creates a new pooled item
				OnTakeFromPool, // Called when an item is requested (active)
				OnReturnToPool, // Called when an item is returned (inactive)
				OnDestroyItem   // Called when an item is destroyed
			);
		}

		// Number of active items in the pool
		public int CountActive => _objectPool.CountActive;

		// Number of inactive items in the pool
		public int CountInactive => _objectPool.CountInactive;

		// Total number of items in the pool
		public int CountAll => _objectPool.CountAll;

		// 생성 또는 활성화상태로 전환
		public GameObject Get()
		{
			GameObject item = _objectPool.Get();
			_activeItemList.Add(item); // Track active items
			return item;
		}

		// 대상 비활성화
		public void Release(GameObject item)
		{
			if (!_activeItemList.Contains(item))
				return;
			_activeItemList.Remove(item); // Remove from active list
			_objectPool.Release(item);    // Return to pool
		}

		public void ReleaseAll()
		{
			List<GameObject> activeItems = new List<GameObject>(_activeItemList);
			foreach (GameObject item in activeItems)
				Release(item);
		}

		public List<GameObject> GetActiveItemList() { return _activeItemList; }

		// 1. Create a new item in the pool
		GameObject CreateItem()
		{
			GameObject item = Object.Instantiate(_itemPrefab, _activeParent);
			item.SetActive(false);
			return item;
		}

		// 2. When taking from pool, activate and re-parent the item
		void OnTakeFromPool(GameObject item)
		{
			item.SetActive(true);
			item.transform.SetParent(_activeParent);
		}

		// 3. When returning to pool, deactivate and re-parent the item
		void OnReturnToPool(GameObject item)
		{
			item.SetActive(false);
			if (_inactiveParent != null)
				item.transform.SetParent(_inactiveParent);
		}

		// 4. Destroy item when removed from pool
		void OnDestroyItem(GameObject item) { Object.Destroy(item); }
	}
}
