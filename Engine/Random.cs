using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{
    public class Random : System.Random
    {
        private static Random _instance;


        public static Random Instance()
        {
            return _instance ?? (_instance = new Random());
        }
        
        public T NextColor<T>()
        {
            // TODO: Save for later usage
            var types = Enum.GetValues(typeof(T)).Cast<T>().ToArray();

            return types[Next(0, types.Length)];
        }        
        
        public T NextEnum<T>()
        {
            // TODO: Save for later usage
            var types = Enum.GetValues(typeof(T)).Cast<T>().ToArray();

            return types[Next(0, types.Length)];
        }
        
        public T NextEnum<T>(IEnumerable<T> excludes)
        {
            // TODO: Save for later usage
            var types = Enum.GetValues(typeof(T)).Cast<T>().Except(excludes).ToArray();

            return types[Next(0, types.Length)];
        }
        
        public T Next<T>(T[] array)
        {
            return array[Next(0, array.Length)];
        }

        public Position Next(byte[][] jaggedArray, int maxHeight)
        {
            var availablePositions = new List<Position>();
            for (int i = 0; i < Math.Min(maxHeight, jaggedArray.Length); i++)
            {
                for (int j = 0; j < jaggedArray[0].Length; j++)
                {
                    if (jaggedArray[i][j] == 0)
                    {
                        availablePositions.Add(new Position(j, i));
                    }
                }
            }

            if (availablePositions.Count == 0)
            {
                return null;
            }

            return availablePositions[Next(0, availablePositions.Count)];
        }
    }
}