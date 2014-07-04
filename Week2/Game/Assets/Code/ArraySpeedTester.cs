using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArraySpeedTester : MonoBehaviour
{
    // http://wiki.unity3d.com/index.php?title=Which_Kind_Of_Array_Or_Collection_Should_I_Use?

    int[] arr;
    ArrayList arrList;
    List<int> list;
    
    const int size = 10000000;
    float start;

    // Use this for initialization
    void Start()
    {
        start = Time.realtimeSinceStartup;


        print("---Filling---");

        // standard array
        arr = new int[size];
        FilleArray();
        print("Array: " + (Time.realtimeSinceStartup - start).ToString("f6"));

        // array list
        arrList = new ArrayList(size);
        FilleArrayList();
        print("ArrayList: " + (Time.realtimeSinceStartup - start).ToString("f6"));

        // list
        list = new List<int>(size);
        FillList();
        print("List: " + (Time.realtimeSinceStartup - start).ToString("f6"));

        print("---Retrieving---");

        ReadArray();
        print("Array: " + (Time.realtimeSinceStartup - start).ToString("f6"));

        ReadArrayList();
        print("ArrayList: " + (Time.realtimeSinceStartup - start).ToString("f6"));

        ReadList();
        print("List: " + (Time.realtimeSinceStartup - start).ToString("f6"));



    }

    void FilleArray()
    {
        for (int i = 0; i < size; i++)
        {
            arr[i] = i;
        }
    }

    void ReadArray()
    {
        int sum = 0;
        for (int i = 0; i < size; i++)
        {
            sum += arr[i];
        }
    }

    void FilleArrayList()
    {
        for (int i = 0; i < size; i++)
        {
            arrList.Add(i);
        }
    }

    void ReadArrayList()
    {
        int sum = 0;
        for (int i = 0; i < size; i++)
        {
            sum += (int)arrList[i];
        }
    }

    void FillList()
    {
        for (int i = 0; i < size; i++)
        {
            list.Add(i);
        }
    }

    void ReadList()
    {
        int sum = 0;
        for (int i = 0; i < size; i++)
        {
            sum += list[i];
        }
    }
}