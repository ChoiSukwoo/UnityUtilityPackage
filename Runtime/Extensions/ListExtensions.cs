using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Suk.Extensions
{
	public static class ListExtensions
	{
		// Map 메서드
		public static List<TResult> Map<TSource, TResult>(this List<TSource> list, Func<TSource, TResult> action) { return list.Select(action).ToList(); }

		public static List<TResult> Map<TSource, TResult>(this List<TSource> list, Func<TSource, int, TResult> action) { return list.Select(action).ToList(); }

		public static void Map<TSource>(this List<TSource> list, UnityAction<TSource> action)
		{
			foreach (var item in list)
				action(item);
		}

		public static void Map<TSource>(this List<TSource> list, UnityAction<TSource, int> action)
		{
			for (int i = 0; i < list.Count; i++)
				action(list[i], i);
		}

		// Filter function: 조건에 맞는 요소들만 필터링하여 새로운 리스트를 반환합니다.
		public static List<TSource> Filter<TSource>(this List<TSource> list, Func<TSource, bool> predicate) { return list.Where(predicate).ToList(); }

		public static List<TSource> Filter<TSource>(this List<TSource> list, Func<TSource, int, bool> predicate) { return list.Where(predicate).ToList(); }

		// Reduce function: 리스트의 모든 요소를 하나의 값으로 축소합니다.
		public static TResult Reduce<TSource, TResult>(this List<TSource> list, TResult seed, Func<TResult, TSource, TResult> accumulator) { return list.Aggregate(seed, accumulator); }

		public static TResult Reduce<TSource, TResult>(this List<TSource> list, TResult seed, Func<TResult, TSource, int, TResult> accumulator)
		{
			TResult result = seed;
			for (int i = 0; i < list.Count; i++)
				result = accumulator(result, list[i], i);
			return result;
		}


		// ForEach function: 리스트의 각 요소에 대해 주어진 액션을 수행합니다.
		public static void ForEach<TSource>(this List<TSource> list, Action<TSource> action)
		{
			foreach (var item in list)
				action(item);
		}

		public static void ForEach<TSource>(this List<TSource> list, Action<TSource, int> action)
		{
			for (int i = 0; i < list.Count; i++)
				action(list[i], i);
		}

		/// <summary>리스트에서 랜덤한 요소를 가져옵니다.</summary>
		public static bool TryGetRandomElement<T>(this List<T> list, out T element)
		{
			if (list == null || list.Count == 0) {
				element = default;
				return false;
			}
			int randomIndex = Random.Range(0, list.Count);
			element = list[randomIndex];
			return true;
		}

		/// <summary>두 인덱스 위치에 있는 값을 교환합니다.</summary>
		public static void Swap<T>(this IList<T> list, int firstIndex, int secondIndex)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

			if (list.Count < 2)
				throw new ArgumentException("List count should be at least 2 for a swap.");

			T firstValue = list[firstIndex];
			list[firstIndex] = list[secondIndex];
			list[secondIndex] = firstValue;
		}

		/// <summary>리스트의 값을 Fisher-Yates 알고리즘을 사용하여 무작위로 섞습니다.</summary>
		public static void Shuffle<T>(this IList<T> list)
		{
			for (int i = 0; i < list.Count; i++) {
				int randomIndex = Random.Range(i, list.Count);
				Swap(list, randomIndex, i);
			}
		}

		/// <summary>Shuffle 메서드를 랜덤 시드(seed)를 사용하여 섞습니다.</summary>
		public static void Shuffle<T>(this IList<T> list, int seed)
		{
			var state = Random.state;
			Random.InitState(seed);
			Shuffle(list);
			Random.state = state;
		}

		/// <summary>리스트의 아이템들을 왼쪽으로 회전시킵니다.</summary>
		public static void RotateLeft<T>(this IList<T> list, int count = 1)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

			if (list.Count < 2)
				return;

			for (int current = 0; current < count; current++) {
				T first = list[0];
				list.RemoveAt(0);
				list.Add(first);
			}
		}

		/// <summary>리스트의 아이템들을 오른쪽으로 회전시킵니다.</summary>
		public static void RotateRight<T>(this IList<T> list, int count = 1)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

			if (list.Count < 2)
				return;

			int lastIndex = list.Count - 1;
			for (int current = 0; current < count; current++) {
				T last = list[lastIndex];
				list.RemoveAt(lastIndex);
				list.Insert(0, last);
			}
		}

		/// <summary>리스트에서 null 값을 제거합니다.</summary>
		public static void RemoveNullEntries<T>(this IList<T> list) where T : class
		{
			for (int i = list.Count - 1; i >= 0; i--)
				if (Equals(list[i], null))
					list.RemoveAt(i);
		}

		/// <summary>리스트에서 기본값(default value)을 제거합니다.</summary>
		public static void RemoveDefaultValues<T>(this IList<T> list)
		{
			for (int i = list.Count - 1; i >= 0; i--)
				if (Equals(default(T), list[i]))
					list.RemoveAt(i);
		}

		/// <summary>주어진 인덱스가 리스트의 범위 내에 있는지 확인합니다.</summary>
		public static bool HasIndex<T>(this IList<T> list, int index) { return index >= 0 && index <= list.Count - 1; }
	}
}
