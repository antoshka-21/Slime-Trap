using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class RandomExtensions
{
    public static int ElementFromArray(params int[] array)
    {
        int element = array[Random.Range(0, array.Length)];
        return element;
    }

    public static float ElementFromArray(params float[] array)
    {
        float element = array[Random.Range(0, array.Length)];
        return element;
    }
}
