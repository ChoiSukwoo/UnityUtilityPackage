using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Suk.Extensions
{
	public static class ListExtensions
	{
		//반환값 있는 Map
		public static List<TResult> Map<TSource, TResult>(this List<TSource> list, Func<TSource, TResult> action) { return list.Select(action).ToList(); }

		public static List<TResult> Map<TSource, TResult>(this List<TSource> list, Func<TSource, int, TResult> action) { return list.Select(action).ToList(); }

		public static List<TResult> Map<TResult>(this int self, Func<int, TResult> action) { return Enumerable.Range(0, self).Select(action).ToList(); }
		public static List<TResult> Map<TResult>(this int self, Func<TResult> action) { return Enumerable.Range(0, self).Select(_ => action()).ToList(); }

		//반환값 없는 ForEach
		public static void ForEach<TSource>(this List<TSource> list, Action<TSource> action)
		{
			foreach (TSource item in list)
				action(item);
		}

		public static void ForEach<TSource>(this List<TSource> list, Action<TSource, int> action)
		{
			for (int i = 0; i < list.Count; i++)
				action(list[i], i);
		}

		public static void ForEach(this int self, Action<int> action)
		{
			foreach (int i in Enumerable.Range(0, self))
				action(i);
		}

		public static void ForEach(this int self, Action action)
		{
			foreach (int i in Enumerable.Range(0, self))
				action();
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
		
		public static bool TryGetRandomElements<T>(this List<T> list, int count, out List<T> elements)
		{
			elements = new List<T>();
			if (list == null || list.Count == 0 || count <= 0) {
				return false;
			}

			// 중복을 허용하지 않고 n개의 요소를 선택하려면, 원본 리스트를 셔플 후 상위 n개를 추출
			if (count >= list.Count) {
				elements.AddRange(list); // 요청된 개수가 리스트 전체 크기 이상이면 전체 반환
				return true;
			}

			var shuffledList = new List<T>(list);         // 기존 리스트 복사
			shuffledList.Shuffle();                       // 셔플하여 섞음
			elements = shuffledList.Take(count).ToList(); // 지정한 개수만큼 선택
			return true;
		}

		#region Swap
		/// <summary>두 인덱스 위치에 있는 값을 안전하게 교환합니다.</summary>
		public static bool TrySwap<T>(this IList<T> list, int firstIndex, int secondIndex)
		{
			// 리스트가 null인지 검사
			if (list == null || list.Count < 2) {
				return false; // 리스트가 null이거나 교환할 요소가 부족한 경우 실패 처리
			}

			// 유효한 인덱스인지 검사
			if (firstIndex < 0 || firstIndex >= list.Count || secondIndex < 0 || secondIndex >= list.Count) {
				return false; // 인덱스가 범위를 벗어난 경우 실패 처리
			}
			
			// 값 교환
			T firstValue = list[firstIndex];
			list[firstIndex] = list[secondIndex];
			list[secondIndex] = firstValue;

			return true; // 성공적으로 교환됨
		}
		
		/// <summary>두 인덱스 위치에 있는 값을 안전하게 교환합니다.</summary>
		public static bool TrySwapImmutable<T>(this IList<T> list, int firstIndex, int secondIndex, out List<T> swappedList)
		{
			// 리스트가 null인지 검사
			if (list == null) {
				swappedList = null;
				return false; // 리스트가 null이거나 교환할 요소가 부족한 경우 실패 처리
			}

			// 유효한 인덱스인지 검사
			if ( list.Count < 2 || firstIndex < 0 || firstIndex >= list.Count || secondIndex < 0 || secondIndex >= list.Count) {
				swappedList = new List<T>(list);
				return false; // 인덱스가 범위를 벗어난 경우 실패 처리
			}

			// 새로운 리스트 생성
			var newList = new List<T>(list);
			
			// 값 교환
			T firstValue = newList[firstIndex];
			newList[firstIndex] = newList[secondIndex];
			newList[secondIndex] = firstValue;

			swappedList = newList;
			return true; // 성공적으로 교환됨
		}
		#endregion

		#region Shuffle
		/// <summary>리스트의 값을 Fisher-Yates 알고리즘을 사용하여 무작위로 섞습니다.</summary>
		public static void Shuffle<T>(this IList<T> list)
		{
			// 리스트가 null이거나 요소가 2개 미만인 경우, 섞을 필요 없음
			if (list == null || list.Count < 2) {
				return;
			}

			for (int i = 0; i < list.Count; i++) {
				int randomIndex = Random.Range(i, list.Count);
				
				T temp = list[i];
				list[i] = list[randomIndex];
				list[randomIndex] = temp;
			}
		}
				
		/// <summary>Shuffle 메서드를 랜덤 시드(seed)를 사용하여 섞습니다.</summary>
		public static void Shuffle<T>(this IList<T> list, int seed)
		{
			Random.State state = Random.state;
			Random.InitState(seed);
			Shuffle(list);
			Random.state = state;
		}
		
		public static IList<T> ShuffleImmutable<T>(this IList<T> list)
		{
			// 리스트가 null이거나 요소가 2개 미만인 경우, 섞을 필요 없음
			if (list == null) {
				return null;
			}

			if (list.Count < 2) {
				return new List<T>(list);
			}
			
			var newList = new List<T>(list);
			
			for (int i = 0; i < list.Count; i++) {
				int randomIndex = Random.Range(i, list.Count);
				
				T temp = newList[i];
				newList[i] = newList[randomIndex];
				newList[randomIndex] = temp;
			}

			return newList;
		}
		
		/// <summary>Shuffle 메서드를 랜덤 시드(seed)를 사용하여 섞습니다.</summary>
		public static IList<T> ShuffleImmutable<T>(this IList<T> list, int seed)
		{
			Random.State state = Random.state;
			Random.InitState(seed);
			IList<T> newList = ShuffleImmutable(list);
			Random.state = state;
			return newList;
		}
		#endregion

		#region Rotate
		/// <summary>리스트의 아이템들을 왼쪽으로 회전시킵니다.</summary>
		public static void RotateLeft<T>(this IList<T> list, int count = 1)
		{
			if (list == null)
				return;

			if (list.Count < 2|| count <= 0)
				return;

			for (int current = 0; current < count; current++) {
				T first = list[0];
				list.RemoveAt(0);
				list.Add(first);
			}
		}

		public static IList<T> RotateLeftImmutable<T>(this IList<T> list, int count = 1)
		{
			if (list == null) 
				return null;
			
			if (list.Count < 2 || count <= 0)
				return new List<T>(list);

			int rotation = count % list.Count; // 회전 수가 리스트 크기 이상이면 필요 없는 회전 제거
			var rotatedList = new List<T>(list.Count);

			// 새로운 리스트에 왼쪽을 기준으로 재배치
			rotatedList.AddRange(list.Skip(rotation));
			rotatedList.AddRange(list.Take(rotation));

			return rotatedList;
		}
		
		/// <summary>리스트의 아이템들을 오른쪽으로 회전시킵니다.</summary>
		public static void RotateRight<T>(this IList<T> list, int count = 1)
		{
			if (list == null)
				return;

			if (list.Count < 2)
				return;

			int lastIndex = list.Count - 1;
			for (int current = 0; current < count; current++) {
				T last = list[lastIndex];
				list.RemoveAt(lastIndex);
				list.Insert(0, last);
			}
		}
		
		/// <summary>리스트의 아이템들을 오른쪽으로 회전시킨 새 리스트를 반환합니다.</summary>
		public static IList<T> RotateRightImmutable<T>(this IList<T> list, int count = 1)
		{
			// 리스트가 null이거나 회전할 필요가 없으면 원본 리스트 복사본 반환
			if (list == null) 
				return null;
			
			if (list.Count < 2 || count <= 0)
				return new List<T>(list);

			int rotation = count % list.Count; // 회전 수가 리스트 크기 이상이면 필요 없는 회전 제거
			var rotatedList = new List<T>(list.Count);

			// 새로운 리스트에 오른쪽을 기준으로 재배치
			rotatedList.AddRange(list.Skip(list.Count - rotation));
			rotatedList.AddRange(list.Take(list.Count - rotation));

			return rotatedList;
		}
		#endregion

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

		public static bool TryGetValue<T>(this IList<T> list, int index, out T value)
		{
			if (list == null || index < 0 || index >= list.Count) {
				value = default;
				return false;
			}

			value = list[index];
			return true;
		}
	}
}
