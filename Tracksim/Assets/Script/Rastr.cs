using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rastr : MonoBehaviour
{
    [SerializeField] private Plane mainPlane;
    [SerializeField] private Material rastrMaterial;
    public int distance = 0;
    private Vector3 startPoint = new Vector3(0, 0.05f, 0);
    private Vector3[] verticalLine;
    private Vector3[] horizontalLine;

    // Start is called before the first frame update
    void Start()
    {
        distance = 1;
        // Plane, which was used, doesnt have Transform API. It's not possible to get its sizes. I managed it by measuring it
        verticalLine = new Vector3[] { new Vector3(0, 0.05f, -15), startPoint, new Vector3(0, 0.05f, 35) };
        horizontalLine = new Vector3[] { new Vector3(-10, 0.05f, 0), startPoint, new Vector3(40, 0.05f, 0) };
        CreateRastr();

    }


    public void CreateRastr()
    {
        // create vertical line origin
        CreateVertLine(verticalLine, "vertical");
        // create horizontal line origin
        CreateVertLine(horizontalLine, "horizontal");
        // create same lines
        CreateCopies();
    }


    /// <summary>
    /// Creates origin line
    /// </summary>
    private void CreateVertLine(Vector3[] array, string name)
    {
        GameObject emptyObject = new GameObject();
        emptyObject.AddComponent<LineRenderer>();
        emptyObject.transform.name = "origin - " + name ;
        emptyObject.transform.localPosition = new Vector3(0, 0, 0);

        LineRenderer lineRen = emptyObject.GetComponent<LineRenderer>();

        lineRen.positionCount = 3;
        lineRen.SetPositions(array);
        lineRen.startWidth = 0.1f;
        lineRen.endWidth = 0.1f;
        lineRen.material = rastrMaterial;
        
        emptyObject.transform.SetParent(this.transform);
    }

    /// <summary>
    /// create copies of origin lines
    /// </summary>
    private void CreateCopies()
    {
        GameObject tempVert = GameObject.Find("origin - vertical");
        GameObject tempHori = GameObject.Find("origin - horizontal");

        int counter = 0;

        int minIv = Mathf.FloorToInt((-10) / distance);
        int maxIv = Mathf.FloorToInt((40) / distance);
        int minIh = Mathf.FloorToInt((-15) / distance);
        int maxIh = Mathf.FloorToInt((35) / distance);

        for (int i = minIv; i <= maxIv; ++i)
        {
            if(i != 0)
            {
                GameObject newLine = GameObject.Instantiate(tempVert, new Vector3(i * distance, 0.05f, 0), new Quaternion(0,0,0,0));
                newLine.transform.name = "lineV" + (counter).ToString();

                LineRenderer newLineRen =  newLine.GetComponent<LineRenderer>();

                Vector3[] tempArray = verticalLine;

                for(int j = 0; j < 3; ++j)
                    tempArray[j].x = i * distance;
                
                newLineRen.positionCount = 3;
                newLineRen.SetPositions(tempArray);

                newLine.transform.SetParent(this.transform);
                counter++;
            }
        }

        for (int i = minIh; i <= maxIh; ++i)
        {
            if (i != 0)
            {
                GameObject newLine = GameObject.Instantiate(tempVert, new Vector3(0, 0.05f, i * distance), new Quaternion(0, 0, 0, 0));
                newLine.transform.name = "lineH" + (counter).ToString();

                LineRenderer newLineRen = newLine.GetComponent<LineRenderer>();

                Vector3[] tempArray = horizontalLine;

                for (int j = 0; j < 3; ++j)
                    tempArray[j].z = i * distance;

                newLineRen.positionCount = 3;
                newLineRen.SetPositions(tempArray);

                newLine.transform.SetParent(this.transform);
                counter++;
            }
        }
    }
    

}
