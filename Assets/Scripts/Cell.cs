using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int i, j;
    LifeVisual main;
    private void Start()
    {
        main = GameObject.Find("Main Camera").GetComponent<LifeVisual>();
    }

    private void OnMouseDown()
    {
        main.ChangeCell(i, j);
        //Debug.Log(i.ToString() + " " + j.ToString());
    }
}
