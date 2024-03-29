using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2Int gridPosition { get; set; }
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private bool moveable;
    [SerializeField]
    private float movementCost;

    private bool drawDebugLines = true;

    public List<Node> neighborNodes; 

    public float gCost { get; set; } // Distance from starting node
    public float fCost { get; set; } // Total cost (gCost + heuristic)
    public Node parent { get; set; } // Parent node for path reconstruction

    //node type. maybe an enum. maybe incorporate movable and movementcost into the enum selection. select enum and it chooses the type and movement cost.

    //public Node(Vector2Int position, bool canMove)
    //{
    //    //gridPosition = position;
    //    //moveable = canMove;
    //    //neighborNodes = new List<Node>();
    //}

    public void Initialize(Vector2Int position, bool canMove)
    {
        gridPosition = position;
        moveable = canMove;
        this.name = "Node Type: " + gridPosition;
    }

    private void Start()
    {
        
    }

    public bool GetMoveable()
    {
        return moveable;
    }

    public void SetMoveable(bool canMove)
    {
        moveable = canMove;
    }

    public void AddNeighbor(Node neighbor)
    {
        neighborNodes.Add(neighbor);
        DrawConnections();
    }

    
    public void DrawConnections()
    {
        int pathCount = 0;
        List<Node> connectedNodes = neighborNodes.ToList();
        connectedNodes.Add(this);
        

        if (connectedNodes.Count > 1)
        {
            while (pathCount < connectedNodes.Count)
            {
                for (int i = 0; i < connectedNodes.Count - 1; i++) //visualize your path in the sceneview
                {
                    Debug.DrawLine(new Vector3(connectedNodes[i].gridPosition.x + 0.5f, connectedNodes[i].gridPosition.y + 0.5f, -0.5f),
                                   new Vector3(connectedNodes[i + 1].gridPosition.x + 0.5f, connectedNodes[i + 1].gridPosition.y + 0.5f, -0.5f), Color.white, 10.0f);
                    Debug.Log("debug lines");                    
                }
                pathCount++;
            }
        }
    }
}