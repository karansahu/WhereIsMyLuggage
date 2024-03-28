using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class DrawPathOnTilemap : MonoBehaviour
{

    private bool isDragging = false;
    [SerializeField] private Tilemap tilemap;

    public Tile pathTile;
    public Tile backgroundTile;
    public TileBase pathRuleTile;

    
    public Text mousePosInGridText;
    public Text gridCellText;
    public float cellSelectOffset = 0.2f;

    private GridGenerator gridGenerator;
    Node starterNode;

    private void Start()
    {
        GameObject grid = GameObject.FindWithTag("GridGenerator");
        if (grid != null)
        {
            gridGenerator = grid.GetComponent<GridGenerator>();
            if(gridGenerator != null )
            {
                Debug.Log("Successfully found GridGenerator");
            }
        }
        
    }
    void Update()
    {
        CheckMouseInput();        
        //gridCellText.text = (Camera.main.ScreenToWorldPoint(Input.mousePosition)).ToString();
    }

    void CheckMouseInput()
    {   
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            Vector3Int cellPosition = Vector3Int.FloorToInt(mousePos);

            if (IsMouseWithinCellOffset(mousePos, cellPosition))
            {                
                mousePosInGridText.text = "Mouse in cell offset: " + cellPosition.ToString();
                starterNode = gridGenerator.GetNodeAtGridPosition(cellPosition.x, cellPosition.y);

                if (tilemap.GetTile(cellPosition) == backgroundTile)
                {                                      
                    tilemap.SetTile (cellPosition, pathTile);
                    
                    Debug.Log("Mouse Down");
                }
                else if(tilemap.GetTile(cellPosition) == pathTile)
                {                    
                    Debug.Log("Mouse Down at Path Tile");
                }        
                isDragging = true;
            }
            //else
            //{
            //    mousePosInGridText.text = "Mouse OUT cell offset: " + cellPosition.ToString();
            //}
            gridCellText.text = tilemap.GetTile(cellPosition).ToString();
        }
        
        else if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            isDragging = false;
            starterNode = null;
        }

        if (isDragging)
        {
            Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            Vector3Int cellPosition = Vector3Int.FloorToInt(mousePos);
            
            if (IsMouseWithinCellOffset(mousePos, cellPosition))
            {
                TileBase tileAtMousePosition = tilemap.GetTile(tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));

                if (tileAtMousePosition == backgroundTile)
                {
                    tilemap.SetTile(tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)), pathTile);                    
                }
                             
                Node currentNode = gridGenerator.GetNodeAtGridPosition(cellPosition.x, cellPosition.y);

                if(currentNode != null)
                {
                    if (currentNode == starterNode)
                    {
                        //mouse is at the same node. do not add anything
                        Debug.Log("Same node");
                    }
                    else
                    {
                        if (!starterNode.neighborNodes.Contains(currentNode))
                        {
                            //startnode does not contain current node
                            starterNode.AddNeighbor(currentNode);
                            Debug.Log("Adding " + currentNode + " to " + starterNode);
                        }

                        if (!currentNode.neighborNodes.Contains(starterNode))
                        {
                            currentNode.AddNeighbor(starterNode);
                            Debug.Log("Adding " + starterNode + " to " + currentNode);
                        }

                        starterNode = currentNode;
                    }
                }
            }       

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        
            //Debug.DrawRay(ray.origin, ray.direction * 10f, Color.blue, 2f);
        }
    }

    bool IsMouseWithinCellOffset(Vector2 mousePos, Vector3Int cellPosition)
    {
        return (mousePos.x > (cellPosition.x + cellSelectOffset) &&
                mousePos.x < ((cellPosition.x + 1) - cellSelectOffset) &&
                mousePos.y > (cellPosition.y + cellSelectOffset) &&
                mousePos.y < ((cellPosition.y + 1) - cellSelectOffset));   
    }
}