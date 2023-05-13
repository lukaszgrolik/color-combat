using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDistributionSampler<T>
{
    private List<T> list = new List<T>();

    public CustomDistributionSampler(Dictionary<T, int> values)
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
        return list.Sample();
    }
}

public static class LukUtils
{
    public static int LayerMaskToLayer(LayerMask layerMask)
    {
        return (int)Mathf.Log(layerMask.value, 2);
    }
}