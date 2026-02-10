using System.Collections.Generic;

namespace Deviloop
{
    public static class ListUtilities
    {
        public static void ShuffleItems<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = SeededRandom.Range(0, n + 1);
                T temp = list[k];
                list[k] = list[n];
                list[n] = temp;
            }
        }

        public static T GetRandomElement<T>(List<T> list)
        {
            int rand = SeededRandom.Range(0, list.Count);
            return list[rand];
        }
    }
}