using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Game.Pathfinding;

public class TileMapPathfinding : MonoBehaviour
{
    Vector3Int[] directions = new Vector3Int[8] {Vector3Int.left,Vector3Int.right,Vector3Int.up,Vector3Int.down, new(1,1,0), new(1,-1,0), new(-1,1,0), new(-1,-1,0) };

    public Tilemap tilemap;
    public TileAndMovementCost[] tiles;
    AstarPathFind<Vector3Int> pathfinder;

    [System.Serializable]
    public struct TileAndMovementCost
    {
        public Tile tile;
        public bool movable;
        public float movementCost;
    }

    public List<Vector3Int> path;

    [Range(0.001f,1f)]
    public float stepTime;
    public bool drawDebugLines = false;
    Vector3Int startingTilePosition;
    Vector3Int endTilePosition;

    public float DistanceFunc(Vector3Int a, Vector3Int b)
    {
        return (a-b).sqrMagnitude;
    }


    public Dictionary<Vector3Int,float> connectionsAndCosts(Vector3Int a)
    {
        Dictionary<Vector3Int, float> result= new Dictionary<Vector3Int, float>();
        
        foreach (Vector3Int dir in directions)
        {
            foreach (TileAndMovementCost tmc in tiles)
            {
                if (tilemap.GetTile(a+dir)==tmc.tile)
                {
                    if (tmc.movable) result.Add(a + dir, tmc.movementCost);
                }
            }
                
        }
        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = new AstarPathFind<Vector3Int>(DistanceFunc, connectionsAndCosts);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        pathfinder.GenerateAstarPath(startingTilePosition, endTilePosition, out path);
    //        StopAllCoroutines();
    //        StartCoroutine(MoveForward());
    //    }
    //}

    public void SetStartingAndEndTiles(Vector3Int startingPosition, Vector3Int endPosition)
    {
        startingTilePosition = startingPosition;
        endTilePosition = endPosition;
        transform.position = startingPosition;
    }

    IEnumerator MoveBackward()
    {
        int pathCount = path.Count - 1;

        while (pathCount >= 0)
        {
            transform.position = tilemap.CellToWorld(path[pathCount]);
            if (drawDebugLines)
            {
                for (int i = 0; i < path.Count - 1; i++) //visualize your path in the sceneview
                {
                    Debug.DrawLine(new Vector3(path[i].x + 0.5f, path[i].y + 0.5f, path[i].z), new Vector3(path[i + 1].x + 0.5f, path[i + 1].y + 0.5f, path[i + 1].z), Color.black, 2.0f);
                }
            }
            pathCount--;
            yield return new WaitForSeconds(stepTime);
        }

        StartCoroutine(MoveForward());
    }

    IEnumerator MoveForward()
    {
        int pathCount = 0;

        while (pathCount < path.Count)
        {
            transform.position = tilemap.CellToWorld(path[pathCount]);
            if (drawDebugLines)
            {
                for (int i = 0; i < path.Count - 1; i++) //visualize your path in the sceneview
                {
                    Debug.DrawLine(new Vector3(path[i].x + 0.5f, path[i].y + 0.5f, path[i].z), new Vector3(path[i + 1].x + 0.5f, path[i + 1].y + 0.5f, path[i + 1].z), Color.white, 2.0f);
                }
            }
            pathCount++;
            yield return new WaitForSeconds(stepTime);
        }

        StartCoroutine(MoveBackward());
    }
}