using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateLine : MonoBehaviour
{
    [SerializeField] private Plane mainPlane;
    [SerializeField] private LineRenderer mainLine;

    private Ray ray;
    private float timeButtonDown;

    public List<Vector3> points = new List<Vector3>();
    private List<Vector3> pointList = new List<Vector3>();
    private int vertexCount = 12;
    private int number = 0;

    [SerializeField] private OnlyOneButton buttonStatus;

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
        cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
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

        }
        else // new Line
        {

        }

    }

    private void MakeCurve()
    {
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(points[points.Count - 3], points[points.Count - 2], ratio);
            var tangentLineVertex2 = Vector3.Lerp(points[points.Count - 2], points[points.Count - 1], ratio);
            var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierpoint);
        }

        Debug.Log(pointList.Count);

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

    private void ResetLine()
    {
        mainLine.positionCount = 0;
    }

}
