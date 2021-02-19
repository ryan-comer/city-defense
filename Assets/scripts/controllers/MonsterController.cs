using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to see which monsters can be spawned by threat
[System.Serializable]
public struct SpawnableMonstersInfo
{
    public MonsterThreat threat;    // Threat level of the monsters
    public Monster[] monsters_p;    // Prefabs for the monsters
}

// Group of monsters in a wave (same threat)
[System.Serializable]
public struct MonsterSpawnInfo
{
    public MonsterThreat threat;// the threat level
    public int count;   // Number to spawn
}

// Wave definition
[System.Serializable]
public struct WaveInfo
{
    public MonsterSpawnInfo[] monsterThreatsToSpawn;
}

public class MonsterController : MonoBehaviour
{

    public RoadGenerator roadGenerator; // Used to get the intersections
    public WaveInfo[] waves;     // Used to know which monsters to spawn for each wave
    public SpawnableMonstersInfo[] spawnableMonsters; // Which monsters can be spawned
    public float timeBetweenWaves = 10.0f;  // How long before spawning the next wave
    public float spawnLocationRandomness = 5.0f;    // How much to spread the monsters out when spawning on a spot

    private MonsterThreat currentThreat;    // The current threat for monsters that are spawning
    private int currentWaveNumber = 0;    // The wave that we're on
    private bool shouldSpawn = false;   // Should the spawner spawn monsters
    private HashSet<Monster> activeMonsters = new HashSet<Monster>();   // List of monsters for the current wave

    private int numPlayers; // Number of players - used for difficulty scaling
    private List<Vector3> spawnLocations = new List<Vector3>(); // The locations that monsters can spawn from

    private bool spawnPending = false;  // Is there a pending spawn - used to avoid duplicate spawns

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(roadGenerator);
    }

    // Update is called once per frame
    void Update()
    {
        checkNextWave();
    }

    // Initialize the monster spawner
    public void Initialize(int numPlayers, CityConfig cityConfig)
    {
        this.numPlayers = numPlayers;
        initializeSpawnLocations();
    }

    // Start the monster spawner
    public void StartSpawning()
    {
        shouldSpawn = true;
    }

    // Stop the monster spawner
    public void StopSpawning()
    {

    }

    // Check if the next wave should spawn
    private void checkNextWave()
    {
        // See if the monsters are dead
        if(activeMonsters.Count == 0 && !spawnPending)
        {
            // Spawn the next wave
            StartCoroutine(spawnWaitCoroutine());
            spawnPending = true;
        }
    }

    // Used to spawn the wave after a delay
    private IEnumerator spawnWaitCoroutine()
    {
        yield return new WaitForSeconds(timeBetweenWaves);  // Delay for spawn
        spawnWave();
        currentWaveNumber += 1;
        spawnPending = false;
    }

    // Spawn the next wave of monsters
    private void spawnWave()
    {
        // Get the monsters you want to spawn
        WaveInfo waveInfo = waves[currentWaveNumber];
        Vector3 spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Count)];
        foreach (MonsterSpawnInfo monstersToSpawn in waveInfo.monsterThreatsToSpawn)
        {
            // Spawn all the monsters for that threat level
            for(var i = 0; i < monstersToSpawn.count; i++)
            {
                Monster monsterToSpawn = getMonsterToSpawn(monstersToSpawn.threat);
                spawnMonster(monsterToSpawn, spawnLocation);
            }
        }
    }

    // Get a monster to spawn of a certain threat
    private Monster getMonsterToSpawn(MonsterThreat threat)
    {
        // Find the set of monsters by threat
        SpawnableMonstersInfo possibleMonsters;
        possibleMonsters.monsters_p = null;
        foreach(SpawnableMonstersInfo potentialMonsters in spawnableMonsters)
        {
            if(potentialMonsters.threat == threat)
            {
                possibleMonsters = potentialMonsters;
                break;
            }
        }

        return possibleMonsters.monsters_p[Random.Range(0, possibleMonsters.monsters_p.Length)];
    }

    // Spawn a single monster
    private void spawnMonster(Monster monster, Vector3 spawnLocation)
    {
        // Get the real spawn location (add randomness)
        Vector3 spawnLocationRandom = new Vector3(
            Random.Range(spawnLocation.x - spawnLocationRandomness, spawnLocation.x + spawnLocationRandomness),
            spawnLocation.y,
            Random.Range(spawnLocation.z - spawnLocationRandomness, spawnLocation.z + spawnLocationRandomness)
        );

        Monster newMonster = Instantiate(monster);
        newMonster.transform.position = spawnLocationRandom;
        newMonster.transform.position = new Vector3
        (
            newMonster.transform.position.x,
            newMonster.transform.position.y + 1.0f,
            newMonster.transform.position.z
        );

        Combat combat = newMonster.GetComponent<Combat>();
        combat.OnDeath += monsterDied;

        activeMonsters.Add(newMonster); // Add to tracked monsters
    }

    // Callback for when a monster dies
    private void monsterDied(GameObject deadMonster)
    {
        Monster monster = deadMonster.GetComponent<Monster>();
        activeMonsters.Remove(monster);
    }

    // Find all the spawn locations based on the city config
    private void initializeSpawnLocations()
    {
        Transform[] intersections = roadGenerator.GetIntersections().ToArray();
        foreach(Transform intersection in intersections)
        {
            spawnLocations.Add(intersection.position);
        }
    }

}
