using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Config to generate the city
public struct CityConfig
{
    public Vector3 cityStart;   // Starting position for the city
    public int numBlocks;   // How many blocks should the city have
    public float blockSize; // How big is the base block size
    public float blockBuildingSpacing;  // Spacing between buildings in the blocks
    public float roadWidth; // The width of the roads in the city
    public CityType cityType;   // The type of the city (Modern, Medeival, etc.)
}

// Type of the city - this will effect the buildings that are chosen
public enum CityType
{
    MODERN,
    MEDIEVAL,
    SCIFI
}

// The type that the block is - this will determine what objects are chosen
public enum BlockType
{
    RESIDENTIAL,
    OFFICE,
    PARK,
    WATER,
    SHOPPING
}

public class CityController : MonoBehaviour
{
    public OfficeGenerator officeGenerator;
    public ParkGenerator parkGenerator;
    public ResidentialGenerator residentialGenerator;
    public ShoppingGenerator shoppingGenerator;
    public WaterGenerator waterGenerator;
    public RoadGenerator roadGenerator;

    private GameObject[,] cityGrid;    // 2D array to represent the city (adjacent blocks)

    private CityConfig currentConfig;   // Config for this city generation

    // Get the limits of the city in world space
    public void GetWorldLimits(out float x1, out float x2, out float y1, out float y2)
    {
        x1 = y1 = 0;
        x2 = y2 = (currentConfig.numBlocks * currentConfig.blockSize) + ((currentConfig.numBlocks - 1) * currentConfig.roadWidth);
    }

    // Get all the buildings in the city
    public Building[] GetAllBuildings()
    {
        List<Building> buildings = new List<Building>();

        // Get buildings from all the generators
        foreach(Building building in officeGenerator.GetBuildings())
        {
            buildings.Add(building);
        }
        foreach(Building building in residentialGenerator.GetBuildings())
        {
            buildings.Add(building);
        }
        foreach(Building building in parkGenerator.GetBuildings())
        {
            buildings.Add(building);
        }
        foreach(Building building in waterGenerator.GetBuildings())
        {
            buildings.Add(building);
        }
        foreach(Building building in shoppingGenerator.GetBuildings())
        {
            buildings.Add(building);
        }

        return buildings.ToArray();
    }

    // Generate a city to do tower defense in
    public void GenerateCity(CityConfig cityConfig, Transform cityCenter)
    {
        currentConfig = cityConfig;
        initializeGrid(cityConfig);

        // Generate each block
        recursiveGenerateBlock(0, 0, cityConfig);

        // Generate roads
        generateRoads(cityConfig);
    }

    // ***************** GENERATE BLOCK ************************

    // Recursive function to generate blocks for the city
    private void recursiveGenerateBlock(int blockX, int blockY, CityConfig cityConfig)
    {
        // Base case - no more blocks
        int numRows = cityGrid.GetLength(0);
        if(blockX > numRows || blockY > numRows)
        {
            return;
        }
        
        // Base case - already did block
        if(cityGrid[blockX, blockY] != null)
        {
            return;
        }

        // Generate this block
        Building[] buildings;
        BlockType blockType = getNextBlockType();
        var block = generateBlock(blockType, cityConfig, out buildings);
        cityGrid[blockX, blockY] = block;

        block.transform.position = new Vector3(blockX * cityConfig.blockSize, 0, blockY * cityConfig.blockSize);

        // Generate adjacent blocks
        if(blockX - 1 >= 0)
        {
            recursiveGenerateBlock(blockX - 1, blockY, cityConfig);
        }
        if(blockX + 1 < numRows)
        {
            recursiveGenerateBlock(blockX + 1, blockY, cityConfig);
        }
        if(blockY + 1 < numRows)
        {
            recursiveGenerateBlock(blockX, blockY + 1, cityConfig);
        }
        if(blockY - 1 >= 0)
        {
            recursiveGenerateBlock(blockX, blockY - 1, cityConfig);
        }

    }

    private GameObject generateBlock(BlockType blockType, CityConfig cityConfig, out Building[] buildings)
    {
        IGenerator generator = null;
        switch (blockType)
        {
            case BlockType.OFFICE:
                generator = officeGenerator;                         
                break;
            case BlockType.PARK:
                generator = parkGenerator;
                break;
            case BlockType.RESIDENTIAL:
                generator = residentialGenerator;
                break;
            case BlockType.SHOPPING:
                generator = shoppingGenerator;
                break;
            case BlockType.WATER:
                generator = waterGenerator;
                break;
        }

        GameObject block = generator.Generate(cityConfig);
        buildings = generator.GetBuildings();

        return block;
    }

    private BlockType getNextBlockType()
    {
        return BlockType.RESIDENTIAL;
        var values = System.Enum.GetValues(typeof(BlockType));
        return (BlockType) values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    // Generate the roads 
    private void generateRoads(CityConfig cityConfig)
    {
        var roadsObject = roadGenerator.Generate(cityConfig);

        // Space out all the blocks
        for(int i = 0; i < cityConfig.numBlocks; i++)
        {
            for(int j = 0; j < cityConfig.numBlocks; j++)
            {
                float xOffset = i * cityConfig.roadWidth;
                float zOffset = j * cityConfig.roadWidth;
                var block = cityGrid[i, j];
                block.transform.localPosition = new Vector3
                {
                    x = block.transform.localPosition.x + xOffset,
                    y = block.transform.localPosition.y,
                    z = block.transform.localPosition.z + zOffset
                };
            }
        }
    }

    // Initialize the grid object for generation
    private void initializeGrid(CityConfig cityConfig)
    {
        cityGrid = new GameObject[cityConfig.numBlocks, cityConfig.numBlocks];
    }

}
