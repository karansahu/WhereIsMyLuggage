using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


public class MouseDirectionTest: MonoBehaviour
{
    public enum SnapDirection { Up, Down, Left, Right, UpRight, UpLeft, DownRight, DownLeft };
    private Dictionary<SnapDirection, Vector2> snapVectors = new Dictionary<SnapDirection, Vector2>();
    private SnapDirection snapDir; // Current snap direction
    [SerializeField]
    private float pathDrawTolerance;

    //MOUSE VISUALIZER
    [SerializeField]
    private SpriteRenderer snapSprite; // Reference to the sprite renderer for snap visuals    

    //DEBUG
    public Text text1;
    public Text text2;
    private Vector3 initialMousePosWorldPoint;
    LineRenderer lineRenderer;

    private float gridSize;   

    private void Start()
    {
        GameObject grid = GameObject.FindWithTag("GridGenerator");
        gridSize = grid.GetComponent<GridGenerator>().GetGridSize();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnMouseDown()
    {
        Vector3 initialMousePos = Input.mousePosition;
        initialMousePosWorldPoint = Camera.main.ScreenToWorldPoint(initialMousePos);

        snapSprite.gameObject.transform.position = SnapToGrid(initialMousePosWorldPoint);
        snapSprite.gameObject.SetActive(true); // Show snap indicator        

        lineRenderer.SetPosition(0, new Vector3(initialMousePosWorldPoint.x, initialMousePosWorldPoint.y, 0.0f));
    }

    private void OnMouseDrag()
    {
        Vector3 currentMousePosWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distanceBetweenMousePositions = Vector3.Distance(currentMousePosWorldPoint, initialMousePosWorldPoint);             

        if (distanceBetweenMousePositions > pathDrawTolerance)
        {
            //ADD PATH NODE
            lineRenderer.SetPosition(1, new Vector3(currentMousePosWorldPoint.x, currentMousePosWorldPoint.y, 0.0f));
            float angle = GetSignedAngleBetweenMousePositionVectors(initialMousePosWorldPoint, currentMousePosWorldPoint);

            snapSprite.gameObject.transform.rotation = Quaternion.AngleAxis(GetSnapDirection(angle), Vector3.forward);            

        }
        
    }

    private void OnMouseUp()
    {
        //snapSprite.gameObject.SetActive(false); // Hide snap indicator
    }

    //Get angle between the (vector parelle to the X-axis from the initial mouse position) and (the current vector position of the mouse)
    private float GetSignedAngleBetweenMousePositionVectors(Vector2 _initialMousePosWorldPoint, Vector2 _currentMousePosWorldPoint)
    {
        return Vector2.SignedAngle(new Vector2(_initialMousePosWorldPoint.x, 0.0f),
                                   new Vector2(_currentMousePosWorldPoint.x, _currentMousePosWorldPoint.y) - new Vector2(_initialMousePosWorldPoint.x, _initialMousePosWorldPoint.y));
    }


    private float GetSnapDirection(float _angleBetweenMousePositions)
    {
        float snapAngle = 0.0f;
        
        if (_angleBetweenMousePositions < 22.5f && _angleBetweenMousePositions > -22.5f)
        {
            // RIGHT
            text2.text = ("RIGHT");
            snapAngle = 0.0f;
        }
        else if (_angleBetweenMousePositions < 75f && _angleBetweenMousePositions > 22.5f)
        {
            // UP RIGHT
            text2.text = ("UP RIGHT");
            snapAngle = 45.0f;
        }
        else if (_angleBetweenMousePositions < 115f && _angleBetweenMousePositions > 75f)
        {
            // UP
            text2.text = ("UP");
            snapAngle = 90.0f;
        }
        else if (_angleBetweenMousePositions < 165f && _angleBetweenMousePositions > 115f)
        {
            // UP LEFT
            text2.text = ("UP LEFT");
            snapAngle = 135.0f;
        }
        else if (_angleBetweenMousePositions < -165f || _angleBetweenMousePositions > 165f)
        {
            // LEFT
            text2.text = ("LEFT");
            snapAngle = 180.0f;
        }
        else if (_angleBetweenMousePositions < -115f && _angleBetweenMousePositions > -165f)
        {
            // DOWN LEFT
            text2.text = ("DOWN LEFT");
            snapAngle = -135.0f;
        }
        else if (_angleBetweenMousePositions < -75f && _angleBetweenMousePositions > -115f)
        {
            // DOWN
            text2.text = ("DOWN");
            snapAngle = -90.0f;
        }
        else if (_angleBetweenMousePositions < -22.5f && _angleBetweenMousePositions > -75f)
        {
            // DOWN RIGHT
            text2.text = ("DOWN RIGHT");
            snapAngle = -45.0f;
        }
        else
        {
            text2.text = ("NO ANGLE");
        }

        return snapAngle;
    }

    private Vector3 SnapToGrid(Vector3 worldPos) 
    {
        float x = Mathf.Floor(worldPos.x); //Mathf.Round(worldPos.x / gridSize) * gridSize;
        float y = Mathf.Floor(worldPos.y); //Mathf.Round(worldPos.y / gridSize) * gridSize;

        Vector3 pos = new Vector3(x + gridSize/2, y + gridSize/2, transform.position.z);
        text1.text = worldPos.ToString() + " SnapToGrid " + pos.ToString();
        return pos;
    }

    private Vector3 ClampVector3(Vector3 vector)
    {
        float x = Mathf.Clamp(vector.x, -1f, 1f);
        float y = Mathf.Clamp(vector.y, -1f, 1f);
        float z = Mathf.Clamp(vector.z, -1f, 1f);
        return new Vector3(x, y, z);
    }
}