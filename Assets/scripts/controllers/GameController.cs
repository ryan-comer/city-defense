using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;  // Singleton

    public GameObject player_p; // Prefab for the player
    public PlayerCamera playerCamera; // Camera for the player
    public Transform playerSpawnPoint;  // Spawn point for the player

    private CityGenerator cityGenerator;
    public MonsterController monsterController;

    private GameObject m_player;
    public GameObject Player
    {
        get
        {
            return m_player;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Confined;

        cityGenerator = GetComponent<CityGenerator>();
        Debug.Assert(cityGenerator);
        Debug.Assert(monsterController);

        // City config for making the city
        CityConfig cityConfig = new CityConfig
        {
            blockSize = 150,
            cityType = CityType.MODERN,
            numBlocks = 3,
            roadWidth = 10,
            blockBuildingSpacing = 5,
            cityStart = Vector3.zero
        };

        // Make the city
        cityGenerator.GenerateCity(cityConfig, transform);

        // Spawn the player
        spawnPlayer();

        // Start spawning monsters
        monsterController.Initialize(1, cityConfig);
        monsterController.StartSpawning();
    }

    private void spawnPlayer()
    {
        m_player = Instantiate(player_p);
        m_player.transform.position = playerSpawnPoint.position;

        playerCamera.SetFollowTarget(m_player);
        playerCamera.SetLookAtTarget(m_player);
    }

}
