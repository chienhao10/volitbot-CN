using System;
using System.Collections.Generic;
using System.Linq;

namespace VoliBot
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.Shuffle(new Random());
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (rng == null)
			{
				throw new ArgumentNullException("rng");
			}
			return source.ShuffleIterator(rng);
		}

		private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random rng)
		{
			List<T> list = source.ToList<T>();
			for (int i = 0; i < list.Count; i++)
			{
				int index = rng.Next(i, list.Count);
				yield return list[index];
				list[index] = list[i];
			}
			yield break;
		}
	}
}
