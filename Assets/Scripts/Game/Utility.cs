using UnityEngine;

public static class Utility
{
    public static void AddChildsToArray<T>(out T[] array, string name, Transform transform)
    {
        array = new T[transform.FindChild(name).childCount];
        for (int i = 0; i < transform.FindChild(name).childCount; i++)
        {
            array[i] = transform.FindChild(name).GetChild(i).GetComponent<T>();
        }
    }
}