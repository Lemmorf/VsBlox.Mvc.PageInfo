using System;
using System.Collections.Generic;
using System.Linq;

namespace VsBlox.Mvc.PageInfo.Extensions
{
  /// <summary>
  /// Linq extension methods
  /// </summary>
  internal static class LinqExtensions
  {
    /// <summary>
    /// Using() clause to be used in a Linq query.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    public static IEnumerable<T> Use<T>(this T obj) where T : IDisposable
    {
      try
      {
        yield return obj;
      }
      finally
      {
        // ReSharper disable once CompareNonConstrainedGenericWithNull
        if (obj != null) obj.Dispose();
      }
    }

    /// <summary>
    /// Distincts the by.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="list">The list.</param>
    /// <param name="propertySelector">The property selector.</param>
    /// <returns></returns>
    public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> list, Func<T, object> propertySelector)
    {
      return list.GroupBy(propertySelector).Select(x => x.First());
    }
  }
}