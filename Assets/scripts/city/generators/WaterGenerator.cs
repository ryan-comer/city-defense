using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour, IGenerator
{

    public GameObject Generate(CityConfig cityConfig)
    {
        return new GameObject();
    }

    public Building[] GetBuildings()
    {
        return new Building[] { };
    }
}
