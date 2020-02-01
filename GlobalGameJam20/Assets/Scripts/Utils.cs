using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils<T>
{
    public static T[] Mix(T[] array)
    {
        List<T> sourceElements = new List<T>();
        List<T> elements = new List<T>();

        sourceElements.AddRange(array);

        while (elements.Count < array.Length)
        {
            int elemntTaken = Random.Range(0, sourceElements.Count);
            T el = sourceElements[elemntTaken];
            elements.Add(el);
            sourceElements.RemoveAt(elemntTaken);
        }

        return elements.ToArray();
    }
}
