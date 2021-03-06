﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;  // Singleton

    public GameObject player_p; // Prefab for the player
    public PlayerCamera playerCamera; // Camera for the player
    public Transform playerSpawnPoint;  // Spawn point for the player
    public HealthBar playerHealthBar;   // Health bar for the player

    public CityController cityController;
    public MonsterController monsterController;
    public UiController uiController;
    public MinimapController minimapController;

    public GameObject Player
    {
        get
        {
            return m_player;
        }
    }

    private GameObject m_player;
    private int startingNumberOfBuildings = 0;  // The number of buildings at the start of the game - used to determine city health
    private int currentNumberOfBuildings = 0;   // The number of currently alive buildings - used to determine city health

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        Debug.Assert(cityController);
        Debug.Assert(monsterController);
        Debug.Assert(uiController);
        Debug.Assert(minimapController);

        // City config for making the city
        CityConfig cityConfig = new CityConfig
        {
            blockSize = 150,
            cityType = CityType.MODERN,
            numBlocks = 4,
            roadWidth = 20,
            blockBuildingSpacing = 5,
            cityStart = Vector3.zero
        };

        // Make the city
        cityController.GenerateCity(cityConfig, transform);
        startingNumberOfBuildings = cityController.GetAllBuildings().Length;
        currentNumberOfBuildings = startingNumberOfBuildings;

        // Spawn the player
        spawnPlayer();

        // Start spawning monsters
        monsterController.Initialize(1, cityConfig);
        monsterController.StartSpawning();

        // Set up UI
        uiController.SetCityHealthThresholdMarker(0.5f);
        float x1, x2, y1, y2;
        cityController.GetWorldLimits(out x1, out x2, out y1, out y2);
        minimapController.SetWorldLimits(x1, x2, y1, y2);

        // Subscribe to events
        foreach(Building building in cityController.GetAllBuildings())
        {
            Combat combat = building.GetComponent<Combat>();
            combat.OnDeath += buildingDestroyed;
        }
        monsterController.OnWavesComplete += () =>
        {
            Debug.Log("You Won!");
        };
        monsterController.OnMonsterSpawn += (monster) =>
        {
            minimapController.AddTrackedObject(monster.gameObject, "monster");
        };
        monsterController.OnMonsterDied += (monster) =>
        {
            minimapController.RemoveTrackedObject(monster.gameObject);
        };
    }

    // Callback for when a building is destroyed
    private void buildingDestroyed(GameObject building)
    {
        currentNumberOfBuildings -= 1;
        float buildingsAlivePercentage = (float)currentNumberOfBuildings / (float)startingNumberOfBuildings;
        uiController.SetCityHealthBar(buildingsAlivePercentage);
    }

    private void spawnPlayer()
    {
        m_player = Instantiate(player_p);
        m_player.transform.position = playerSpawnPoint.position;

        playerCamera.SetFollowTarget(m_player);
        playerCamera.SetLookAtTarget(m_player);

        m_player.GetComponent<Combat>().OnDeath += playerDied;
        m_player.GetComponent<Combat>().healthBar = playerHealthBar;

        minimapController.AddTrackedObject(m_player, "player");
    }

    // Callback for when the player dies
    private void playerDied(GameObject player)
    {
        minimapController.RemoveTrackedObject(player);
        Debug.Log("Game Over!");
    }

}
