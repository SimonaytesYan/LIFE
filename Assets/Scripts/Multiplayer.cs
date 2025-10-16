using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateMultiplayer : MonoBehaviour
{
    List<List<bool>> firstPlayerField;
    List<List<bool>> secondPlayerField;

    GameObject FirstPlayerUI;
    GameObject SecondPlayerUI;
    GameObject GameUI;

    void Start()
    {
        FirstPlayerUI = GameObject.Find("FirstPlayer");
        SecondPlayerUI = GameObject.Find("SecondPlayer");
        GameUI = GameObject.Find("Game");

        SecondPlayerUI.SetActive(false);
    }

    void Update()
    {
        
    }

    public void ApplyFirstPlayerField()
    {
        firstPlayerField = GetComponent<LifeVisual>().getField();

        FirstPlayerUI.SetActive(false);
        SecondPlayerUI.SetActive(true);

        GetComponent<LifeVisual>().ClearField();
    }


    public void ApplySecondPlayerField()
    {
        secondPlayerField = GetComponent<LifeVisual>().getField();
        StartGame();

        SecondPlayerUI.SetActive(false);
        GameUI.SetActive(true);
    }


    List<List<bool>> ConcatFields()
    {
        List<List<bool>> field = new();

        for (int i = 0; i < firstPlayerField.Count; i++)
        {
            field.Add(new List<bool>());
            field[i].AddRange(firstPlayerField[i]);
            field[i].AddRange(secondPlayerField[i]);
        }

        return field;
    }

    public void StartGame()
    {
        List<List<bool>> field = ConcatFields();
        GetComponent<LifeVisual>().RecreateGame(field.Count, field[0].Count, field);
    }
}
