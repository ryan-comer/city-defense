using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject player_p; // Prefab for the player
    public PlayerCamera playerCamera; // Camera for the player
    public Transform playerSpawnPoint;  // Spawn point for the player

    private CityGenerator cityGenerator;

    // Start is called before the first frame update
    void Start()
    {
        cityGenerator = GetComponent<CityGenerator>();
        Debug.Assert(cityGenerator);

        // City config for making the city
        CityConfig cityConfig = new CityConfig
        {
            blockSize = 150,
            cityType = CityType.MODERN,
            numBlocks = 4,
            roadWidth = 10,
            blockBuildingSpacing = 5,
            cityStart = Vector3.zero
        };

        // Make the city
        cityGenerator.GenerateCity(cityConfig, transform);

        // Spawn the player
        spawnPlayer();
    }

    private void spawnPlayer()
    {
        GameObject playerObject = Instantiate(player_p);
        playerObject.transform.position = playerSpawnPoint.position;

        playerCamera.SetFollowTarget(playerObject);
        playerCamera.SetLookAtTarget(playerObject);
    }

}
