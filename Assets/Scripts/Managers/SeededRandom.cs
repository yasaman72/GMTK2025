using UnityEngine;
using System;

namespace Deviloop
{
    public class SeededRandom : Singleton<SeededRandom>
    {
        private static int seed;
        private static System.Random rng;

        public static int GetSeed() => seed;

        [SerializeField] private bool shouldLog = true;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            seed = GenerateSeed();
            rng = new System.Random(seed);

            Logger.Log("Current random seed: " + seed, shouldLog);
        }

        public static void SetSeed(int newSeed)
        {
            seed = newSeed;
            rng = new System.Random(seed);

            Logger.Log("Random seed set to: " + seed);
        }

        private static int GenerateSeed()
        {
            return Environment.TickCount;
        }

        public static int Range(int minInclusive, int maxExclusive)
        {
            return rng.Next(minInclusive, maxExclusive);
        }

        public static float Range(float minInclusive, float maxInclusive)
        {
            return minInclusive + (float)rng.NextDouble() * (maxInclusive - minInclusive);
        }

        public static float Value()
        {
            return (float)rng.NextDouble();
        }
    }
}
