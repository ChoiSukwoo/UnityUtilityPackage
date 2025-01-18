using System;
using System.Collections.Generic;
using System.Linq;

namespace Suk.Util.Extensions
{

   public static class ListExtensions
   {
      // Map 메서드
      public static List<TResult> Map<TSource, TResult>(this List<TSource> list, Func<TSource, TResult> selector) { return list.Select(selector).ToList(); }

      public static List<TResult> Map<TSource, TResult>(this List<TSource> list, Func<TSource, int, TResult> selector) { return list.Select(selector).ToList(); }

      public static void Map<TSource>(this List<TSource> list, Action<TSource> selector)
      {
         foreach (var item in list)
            selector(item);
      }

      public static void Map<TSource>(this List<TSource> list, Action<TSource, int> action)
      {
         for (int i = 0; i < list.Count; i++) {
            action(list[i], i);
         }
      }


      // Filter function: 조건에 맞는 요소들만 필터링하여 새로운 리스트를 반환합니다.
      public static List<TSource> Filter<TSource>(this List<TSource> list, Func<TSource, bool> predicate) { return list.Where(predicate).ToList(); }

      public static List<TSource> Filter<TSource>(this List<TSource> list, Func<TSource, int, bool> predicate) { return list.Where(predicate).ToList(); }

      // Reduce function: 리스트의 모든 요소를 하나의 값으로 축소합니다.
      public static TResult Reduce<TSource, TResult>(this List<TSource> list, TResult seed, Func<TResult, TSource, TResult> accumulator) { return list.Aggregate(seed, accumulator); }

      public static TResult Reduce<TSource, TResult>(this List<TSource> list, TResult seed, Func<TResult, TSource, int, TResult> accumulator)
      {
         TResult result = seed;
         for (int i = 0; i < list.Count; i++) {
            result = accumulator(result, list[i], i);
         }
         return result;
      }


      // ForEach function: 리스트의 각 요소에 대해 주어진 액션을 수행합니다.
      public static void ForEach<TSource>(this List<TSource> list, Action<TSource> action)
      {
         foreach (var item in list) {
            action(item);
         }
      }


      public static void ForEach<TSource>(this List<TSource> list, Action<TSource, int> action)
      {
         for (int i = 0; i < list.Count; i++) {
            action(list[i], i);
         }
      }
   }

}