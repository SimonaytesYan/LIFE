using System;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    const float moveScale = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 distance)
    {
        float currentScale = moveScale / Mathf.Sqrt(GetComponent<Camera>().orthographicSize);
        GetComponent<Transform>().position += new Vector3(currentScale * distance.x, currentScale * distance.y, 0);
    }
    public void Zoom(float zoom)
    {
        GetComponent<Camera>().orthographicSize += zoom;
        if (GetComponent<Camera>().orthographicSize <= 5)
            GetComponent<Camera>().orthographicSize = 5;
    }
    private void OnGUI()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            Zoom(-Input.mouseScrollDelta.y);
        }
    }
}
