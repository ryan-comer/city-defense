using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CityPrefabInfo
{
    [SerializeField]
    public CityType cityType;

    [SerializeField]
    public GameObject prefab;

    [HideInInspector]
    public Vector3 dimensions;

    public float randomWeight;  // Weight for the random chance of picking this prefab
}
