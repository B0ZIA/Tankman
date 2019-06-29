using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFollow : MonoBehaviour {

    public float grid = 0.5f;
    float x = 0f;
    float y = 0f;
    //public Camera camera = Camera.main;

    //public void Start()
    //{
    //    camera = Camera.main;
    //}


    void Update ()
    {
        if (grid > 0)
        {
            float reciprocalGrid = 1f / grid;

            var mousePos = Input.mousePosition;
            mousePos.z = 10; // select distance = 10 units from the camera
            //Debug.Log(camera.ScreenToWorldPoint(mousePos));

            x = Mathf.Round(Camera.main.ScreenToWorldPoint(mousePos).x * reciprocalGrid) / reciprocalGrid;
            y = Mathf.Round(Camera.main.ScreenToWorldPoint(mousePos).y * reciprocalGrid) / reciprocalGrid;

            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
