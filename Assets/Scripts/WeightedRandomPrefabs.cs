﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exception = System.Exception;
using Serializable = System.SerializableAttribute;

[CreateAssetMenu]
public class WeightedRandomPrefabs : ScriptableObject
{
    [Serializable]
    public class Item
    {
        public GameObject prefab = null;
        public float weight = 1;
    }

    [SerializeField] Item[] items = null;

    private float weightSum;

    private void Awake() => RecalculateSum();
    private void OnValidate() => RecalculateSum();

    private void RecalculateSum()
    {
        weightSum = 0;
        foreach(Item item in items)
        {
            weightSum += item.weight;
        }
    }

    public GameObject GetRandom()
    {
        /*
            Напишите взвешенный рандом
        */
        throw new System.NotImplementedException("Здесь должен быть взвешенный рандом");
    }
}
