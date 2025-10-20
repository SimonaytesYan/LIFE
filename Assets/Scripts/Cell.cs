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
        if (interactive && drag && cameraMove != null)
        {
            Move();
        }
    }

    void Move()
    {
        Vector3 mouse = Input.mousePosition;
        cameraMove.Move(mousePos - mouse);
        mousePos = mouse;

        if (!Input.GetMouseButton(1))
        {
            drag = false;
        }
    }

    private void OnMouseDown()
    {
        if (interactive)
        {
            bool prefabBuilded = main.BuildPrefab(i, j);
            if (!prefabBuilded)
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

    private void OnMouseEnter()
    {
        if (interactive)
        {
            LifePrefab currentPrefab = main.GetCurrentPrefab();
            if (currentPrefab != null)
            {
                var Cells = currentPrefab.GetCells();
                for (int x = 0; x < currentPrefab.Size().x; x++)
                {
                    for (int y = 0; y < currentPrefab.Size().y; y++)
                        if (Cells[y][x])
                            main.ColorCellToPrefab(i + y, j + x);
                }
            }
        }
    }


    private void OnMouseExit()
    {
        if (interactive)
        {
            LifePrefab currentPrefab = main.GetCurrentPrefab();
            if (currentPrefab != null)
            {
                var Cells = currentPrefab.GetCells();
                for (int x = 0; x < currentPrefab.Size().x; x++)
                {
                    for (int y = 0; y < currentPrefab.Size().y; y++)
                        if (Cells[y][x])
                            main.ClearCellColor(i + y, j + x);
                }
            }
        }
    }
}

