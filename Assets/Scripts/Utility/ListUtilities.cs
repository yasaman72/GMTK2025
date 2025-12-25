using System.Collections.Generic;

namespace Deviloop
{
    public static class ListUtilities
    {
        public static void ShuffleItems<T>(List<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T temp = list[k];
                list[k] = list[n];
                list[n] = temp;
            }
        }

        public static T GetRandomElement<T>(List<T> list)
        {
            int rand = UnityEngine.Random.Range(0, list.Count);
            return list[rand];
        }
    }
}
