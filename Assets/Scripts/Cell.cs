using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int i, j;
    public bool interactive = false;
    LifeVisual main;
    MoveCamera cameraMove;

    Vector2 oldMousePos;
    bool drag= false;
    Vector3 mousePos = Vector3.zero;
    private void Start()
    {
        if (interactive)
        {
            GameObject mainCamera = GameObject.Find("Main Camera");
            main = mainCamera.GetComponent<LifeVisual>();
            cameraMove = mainCamera.GetComponent<MoveCamera>();
        }
    }

    void Update()
    {
        if (interactive && drag)
        {
            Vector3 mouse = Input.mousePosition;
            cameraMove.Move(mousePos - mouse);
            mousePos = mouse;

            if (!Input.GetMouseButton(1))
            {
                drag = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (interactive)
        {
            main.ChangeCell(i, j);
        }
    }

    private void OnMouseOver()
    {
        if (interactive)
        {
            if (!drag && Input.GetMouseButton(1))
            {
                drag = true;
                mousePos = Input.mousePosition;
            }
        }
    }
}
