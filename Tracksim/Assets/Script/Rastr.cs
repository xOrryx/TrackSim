using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rastr : MonoBehaviour
{
    [SerializeField] private Plane mainPlane;
    [SerializeField] private Material rastrMaterial;
    [SerializeField] private InputField field;
    public int distance = 0;

    private Vector3 startPoint = new Vector3(0, 0.05f, 0);
    private Vector3[] verticalLine;
    private Vector3[] horizontalLine;


    // Start is called before the first frame update
    void Start()
    {
        distance = 5;
        // Plane, which was used, doesnt have Transform API. It's not possible to get its sizes. I managed it by measuring it
        //verticalLine = new Vector3[] { new Vector3(0, 0.05f, -15), startPoint, new Vector3(0, 0.05f, 35) };
        //horizontalLine = new Vector3[] { new Vector3(-10, 0.05f, 0), startPoint, new Vector3(40, 0.05f, 0) };

        verticalLine = new Vector3[] { new Vector3(0, 0.05f, -15), startPoint, new Vector3(0, 0.05f, 53.5f) };
        horizontalLine = new Vector3[] { new Vector3(-10, 0.05f, 0), startPoint, new Vector3(100.5f, 0.05f, 0) };

        CreateRastr();

    }

    /// <summary>
    /// event system from UI to get value
    /// </summary>
    public void getDistance()
    {       
        distance = int.Parse(field.text);
        //reset lines
        ResetLines();
        // create new rastr
        CreateRastr();
    }

    /// <summary>
    /// main method
    /// </summary>
    public void CreateRastr()
    {
        // create vertical line origin
        CreateLine(verticalLine, "vertical");
        // create horizontal line origin
        CreateLine(horizontalLine, "horizontal");
        // create same lines
        CreateCopies();
    }


    /// <summary>
    /// Creates vertices lines
    /// </summary>
    private void CreateLine(Vector3[] array, string name)
    {
        GameObject emptyObject = new GameObject();
        emptyObject.AddComponent<LineRenderer>();
        emptyObject.transform.name = "lane - " + name ;
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
        Vector3[] tempArrayV = new Vector3[verticalLine.Length];
        Vector3[] tempArrayH = new Vector3[horizontalLine.Length];

        Array.Copy(verticalLine, tempArrayV, verticalLine.Length);
        Array.Copy(horizontalLine, tempArrayH, verticalLine.Length);

        int counter = 0;

        int minIv = Mathf.FloorToInt((-10) / distance);
        int maxIv = Mathf.FloorToInt((100.5f) / distance);
        int minIh = Mathf.FloorToInt((-15) / distance);
        int maxIh = Mathf.FloorToInt((53) / distance);

        for (int i = minIv; i <= maxIv; ++i)
        {
            if(i != 0)
            {
                for (int j = 0; j < 3; ++j)
                    tempArrayV[j].x = i * distance;

                CreateLine(tempArrayV, counter.ToString());

                counter++;
            }
        }

        for (int i = minIh; i <= maxIh; ++i)
        {
            if (i != 0)
            {
                for (int j = 0; j < 3; ++j)
                    tempArrayH[j].z = i * distance;

                CreateLine(tempArrayH, counter.ToString());

                counter++;
            }
        }
    }

    private void ResetLines()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
