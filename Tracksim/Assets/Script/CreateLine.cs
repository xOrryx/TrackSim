using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateLine : MonoBehaviour
{
    [SerializeField] private Plane mainPlane;
    [SerializeField] private LineRenderer mainLine;
    [SerializeField] private OnlyOneButton buttonStatus;
    [SerializeField] private Material lineMaterial;

    private Ray ray;
    private float timeButtonDown;
    private List<Vector3> pointList = new List<Vector3>();
    private int vertexCount = 12;
    private int number = 0;

    public List<Vector3> points = new List<Vector3>();
    public List<Vector3> triPoints = new List<Vector3>();
    public bool usedTri = false;
    public int TriLineShape = 0;
    public int TriLineType = 0;

    public enum Status
    {
        straight = 0,
        curve = 1,
        tri = 2,
        newLine = 3
    }


    void Start()
    {
        // add start point
        points.Add(new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        // cant be clicked on UI
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            timeButtonDown = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if ((Time.time - timeButtonDown) <= 0.2f)  // find out if it is mouse hold or mouse click
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray);
                LeftMouseClick(ray, hits);
            }
        }
    }

    public void LeftMouseClick(Ray ray, RaycastHit[] hits)
    {

        if(hits.Length == 1)
        {
            Vector3 tempPos =  mainPlane.ClosestPointOnPlane(hits[0].point);

            points.Add(tempPos);
            CreateCube();
            CreateLineRender();
        }
    }

    private void CreateCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube.transform.parent = this.transform;

        cube.transform.position = points[points.Count-1];
        cube.transform.localScale = new Vector3(1, 1, 1);
        cube.name = "cubeRED" + (points.Count-1).ToString() + this.name.ToString();
        cube.GetComponent<Renderer>().material.color = Color.red;
        Destroy(cube.GetComponent<BoxCollider>());   
    } 
    
    private void CreateLineRender()
    {
        if(buttonStatus.typeLine == Status.straight) //straight
        {
            DrawLine();
        }
        else if(buttonStatus.typeLine == Status.curve) //curve
        {
            if (number == 1)
            {
                int tempC = points.Count;
                MakeCurve();
                points.RemoveAt(tempC - 1);
                points.RemoveAt(tempC - 2);
                DrawLine();
                number = 0;
            }
            else
                number++;
        }
        else if(buttonStatus.typeLine == Status.tri) // tri Lines
        {
            usedTri = true;
            makeTriLines();
            DrawLine();
            DrawTriLines();
        }
        else // new Line
        {

        }

    }

    /// <summary>
    /// Calculate parallels
    /// </summary>
    private void makeTriLines()
    {
        Debug.Log(points.Count);

        Vector3 pointStart = points[points.Count - 2]; // penultimate point
        Vector3 pointEnd = points[points.Count-1];

        Debug.Log(pointStart + " " + pointEnd);

        Vector3 normal = (pointEnd - pointStart);
        normal /= normal.z;

        if (normal != Vector3.forward && normal != Vector3.back)
        {
            // create upper parallel
            pointStart.z -= 1;
            triPoints.Add(pointStart);
            pointEnd.z -= 1;
            triPoints.Add(pointEnd);

            // create lower parallel
            pointStart.z += 2;
            triPoints.Add(pointStart);
            pointEnd.z += 2;
            triPoints.Add(pointEnd);
        }
        else
        {
            // create upper parallel
            pointStart.x -= 1;
            triPoints.Add(pointStart);
            pointEnd.x -= 1;
            triPoints.Add(pointEnd);

            // create lower parallel
            pointStart.x += 1;
            triPoints.Add(pointStart);
            pointEnd.x += 1;
            triPoints.Add(pointEnd);
        }

    }

    /// <summary>
    /// Calculate curve between two points
    /// </summary>
    private void MakeCurve()
    {
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(points[points.Count - 3], points[points.Count - 2], ratio);
            var tangentLineVertex2 = Vector3.Lerp(points[points.Count - 2], points[points.Count - 1], ratio);
            var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierpoint);
        }

        for(int i = 0; i < pointList.Count; i++)
        {
            points.Add(pointList[i]);
        }
        pointList.Clear();  
    }

    private void DrawLine()
    {
        ResetLine();

        mainLine.positionCount = points.Count;

        for (int i = 0; i < points.Count; i++)
        {
            Vector3 temp = points[i];
            temp.y = 0.3f;
            mainLine.SetPosition(i, temp);
        }
    }

    private void DrawTriLines()
    {
        ResetTriLines();

        for (int i = 0; i < triPoints.Count; i+=2)
        {
            GameObject emptyObject = new GameObject();
            emptyObject.AddComponent<LineRenderer>();
            
            LineRenderer lineRenObj = emptyObject.GetComponent<LineRenderer>();
            lineRenObj.transform.name = "lineTri" + (i+1).ToString();

            lineRenObj.positionCount = 2;

            Vector3 triTemp1 = triPoints[i];
            triTemp1.y = 0.3f;
            Vector3 triTemp2 = triPoints[i+1];
            triTemp2.y = 0.3f;

            lineRenObj.SetPositions(new Vector3[] { triTemp1, triTemp2 });

            lineRenObj.material = lineMaterial;
            lineRenObj.startWidth = 0.25f;
            lineRenObj.endWidth = 0.25f;

        }
    }

    private void ResetLine()
    {
        mainLine.positionCount = 0;
    }

    private void ResetTriLines()
    {
        for (int i = 1; i <= triPoints.Count/2; ++i)
        {
            Destroy(GameObject.Find("lineTri" + i.ToString()));
        }
    }

}
