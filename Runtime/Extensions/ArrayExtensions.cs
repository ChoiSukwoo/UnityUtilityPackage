using System;
using System.Linq;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Suk.Extensions
{
	public static class ArrayExtensions
	{
		// Map 메서드
		public static TResult[] Map<TSource, TResult>(this TSource[] array, Func<TSource, TResult> action) { return array.Select(action).ToArray(); }

		public static TResult[] Map<TSource, TResult>(this TSource[] array, Func<TSource, int, TResult> action) { return array.Select(action).ToArray(); }

		public static void Map<TSource>(this TSource[] array, UnityAction<TSource> action)
		{
			foreach (var item in array)
				action(item);
		}

		public static void Map<TSource>(this TSource[] array, UnityAction<TSource, int> action)
		{
			for (int i = 0; i < array.Length; i++)
				action(array[i], i);
		}

		// Filter function: 조건에 맞는 요소들만 필터링하여 새로운 배열을 반환합니다.
		public static TSource[] Filter<TSource>(this TSource[] array, Func<TSource, bool> predicate) { return array.Where(predicate).ToArray(); }

		public static TSource[] Filter<TSource>(this TSource[] array, Func<TSource, int, bool> predicate) { return array.Where(predicate).ToArray(); }

		// Reduce function: 배열의 모든 요소를 하나의 값으로 축소합니다.
		public static TResult Reduce<TSource, TResult>(this TSource[] array, TResult seed, Func<TResult, TSource, TResult> accumulator) { return array.Aggregate(seed, accumulator); }

		public static TResult Reduce<TSource, TResult>(this TSource[] array, TResult seed, Func<TResult, TSource, int, TResult> accumulator)
		{
			TResult result = seed;
			for (int i = 0; i < array.Length; i++)
				result = accumulator(result, array[i], i);
			return result;
		}

		// ForEach function: 배열의 각 요소에 대해 주어진 액션을 수행합니다.
		public static void ForEach<TSource>(this TSource[] array, Action<TSource> action)
		{
			foreach (var item in array)
				action(item);
		}

		public static void ForEach<TSource>(this TSource[] array, Action<TSource, int> action)
		{
			for (int i = 0; i < array.Length; i++)
				action(array[i], i);
		}

		/// <summary>배열에서 랜덤한 요소를 가져옵니다.</summary>
		public static bool TryGetRandomElement<T>(this T[] array, out T element)
		{
			if (array == null || array.Length == 0) {
				element = default;
				return false;
			}
			int randomIndex = Random.Range(0, array.Length);
			element = array[randomIndex];
			return true;
		}

		/// <summary>두 인덱스 위치에 있는 값을 교환합니다.</summary>
		public static void Swap<T>(this T[] array, int firstIndex, int secondIndex)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));

			if (array.Length < 2)
				throw new ArgumentException("Array length should be at least 2 for a swap.");

			T firstValue = array[firstIndex];
			array[firstIndex] = array[secondIndex];
			array[secondIndex] = firstValue;
		}

		/// <summary>배열의 값을 Fisher-Yates 알고리즘을 사용하여 무작위로 섞습니다.</summary>
		public static void Shuffle<T>(this T[] array)
		{
			for (int i = 0; i < array.Length; i++) {
				int randomIndex = Random.Range(i, array.Length);
				Swap(array, randomIndex, i);
			}
		}

		/// <summary>Shuffle 메서드를 랜덤 시드(seed)를 사용하여 섞습니다.</summary>
		public static void Shuffle<T>(this T[] array, int seed)
		{
			var state = Random.state;
			Random.InitState(seed);
			Shuffle(array);
			Random.state = state;
		}

		/// <summary>배열의 아이템들을 왼쪽으로 회전시킵니다.</summary>
		public static void RotateLeft<T>(this T[] array, int count = 1)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));

			if (array.Length < 2)
				return;

			for (int current = 0; current < count; current++) {
				T first = array[0];
				Array.Copy(array, 1, array, 0, array.Length - 1);
				array[^1] = first;
			}
		}

		/// <summary>배열의 아이템들을 오른쪽으로 회전시킵니다.</summary>
		public static void RotateRight<T>(this T[] array, int count = 1)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));

			if (array.Length < 2)
				return;

			int lastIndex = array.Length - 1;
			for (int current = 0; current < count; current++) {
				T last = array[lastIndex];
				Array.Copy(array, 0, array, 1, array.Length - 1);
				array[0] = last;
			}
		}

		/// <summary>배열에서 null 값을 제거합니다.</summary>
		public static void RemoveNullEntries<T>(this T[] array) where T : class { array = array.Where(item => item != null).ToArray(); }

		/// <summary>배열에서 기본값(default value)을 제거합니다.</summary>
		public static void RemoveDefaultValues<T>(this T[] array) { array = array.Where(item => !Equals(default(T), item)).ToArray(); }

		/// <summary>주어진 인덱스가 배열의 범위 내에 있는지 확인합니다.</summary>
		public static bool HasIndex<T>(this T[] array, int index) { return index >= 0 && index < array.Length; }
	}
}
