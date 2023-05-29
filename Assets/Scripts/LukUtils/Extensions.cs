using UnityEngine;
using System.Collections.Generic;

public static class ArrayExtensions
{

}

public static class ListExtensions
{
    public static T NextOrFirst<T>(this List<T> list, T currentItem)
    {
        var index = list.IndexOf(currentItem);
        if (index == -1) return default(T);

        var nextIndex = index == list.Count - 1 ? 0 : index + 1;

        return list[nextIndex];
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