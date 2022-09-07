using UnityEngine;
using System.Collections.Generic;

public static class ArrayExtensions
{
    public static T Sample<T>(this T[] arr)
    {
        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }
}

public static class ListExtensions
{
    public static T Sample<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T Sample<T>(this IReadOnlyList<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    // http://answers.unity.com/answers/1566405/view.html
    public static List<T> RandomMany<T>(this List<T> list, int number)
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

public static class Vector3Extensions
{
    public static Vector3 With(this Vector3 origin, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? origin.x, y ?? origin.y, z ?? origin.z);
    }
}

public static class Vector2Extensions
{
    public static Vector3 ToVector3(this Vector2 origin)
    {
        return new Vector3(origin.x, 0, origin.y);
    }
}

public static class Vector2IntExtensions
{
    public static Vector3Int ToVector3(this Vector2Int origin)
    {
        return new Vector3Int(origin.x, 0, origin.y);
    }
}

public static class TransformExtensions
{
    public static void DestroyChildren(this Transform transform)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public static void DestroyChildrenImmediate(this Transform transform)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}