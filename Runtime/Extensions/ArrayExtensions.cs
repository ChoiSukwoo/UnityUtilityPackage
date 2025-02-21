using System;
using System.Linq;

namespace Suk.Extensions
{
    public static class ArrayExtensions
    {
        // Map 메서드
        public static TOutput[] Map<TInput, TOutput>(this TInput[] array, Func<TInput, TOutput> func)
        {
            return array.Select(func).ToArray();
        }

        public static TOutput[] Map<TInput, TOutput>(this TInput[] array, Func<TInput, int, TOutput> func)
        {
            return array.Select(func).ToArray();
        }

        public static void Map<TInput>(this TInput[] array, Action<TInput> action)
        {
            foreach (var item in array)
            {
                action(item);
            }
        }

        public static void Map<TInput>(this TInput[] array, Action<TInput, int> action)
        {
            for (int i = 0; i < array.Length; i++)
            {
                action(array[i], i);
            }
        }

        // Filter function: 조건에 맞는 요소들만 필터링하여 새로운 배열을 반환합니다.
        public static T[] Filter<T>(this T[] array, Func<T, bool> predicate)
        {
            return array.Where(predicate).ToArray();
        }

        public static T[] Filter<T>(this T[] array, Func<T, int, bool> predicate)
        {
            return array.Where(predicate).ToArray();
        }

        // Reduce function: 배열의 모든 요소를 하나의 값으로 축소합니다.
        public static TResult Reduce<TInput, TResult>(this TInput[] array, TResult seed, Func<TResult, TInput, TResult> accumulator)
        {
            return array.Aggregate(seed, accumulator);
        }

        public static TResult Reduce<TInput, TResult>(this TInput[] array, TResult seed, Func<TResult, TInput, int, TResult> accumulator)
        {
            TResult result = seed;
            for (int i = 0; i < array.Length; i++)
            {
                result = accumulator(result, array[i], i);
            }
            return result;
        }

        // ForEach function: 배열의 각 요소에 대해 주어진 액션을 수행합니다.
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (var item in array)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this T[] array, Action<T, int> action)
        {
            for (int i = 0; i < array.Length; i++)
            {
                action(array[i], i);
            }
        }
    }
}
