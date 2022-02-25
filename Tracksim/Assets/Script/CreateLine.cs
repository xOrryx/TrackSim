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

    List<Vector3> points = new List<Vector3>();

    static int i = 0;

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
        ResetLine();
        mainLine.positionCount = points.Count;

        for(int i = 0; i < points.Count; i++)
        {
            mainLine.SetPosition(i, points[i]);
        }
    }

    private void ResetLine()
    {
        mainLine.positionCount = 0;
    }

}
