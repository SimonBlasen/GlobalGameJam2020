using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils<T>
{
    public static T[] Mix(T[] array)
    {
        List<T> elements = new List<T>();

        while (elements.Count < array.Length)
        {
            T el = array[Random.Range(0, array.Length - elements.Count)];
            elements.Add(el);
        }

        return elements.ToArray();
    }
}
