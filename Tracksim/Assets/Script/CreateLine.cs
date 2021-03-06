// (BPC-PRP project) creating Track for robot
// author: Petr ?op?k (221022)
// team: team 3
// class function: main class to create Lines

using System;
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
    private int widthSpace = 4;
    private float widthLine = 0.5f;

    public List<Vector3> points = new List<Vector3>();
    public List<Vector3> triPoints = new List<Vector3>();
    public bool usedTri = false;
    public int TriLineShape = 0;
    public int TriLineType = 0;

    // three line types
    public enum Status
    {
        straight = 0,
        curve = 1,
        tri = 2
    }

    // Start is called on the start
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

    #region Public methods

    /// <summary>
    /// get the Left mouse click and find the closest point on plane
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="hits"></param>
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

    #endregion

    #region Private methods

    #region Renderer

    /// <summary>
    /// creating primitive object - Cube on the point 
    /// </summary>
    private void CreateCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.position = points[points.Count-1];
        cube.transform.localScale = new Vector3(1, 1, 1);
        cube.name = "cubeRED" + (points.Count-1).ToString() + this.name.ToString();
        cube.GetComponent<Renderer>().material.color = Color.red;
        Destroy(cube.GetComponent<BoxCollider>());   
    } 
    
    /// <summary>
    /// Find what type of Line was chosen
    /// </summary>
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
            if(TriLineShape == 0) //straight
            {
                usedTri = true;
                makeTriLinesStraight();
                DrawLine();
                DrawTriLines();
            }
            else if(TriLineShape == 1) // curve
            {
                usedTri = true;
                if (number == 1)
                {
                    int tempC = points.Count; // n + 2 -> n is before -> to create curve need 2 points + start point
                    Vector3[] oldPoints = new Vector3[] { points[points.Count - 1], points[points.Count - 2], points[points.Count - 3]};
                    MakeCurve();
                    points.RemoveAt(tempC - 1);
                    points.RemoveAt(tempC - 2);
                    makeTriLinesCurve(oldPoints);
                    DrawLine();
                    DrawTriLines();
                    number = 0;
                }
                else
                    number++;
            }
            else
            {
                Debug.LogError("Dropdown input problem");
            }
        }
        else
        {
            Debug.LogError("Button Status is wrong");
        }
    }

    #endregion

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

        for (int i = 0; i < pointList.Count; i++)
        {
            points.Add(pointList[i]);
        }
        pointList.Clear();
    }

   

    #region Calculate TriLine straight methods

    /// <summary>
    /// Calculate parallels for straight line
    /// </summary>
    private void makeTriLinesStraight()
    {
        Vector3 pointStart = points[points.Count - 2]; // penultimate point
        Vector3 pointEnd = points[points.Count - 1];

        Vector3 normal = (pointEnd - pointStart);
        normal /= normal.z;

        float angle1 = Vector3.Angle(normal, Vector3.forward);
        float angle2 = Vector3.Angle(normal, Vector3.back);

        switch (TriLineType)
        {
            case 0: // one side -  left
                addLeftSide(pointStart, pointEnd, angle1, angle2);
                break;
            case 1: // one side -  right
                addRightSide(pointStart, pointEnd, angle1, angle2);
                break;
            case 2: // both side
                addBothSide(pointStart, pointEnd, angle1, angle2);
                break;
            default: // one side -  left
                addLeftSide(pointStart, pointEnd, angle1, angle2);
                break;
        }

    }

    /// <summary>
    /// add points about right side lines (straight) to buffer
    /// </summary>
    /// <param name="pointStart"></param>
    /// <param name="pointEnd"></param>
    /// <param name="angle1"></param>
    /// <param name="angle2"></param>
    private void addRightSide(Vector3 pointStart, Vector3 pointEnd, float angle1, float angle2)
    {
        if ((angle1 > 55 && angle1 < 125 || angle2 > 55 && angle2 < 125))
        {
            // create upper parallel
            pointStart.z -= widthSpace;
            triPoints.Add(pointStart);
            pointEnd.z -= widthSpace;
            triPoints.Add(pointEnd);

        }
        else
        {
            // create left parallel
            pointStart.x += widthSpace;
            triPoints.Add(pointStart);
            pointEnd.x += widthSpace;
            triPoints.Add(pointEnd);
        }
    }

    /// <summary>
    /// add points about left side lines (straight) to buffer
    /// </summary>
    /// <param name="pointStart"></param>
    /// <param name="pointEnd"></param>
    /// <param name="angle1"></param>
    /// <param name="angle2"></param>
    private void addLeftSide(Vector3 pointStart, Vector3 pointEnd, float angle1, float angle2)
    {
        if ((angle1 > 60 && angle1 < 140))
        {
            // create upper parallel
            pointStart.z += widthSpace;
            triPoints.Add(pointStart);
            pointEnd.z += widthSpace;
            triPoints.Add(pointEnd);

        }
        else
        {
            // create left parallel
            pointStart.x -= widthSpace;
            triPoints.Add(pointStart);
            pointEnd.x -= widthSpace;
            triPoints.Add(pointEnd);
        }
    }

    /// <summary>
    /// add points about Both side lines (straight) to buffer
    /// </summary>
    /// <param name="pointStart"></param>
    /// <param name="pointEnd"></param>
    /// <param name="angle1"></param>
    /// <param name="angle2"></param>
    private void addBothSide(Vector3 pointStart, Vector3 pointEnd, float angle1, float angle2)
    {
        if ((angle1 > 55 && angle1 < 125 || angle2 > 55 && angle2 < 125))
        {
            // create upper parallel
            pointStart.z -= widthSpace;
            triPoints.Add(pointStart);
            pointEnd.z -= widthSpace;
            triPoints.Add(pointEnd);

            // create lower parallel
            pointStart.z += widthSpace * 2;
            triPoints.Add(pointStart);
            pointEnd.z += widthSpace * 2;
            triPoints.Add(pointEnd);
        }
        else
        {
            // create right parallel
            pointStart.x -= widthSpace;
            triPoints.Add(pointStart);
            pointEnd.x -= widthSpace;
            triPoints.Add(pointEnd);

            // create left parallel
            pointStart.x += widthSpace*2;
            triPoints.Add(pointStart);
            pointEnd.x += widthSpace*2;
            triPoints.Add(pointEnd);
        }
    }

    #endregion

    #region Calculate TriLine curve methods

    /// <summary>
    /// Calculate parallels for curve lines
    /// </summary>
    private void makeTriLinesCurve(Vector3[] oldPoints)
    {
        Vector3[] curvePoints;
        Vector3[] curvePoints2 = new Vector3[3];
        int index = 0;

        switch (TriLineType)
        {
            case 0: // one side -  left
                curvePoints = addLeftSideCurve(oldPoints);
                break;
            case 1: // one side -  right
                curvePoints = addRightSideCurve(oldPoints);
                break;
            case 2: // both side
                Vector3[] tempOldPoints = new Vector3[3];
                Array.Copy(oldPoints, tempOldPoints, 3);
                curvePoints = addLeftSideCurve(oldPoints);
                curvePoints2 = addRightSideCurve(tempOldPoints);
                break;
            default: // one side -  left
                curvePoints = addLeftSideCurve(oldPoints);
                break;
        }

        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(curvePoints[curvePoints.Length - 3], curvePoints[curvePoints.Length - 2], ratio);
            var tangentLineVertex2 = Vector3.Lerp(curvePoints[curvePoints.Length - 2], curvePoints[curvePoints.Length - 1], ratio);
            var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierpoint);
        }

        if(TriLineType == 2)
        {
            index = pointList.Count;
            for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
            {
                var tangentLineVertex1 = Vector3.Lerp(curvePoints2[curvePoints2.Length - 3], curvePoints2[curvePoints2.Length - 2], ratio);
                var tangentLineVertex2 = Vector3.Lerp(curvePoints2[curvePoints2.Length - 2], curvePoints2[curvePoints2.Length - 1], ratio);
                var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
                pointList.Add(bezierpoint);
            }
        }

        for (int i = 0; i < pointList.Count; i++)
        {
            if (i == 0 || i == (pointList.Count - 1) || ((i == index-1) && (index != 0) || ((i == index) && (index != 0))))
                triPoints.Add(pointList[pointList.Count - i - 1]);
            else
            {
                triPoints.Add(pointList[pointList.Count - i - 1]);
                triPoints.Add(pointList[pointList.Count - i - 1]);
            }
        }
        pointList.Clear();

    }

    /// <summary>
    /// add points about left side lines (curve) to buffer
    /// </summary>
    /// <param name="oldPoints"></param>
    private Vector3[] addLeftSideCurve(Vector3[] oldPoints)
    {
        Vector3 normal = oldPoints[0] - oldPoints[2];

        normal /= Vector3.Magnitude(normal);

        Vector3 normalAxisX = (oldPoints[2] + Vector3.right) - oldPoints[2];
        Vector3 normalAxisZ = (oldPoints[2] + Vector3.forward) - oldPoints[2];
        Vector3 normalAxisNegX = (oldPoints[2] + Vector3.left) - oldPoints[2];
        Vector3 normalAxisNegz = (oldPoints[2] + Vector3.back) - oldPoints[2];

        float angleX = Vector3.Angle(normalAxisX, normal);
        float angleNegX = Vector3.Angle(normalAxisNegX, normal);
        float angleZ = Vector3.Angle(normalAxisZ, normal);
        float angleNegZ = Vector3.Angle(normalAxisNegz, normal);

        if((angleX > 0 && angleX < 90) && (angleZ > 0 && angleZ < 90)) // I.
        {
            for(int i = 0; i < oldPoints.Length; ++i )
            {
                oldPoints[i].x -= widthSpace;
                oldPoints[i].z += widthSpace;
            }
        }
        else if ((angleX > 0 && angleX < 90) && (angleNegZ > 0 && angleNegZ < 90)) // IV.
        {
            for (int i = 0; i < oldPoints.Length; ++i)
            {
                oldPoints[i].x += widthSpace;
                oldPoints[i].z += widthSpace;
            }
        }
        else if ((angleNegX > 0 && angleNegX < 90) && (angleZ > 0 && angleZ < 90)) // II.
        {
            for (int i = 0; i < oldPoints.Length; ++i)
            {
                oldPoints[i].x -= widthSpace;
                oldPoints[i].z -= widthSpace;
            }
        }
        else if ((angleNegX > 0 && angleNegX < 90) && (angleNegZ > 0 && angleNegZ < 90)) // III.
        {
            for (int i = 0; i < oldPoints.Length; ++i)
            {
                oldPoints[i].x += widthSpace;
                oldPoints[i].z -= widthSpace;
            }
        }

        return oldPoints;
    }

    /// <summary>
    /// add points about right side lines (curve) to buffer
    /// </summary>
    /// <param name="oldPoints"></param>
    private Vector3[] addRightSideCurve(Vector3[] oldPoints)
    {
        Vector3 normal = oldPoints[0] - oldPoints[2];

        normal /= Vector3.Magnitude(normal);

        Vector3 normalAxisX = (oldPoints[2] + Vector3.right) - oldPoints[2];
        Vector3 normalAxisZ = (oldPoints[2] + Vector3.forward) - oldPoints[2];
        Vector3 normalAxisNegX = (oldPoints[2] + Vector3.left) - oldPoints[2];
        Vector3 normalAxisNegz = (oldPoints[2] + Vector3.back) - oldPoints[2];

        float angleX = Vector3.Angle(normalAxisX, normal);
        float angleNegX = Vector3.Angle(normalAxisNegX, normal);
        float angleZ = Vector3.Angle(normalAxisZ, normal);
        float angleNegZ = Vector3.Angle(normalAxisNegz, normal);

        if ((angleX > 0 && angleX < 90) && (angleZ > 0 && angleZ < 90)) // I.
        {
            oldPoints[0].x += widthSpace;
            oldPoints[0].z -= widthSpace;
            oldPoints[1].x += widthSpace;
            oldPoints[1].z -= widthSpace;
            oldPoints[2].x += widthSpace;
            oldPoints[2].z -= widthSpace;
        }
        else if ((angleX > 0 && angleX < 90) && (angleNegZ > 0 && angleNegZ < 90)) // IV.
        {
            oldPoints[0].x -= widthSpace;
            oldPoints[0].z -= widthSpace;
            oldPoints[1].x -= widthSpace;
            oldPoints[1].z -= widthSpace;
            oldPoints[2].x -= widthSpace;
            oldPoints[2].z -= widthSpace;
        }
        else if ((angleNegX > 0 && angleNegX < 90) && (angleZ > 0 && angleZ < 90)) // II.
        {
            oldPoints[0].x += widthSpace;
            oldPoints[0].z += widthSpace;
            oldPoints[1].x += widthSpace;
            oldPoints[1].z += widthSpace;
            oldPoints[2].x += widthSpace;
            oldPoints[2].z += widthSpace;
        }
        else if ((angleNegX > 0 && angleNegX < 90) && (angleNegZ > 0 && angleNegZ < 90)) // III.
        {
            oldPoints[0].x -= widthSpace;
            oldPoints[0].z += widthSpace;
            oldPoints[1].x -= widthSpace;
            oldPoints[1].z += widthSpace;
            oldPoints[2].x -= widthSpace;
            oldPoints[2].z += widthSpace;
        }

        return oldPoints;
    }

    /// <summary>
    /// add points about both sides of lines (curve) to buffer
    /// </summary>
    /// <param name="oldPoints"></param>
    private Vector3[] addBothSideCurve(Vector3[] oldPoints)
    {
        Vector3 normal = oldPoints[0] - oldPoints[2];

        normal /= Vector3.Magnitude(normal);

        Vector3 normalAxisX = (oldPoints[2] + Vector3.right) - oldPoints[2];
        Vector3 normalAxisZ = (oldPoints[2] + Vector3.forward) - oldPoints[2];
        Vector3 normalAxisNegX = (oldPoints[2] + Vector3.left) - oldPoints[2];
        Vector3 normalAxisNegz = (oldPoints[2] + Vector3.back) - oldPoints[2];

        float angleX = Vector3.Angle(normalAxisX, normal);
        float angleNegX = Vector3.Angle(normalAxisNegX, normal);
        float angleZ = Vector3.Angle(normalAxisZ, normal);
        float angleNegZ = Vector3.Angle(normalAxisNegz, normal);

        if ((angleX > 0 && angleX < 90) && (angleZ > 0 && angleZ < 90)) // I.
        {
            oldPoints[0].x += widthSpace;
            oldPoints[0].z -= widthSpace;
            oldPoints[1].x += widthSpace;
            oldPoints[1].z -= widthSpace;
            oldPoints[2].x += widthSpace;
            oldPoints[2].z -= widthSpace;
        }
        else if ((angleX > 0 && angleX < 90) && (angleNegZ > 0 && angleNegZ < 90)) // IV.
        {
            oldPoints[0].x -= widthSpace;
            oldPoints[0].z -= widthSpace;
            oldPoints[1].x -= widthSpace;
            oldPoints[1].z -= widthSpace;
            oldPoints[2].x -= widthSpace;
            oldPoints[2].z -= widthSpace;
        }
        else if ((angleNegX > 0 && angleNegX < 90) && (angleZ > 0 && angleZ < 90)) // II.
        {
            oldPoints[0].x += widthSpace;
            oldPoints[0].z += widthSpace;
            oldPoints[1].x += widthSpace;
            oldPoints[1].z += widthSpace;
            oldPoints[2].x += widthSpace;
            oldPoints[2].z += widthSpace;
        }
        else if ((angleNegX > 0 && angleNegX < 90) && (angleNegZ > 0 && angleNegZ < 90)) // III.
        {
            oldPoints[0].x -= widthSpace;
            oldPoints[0].z += widthSpace;
            oldPoints[1].x -= widthSpace;
            oldPoints[1].z += widthSpace;
            oldPoints[2].x -= widthSpace;
            oldPoints[2].z += widthSpace;
        }

        return oldPoints;
    }
    #endregion

    #region Draw Line

    /// <summary>
    /// Draw main Line (starts from 0,0,0)
    /// </summary>
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

        mainLine.startWidth = widthLine;
        mainLine.endWidth = widthLine;
    }

    /// <summary>
    /// Draw other Lines, which were created (for TriLine)
    /// </summary>
    private void DrawTriLines()
    {
        ResetTriLines();

        for (int i = 0; i < (triPoints.Count-1); i+=2)
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
            lineRenObj.startWidth = widthLine;
            lineRenObj.endWidth = widthLine;

        }
    }

    #endregion

    #region Reset Lines

    /// <summary>
    /// Delete Line
    /// </summary>
    private void ResetLine()
    {
        mainLine.positionCount = 0;
    }

    /// <summary>
    /// Delete all TriLine lines
    /// </summary>
    private void ResetTriLines()
    {
        for (int i = 1; i <= triPoints.Count/2; ++i)
        {
            Destroy(GameObject.Find("lineTri" + i.ToString()));
        }
    }

    #endregion

    #endregion
}
