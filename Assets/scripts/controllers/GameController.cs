using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private CityGenerator cityGenerator;

    // Start is called before the first frame update
    void Start()
    {
        cityGenerator = GetComponent<CityGenerator>();
        Debug.Assert(cityGenerator);

        CityConfig cityConfig = new CityConfig
        {
            blockSize = 150,
            cityType = CityType.MODERN,
            numBlocks = 4,
            roadWidth = 10,
            blockBuildingSpacing = 5,
            cityStart = Vector3.zero
        };

        cityGenerator.GenerateCity(cityConfig, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
