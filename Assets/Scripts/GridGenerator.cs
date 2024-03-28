using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    //GRID
    [SerializeField]
    private float gridSize; // Size of each grid cell
    [SerializeField]
    private Transform nodePrefab; // Prefab of your grid node
    [SerializeField]
    private int gridSizeX = 5; // Number of nodes in X-axis
    [SerializeField]
    private int gridSizeY = 5; // Number of nodes in Y-axis
    private Node[,] gridNodes; // 2D array to hold grid nodes  

    private void Start()
    {        
        CreateGrid();        
    }

    void CreateGrid()
    {
        gridNodes = new Node[gridSizeX,gridSizeY];
        Transform[,] grid = new Transform[gridSizeX, gridSizeY];

        // Instantiate nodes and populate the grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2Int nodePosition = new Vector2Int(x, y);
                grid[x, y] = Instantiate(nodePrefab, new Vector3(nodePosition.x, nodePosition.y, 2.0f), Quaternion.identity, transform);
                grid[x, y].GetComponent<Node>().Initialize(nodePosition, false);

                Node spawnedNode = grid[x,y].GetComponent<Node>();
                if (spawnedNode != null)    
                {
                    gridNodes[x, y] = spawnedNode;
                }
                else
                {
                    Debug.Log("Prefab does not have Node component");
                }

                
            }
        }
    }

    public float GetGridSize()
    {
        return gridSize;
    }

    public Node[,] GetNodes()
    {
        return gridNodes;
    }

    public Node GetNodeAtGridPosition(int gridPositionX, int gridPositionY)
    {
        if (gridPositionX < gridSizeX && gridPositionY < gridSizeY)
        {
            return gridNodes[gridPositionX, gridPositionY];
        }
        else
            return null;
    }    

     
}
