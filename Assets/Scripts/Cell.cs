using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int i, j;
    public bool interactive = false;
    LifeVisual main;
    private void Start()
    {
        if (interactive)
        {
            main = GameObject.Find("Main Camera").GetComponent<LifeVisual>();
        }
    }

    private void OnMouseDown()
    {
        if (interactive)
            main.ChangeCell(i, j);
    }
}
