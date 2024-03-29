using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Luggage.Pathfind;
using System.Linq;
using System.IO;


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
        pathfind = transform.GetChild(0).GetComponent<AStarPathfinding>();
        grid = GameObject.FindWithTag("GridGenerator").GetComponent<GridGenerator>();

        startingTilePosition = new Vector3Int[numOfRoutes];
        endTilePosition = new Vector3Int[numOfRoutes];
        
        for (int i = 0; i < numOfRoutes; i++)
        {
            startingTilePosition[i] = new Vector3Int(Random.Range(0, 2), Random.Range(0, 8), 0);
            endTilePosition[i] = new Vector3Int(Random.Range(15, 16), Random.Range(0, 8), 0);
            tilemap.SetTile(startingTilePosition[i], startingTile);
            tilemap.SetTile(endTilePosition[i], endTile);
        }        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            
            StartCoroutine(DrawConnections());
            //NEED TO FIGURE OUT A WAY TO MAKE SURE MOUSE DOESN'T GO THROUGH THE CELL OFFSET AND CREATE CONNECTIONS THAT ARE NOT NEXT TO EACH OTHER
        }
    }

    IEnumerator MoveObjects()
    {
        yield return new WaitForSeconds(0.5f);

        while (objectToMove[0] == null)
        {
            Debug.Log("Object [0] is null");
        }       
    }

    IEnumerator DrawConnections()
    {
        Debug.Log("Right Click - Find Path");

        List<Node> path = pathfind.FindPath(grid.GetNodeAtGridPosition(startingTilePosition[0].x, startingTilePosition[0].y), grid.GetNodeAtGridPosition(endTilePosition[0].x, endTilePosition[0].y));

        for (int i = 0; i < path.Count - 1; i++) //visualize your path in the sceneview
        {
            Debug.DrawLine(new Vector3(path[i].gridPosition.x + 0.5f, path[i].gridPosition.y + 0.5f, -0.5f),
                           new Vector3(path[i + 1].gridPosition.x + 0.5f, path[i + 1].gridPosition.y + 0.5f, -0.5f), Color.white, 10.0f);
            Debug.Log("debug lines");
        }
        StartCoroutine(MoveObjects());
        StartCoroutine(MoveForward(path));

        yield return null;
    }

    IEnumerator MoveBackward(List<Node> path)
    {
        int pathCount = path.Count - 1;

        while (pathCount >= 0)
        {
            objectToMove[0].transform.position = new Vector3(path[pathCount].gridPosition.x, path[pathCount].gridPosition.y,0.0f) ;
            pathCount--;
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(MoveForward(path));
    }

    IEnumerator MoveForward(List<Node> path)
    {
        int pathCount = 0;

        while (pathCount < path.Count)
        {
            objectToMove[0].transform.position = new Vector3(path[pathCount].gridPosition.x, path[pathCount].gridPosition.y, 0.0f);
            pathCount++;
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(MoveBackward(path));
    }
}
