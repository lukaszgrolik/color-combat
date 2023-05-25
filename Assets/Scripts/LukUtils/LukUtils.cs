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