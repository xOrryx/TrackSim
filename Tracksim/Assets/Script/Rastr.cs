using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rastr : MonoBehaviour
{
    [SerializeField] private GameObject rastr;
    [SerializeField] private Plane mainPlane;
    public int distance = 0;

    // Start is called before the first frame update
    void Start()
    {
        distance = 1;
        CreateRastr();

    }

    
    private void CreateRastr()
    {
        
    }

    

}
