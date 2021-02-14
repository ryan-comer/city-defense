using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentialGenerator : MonoBehaviour, IGenerator
{

    public CityPrefabInfo[] buildings;  // The buildings that can be used in the residential generator
    public CityPrefabInfo[] props;  // Props that can be placed in-between buildings

    public Material groundMaterial; // Material to fill the ground

    public GameObject Generate(CityConfig cityConfig)
    {
        // Block object
        GameObject newBlock = new GameObject("block");

        // Starting point for the buildings
        float x = 0;
        float y = 0;

        // Loop and select random buildings
        while(x < cityConfig.blockSize)
        {
            float maxX = 0; // Used to see how much to move up
            float xLeft = cityConfig.blockSize - x;
            while(y < cityConfig.blockSize)
            {
                float yLeft = cityConfig.blockSize - y;

                // Get the block object
                Vector3 dimensions;
                CityPrefabInfo blockObjectInfo = CityGenerationUtils.GetRandomBlockObjectWithMaxSize(xLeft, yLeft, buildings, out dimensions);

                // Nothing could fit
                if(blockObjectInfo.prefab == null)
                {
                    break;
                }

                // Instantiate and place the block object
                GameObject blockObject = Instantiate(blockObjectInfo.prefab, newBlock.transform);
                blockObject.transform.localPosition = new Vector3(x + (dimensions.x / 2), 0, y + (dimensions.z / 2));

                // Update x and y for the building
                maxX = Mathf.Max(maxX, dimensions.x);
                y += dimensions.z;

                // Update x and y for spacing
                y += cityConfig.blockBuildingSpacing;
            }

            y = 0;  // Reset for next row

            // Update X for next
            x += maxX;
            x += cityConfig.blockBuildingSpacing;
        }

        // Create the ground
        GameObject groundObject = createGroundObject(cityConfig);
        groundObject.transform.parent = newBlock.transform;
        groundObject.transform.localPosition = new Vector3
        {
            x = cityConfig.blockSize / 2,
            y = -0.5f,
            z = cityConfig.blockSize / 2
        };

        return newBlock;
    }

    // Create the ground object to fill the bottom
    private GameObject createGroundObject(CityConfig cityConfig)
    {
        GameObject groundObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        groundObject.transform.localScale = new Vector3
        {
            x = cityConfig.blockSize,
            y = 1,
            z = cityConfig.blockSize
        };
        groundObject.GetComponent<MeshRenderer>().material = groundMaterial;

        return groundObject;
    }

}
