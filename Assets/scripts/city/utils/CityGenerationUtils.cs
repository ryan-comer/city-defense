using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CityGenerationUtils
{

    // Find a random block object that fits in the dimensions
    public static CityPrefabInfo GetRandomBlockObjectWithMaxSize(float maxWidth, float maxHeight, CityPrefabInfo[] potentialObjects, out Vector3 dimensions)
    {
        List<CityPrefabInfo> objectsToChoose = new List<CityPrefabInfo>();

        BoxCollider buildingSpacingCollider = null;
        foreach(var blockObject in potentialObjects)
        {
            buildingSpacingCollider = blockObject.prefab.GetComponent<BoxCollider>();

            dimensions = buildingSpacingCollider.size;    // Multiply by 2 becaust extents is half
            if(dimensions.x < maxWidth && dimensions.z < maxHeight)
            {
                objectsToChoose.Add(blockObject);
            }
        }

        // None to return
        if(objectsToChoose.Count == 0)
        {
            dimensions = Vector3.zero;
            return new CityPrefabInfo();
        }

        // Return a random choice
        CityPrefabInfo returnObject = objectsToChoose[Random.Range(0, objectsToChoose.Count)];
        buildingSpacingCollider = returnObject.prefab.GetComponent<BoxCollider>();
        dimensions = buildingSpacingCollider.size;
        return returnObject;
    }

}
