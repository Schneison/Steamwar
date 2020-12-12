using System;
using System.Collections.Generic;

namespace Steamwar.Utils {
    public class WeightedRandom
    {

        public static int GetTotalWeight<T>(List<T> collection) where T : RandomItem
        {
            int i = 0;
            int j = 0;

            for (int k = collection.Count; j < k; ++j)
            {
                RandomItem item = collection[j];
                i += item.weight;
            }

            return i;
        }

        public static T GetRandomItem<T>(Random random, T[] collection, int totalWeight) where T : RandomItem
        {
            return GetRandomItem(random, new List<T>(collection), totalWeight);
        }

        public static T GetRandomItem<T>(Random random, List<T> collection, int totalWeight) where T : RandomItem
        {
            if (totalWeight <= 0)
            {
                throw new ArgumentException();
            }
            else
            {
                int i = random.Next(totalWeight);
                return (T)GetRandomItem(collection, i);
            }
        }

        public static T GetRandomItem<T>(List<T> collection, int weight) where T : RandomItem
        {
            int i = 0;

            for (int j = collection.Count; i < j; ++i)
            {
                T t = collection[i];
                weight -= t.weight;

                if (weight < 0)
                {
                    return t;
                }
            }

            return default(T);
        }

        public static T GetRandomItem<T>(Random random, T[] collection) where T : RandomItem
        {
            return GetRandomItem(random, new List<T>(collection));
        }

        public static T GetRandomItem<T>(Random random, List<T> collection) where T : RandomItem
        {
            return GetRandomItem(random, collection, GetTotalWeight(collection)) as T;
        }
    }

    [Serializable]
    public class RandomItem
    {
        public int weight;
    }

}