using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// CODE ADAPTED FROM https://youtu.be/-L-WgKMFuhE //

public class Grid : MonoBehaviour
{
    Node[,,] grid;
    public bool displayGridGizmos;
    public Vector3 gridSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;
    public TerrainType[] walkableRegions;
    LayerMask walkableMask;

    float nodeDiameter;
    int gridSizeX, gridSizeY, gridSizeZ;

    public void Update()
    {
        // Run Create Grid Function //

        CreateGrid();
    }

    void Awake()
    {
        // Get Grid Size //

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridSize.z / nodeDiameter);

        // Get Terrain Regions //

        foreach (TerrainType region in walkableRegions)
        {
            walkableMask.value = walkableMask | region.terrainMask.value;
        }

    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY * gridSizeZ;
        }
    }

    // Create grid based on x, y, z (set walkable and non-walkable nodes for each node) //

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];
        Vector3 bottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.up * gridSize.y / 2 - Vector3.forward * gridSize.z / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius) + Vector3.forward * (z * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                    int movementPenalty = 0;

                    if ((Physics.CheckSphere(worldPoint, nodeRadius, walkableRegions[0].terrainMask)))
                    {
                        movementPenalty = walkableRegions[0].terrainPenalty;
                    }

                    grid[x, y, z] = new Node(walkable, worldPoint, x, y, z, movementPenalty);
                }
            }
        }
    }

    // Get a node's neighbours (To later on compare and define the lowest cost) //

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && y == 0 && z == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    int checkZ = node.gridZ + z;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY && checkZ >= 0 && checkZ < gridSizeZ)
                    {
                        neighbours.Add(grid[checkX, checkY, checkZ]);
                    }
                }

            }
        }

        return neighbours;
    }

    // Set nodes of grid (all equally spaced) //

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridSize.x / 2) / gridSize.x;
        float percentY = (worldPosition.y + gridSize.y / 2) / gridSize.y;
        float percentZ = (worldPosition.z + gridSize.z / 2) / gridSize.z;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);

        return grid[x, y, z];
    }

    // Draw Gizmos depending on walkable or non walkable nodes //

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, gridSize.z));


        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.isWalkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }

}
