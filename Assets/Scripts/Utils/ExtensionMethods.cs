using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Utils
{
    public static class ExtensionMethods
    {
        public static TValue GetOrDefault<TKey,TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> defaultValue)
        {
            return source.TryGetValue(key, out TValue value) ? value : defaultValue();
        }

        public static TValue AddIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> defaultValue)
        {
            if (!source.TryGetValue(key, out TValue value))
            {
                value = defaultValue();
                source.Add(key, value);
            }
            return value;
        }

        public static HashSet<V> AddIfAbsent<TKey, V>(this IDictionary<TKey, HashSet<V>> source, TKey key)
        {
            return source.AddIfAbsent(key, ()=>new HashSet<V>());
        }

        public static List<V> AddIfAbsent<TKey, V>(this IDictionary<TKey, List<V>> source, TKey key)
        {
            return source.AddIfAbsent(key, () => new List<V>());
        }

        public static bool AddToSub<TKey, TValue>(this IDictionary<TKey, HashSet<TValue>> source, TKey key, TValue value)
        {
            return source.AddIfAbsent(key).Add(value);
        }

        public static bool RemoveFromSub<TKey, TValue>(this IDictionary<TKey, HashSet<TValue>> source, TKey key, TValue value)
        {
            if (!source.TryGetValue(key, out HashSet <TValue> subSet))
            {
                return true;
            }
            return subSet.Remove(value);
        }

        public static T[][] ToJaggedArray<T>(this T[,] twoDimensionalArray)
        {
            int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i, j];
                }
            }
            return jaggedArray;
        }
    }
}
