using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Test.Pathfind;


public class DestinationGenerator : MonoBehaviour
{
    public Tilemap tilemap;

    public Tile startingTile;
    public Tile endTile;
    Vector3Int[] startingTilePosition;
    Vector3Int[] endTilePosition;

    public int numOfRoutes;
    public GameObject[] objectToMove;

    AStarPathfinding pathfind;

    GridGenerator grid;

    void Start()
    {
        //pathfind = GameObject.child
        grid = GameObject.FindWithTag("GridGenerator").GetComponent<GridGenerator>();

        startingTilePosition = new Vector3Int[numOfRoutes];
        endTilePosition = new Vector3Int[numOfRoutes];

        while (objectToMove[0].GetComponent<TileMapPathfinding>() == null)
        {
            Debug.Log("GetComponent is null");
        }
        for (int i = 0; i < numOfRoutes; i++)
        {
            startingTilePosition[i] = new Vector3Int(Random.Range(0, 2), Random.Range(0, 8), 0);
            endTilePosition[i] = new Vector3Int(Random.Range(15, 16), Random.Range(0, 8), 0);
            tilemap.SetTile(startingTilePosition[i], startingTile);
            tilemap.SetTile(endTilePosition[i], endTile);
            if(objectToMove[i].GetComponent<TileMapPathfinding>() == null)
            {
                Debug.Log(objectToMove[i] + " null");
            }
            objectToMove[i].GetComponent<TileMapPathfinding>().SetStartingAndEndTiles(startingTilePosition[i], endTilePosition[i]);

        }

        

        //StartCoroutine(MoveObjects());
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Right Click - Find Path");

            List<Node> path = pathfind.FindPath(grid.GetNodeAtGridPosition(startingTilePosition[0].x, startingTilePosition[0].y), grid.GetNodeAtGridPosition(endTilePosition[0].x, endTilePosition[0].y));
        }
    }

    IEnumerator MoveObjects()
    {
        yield return new WaitForSeconds(0.5f);

        while (objectToMove[0] == null)
        {
            Debug.Log("Object [0] is null");
        }

        while (objectToMove[0].GetComponent<TileMapPathfinding>() == null)
        {
            Debug.Log("GetComponent is null");
        }

        objectToMove[0].GetComponent<TileMapPathfinding>().SetStartingAndEndTiles(startingTilePosition[0], endTilePosition[0]);
        objectToMove[1].GetComponent<TileMapPathfinding>().SetStartingAndEndTiles(startingTilePosition[1], endTilePosition[1]); 
    }
}
