using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenerator
{
    GameObject Generate(CityConfig cityConfig);
    Building[] GetBuildings();
}
