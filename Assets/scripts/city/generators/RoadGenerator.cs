using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour, IGenerator
{

    private List<Transform> intersections = new List<Transform>();  // Intersections between blocks

    // Generate the roads and return
    public GameObject Generate(CityConfig cityConfig)
    {
        GameObject roadsObjects = new GameObject("roads");
        roadsObjects.transform.position = cityConfig.cityStart;

        for(var x = 0; x < cityConfig.numBlocks; x++)
        {
            for(var y = 0; y < cityConfig.numBlocks; y++)
            {
                // Skip last one
                if(x < cityConfig.numBlocks - 1)
                {
                    // Right road
                    var rightRoad = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightRoad.transform.parent = roadsObjects.transform;
                    rightRoad.transform.localPosition = new Vector3
                    {
                        x = (x + 1) * cityConfig.blockSize + (cityConfig.roadWidth / 2) + (cityConfig.roadWidth * x),
                        y = cityConfig.cityStart.y - (0.5f),
                        z = (y * cityConfig.blockSize) + (cityConfig.blockSize / 2) + (cityConfig.roadWidth * y)
                    };
                    rightRoad.transform.localScale = new Vector3
                    {
                        x = cityConfig.roadWidth,
                        y = 1,
                        z = cityConfig.blockSize
                    };
                    rightRoad.layer = LayerMask.NameToLayer("ground");
                }

                // Skip last one
                if(y < cityConfig.numBlocks - 1)
                {
                    // Top road
                    var topRoad = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    topRoad.transform.parent = roadsObjects.transform;
                    topRoad.transform.localPosition = new Vector3
                    {
                        x = (x * cityConfig.blockSize) + (cityConfig.blockSize / 2) + (cityConfig.roadWidth * x),
                        y = cityConfig.cityStart.y - (0.5f),
                        z = (y + 1) * cityConfig.blockSize + (cityConfig.roadWidth / 2) + (cityConfig.roadWidth * y)
                    };
                    topRoad.transform.localScale = new Vector3
                    {
                        x = cityConfig.blockSize,
                        y = 1,
                        z = cityConfig.roadWidth
                    };
                    topRoad.layer = LayerMask.NameToLayer("ground");
                }

                // Fill in hole
                if(x < cityConfig.numBlocks - 1 && y < cityConfig.numBlocks - 1)
                {
                    var holeFill = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    holeFill.transform.parent = roadsObjects.transform;
                    holeFill.transform.localPosition = new Vector3
                    {
                        x = (x+1) * cityConfig.blockSize + (cityConfig.roadWidth * x) + (cityConfig.roadWidth / 2),
                        y = cityConfig.cityStart.y - (0.5f),
                        z = (y+1) * cityConfig.blockSize + (cityConfig.roadWidth * y) + (cityConfig.roadWidth / 2)
                    };
                    holeFill.transform.localScale = new Vector3
                    {
                        x = cityConfig.roadWidth,
                        y = 1,
                        z = cityConfig.roadWidth
                    };
                    holeFill.layer = LayerMask.NameToLayer("ground");
                    intersections.Add(holeFill.transform);
                }
            }
        }

        return roadsObjects;
    }

    public List<Transform> GetIntersections()
    {
        return intersections;
    }

}
