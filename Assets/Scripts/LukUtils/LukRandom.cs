using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LukRandom
{
    public static class Uniform
    {
        public static T Sample<T>(IReadOnlyList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static List<T> SampleMany<T>(this List<T> list, int number)
        {
            // this is the list we're going to remove picked items from
            List<T> tmpList = new List<T>(list);
            // this is the list we're going to move items to
            List<T> newList = new List<T>();

            // make sure tmpList isn't already empty
            while (newList.Count < number && tmpList.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, tmpList.Count);
                newList.Add(tmpList[index]);
                tmpList.RemoveAt(index);
            }

            return newList;
        }
    }

    public static class CustomDistribution
    {
        public class Sampler<T>
        {
            private List<T> list = new List<T>();

            public Sampler(Dictionary<T, int> values)
            {
                foreach (var item in values)
                {
                    for (int i = 0; i < item.Value; i++)
                    {
                        list.Add(item.Key);
                    }
                }
            }

            public T Sample()
            {
                return LukRandom.Uniform.Sample(list);
            }
        }
    }
}