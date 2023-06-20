using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LukUtils
{
    public static int LayerMaskToLayer(LayerMask layerMask)
    {
        return (int)Mathf.Log(layerMask.value, 2);
    }
}

// @todo add abstract type checking
public class TypeMap : Dictionary<System.Type, System.Type>
{
    public TypeMap()
    {

    }

    public T Instantiate<T>(object obj, params object[] args)
    where T : class
    {
        var type = obj.GetType();

        foreach (var item in this)
        {
            if (type == item.Key)
            {
                // this.skillActivation = new item.Value;
                return System.Activator.CreateInstance(
                    item.Value,
                    args
                ) as T;
            }
        }

        throw new System.Exception($"Unhandled type {type}");
    }
}