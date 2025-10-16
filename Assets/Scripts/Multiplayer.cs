using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static CreateMultiplayer;
using static Life;

public class CreateMultiplayer : MonoBehaviour
{
    public GameObject StepCountInputField;
    public GameObject GameSpeedInputField;

    public GameObject GameResultLabel;

    int StepCount;
    int GameSpeed;

    List<List<CellState>> firstPlayerField;
    List<List<CellState>> secondPlayerField;

    GameObject GameSettingsUI;
    GameObject FirstPlayerUI;
    GameObject SecondPlayerUI;
    GameObject GameUI;
    GameObject EndGameUI;

    void Start()
    {
        GameSettingsUI = GameObject.Find("GameSettings");
        FirstPlayerUI = GameObject.Find("FirstPlayer");
        SecondPlayerUI = GameObject.Find("SecondPlayer");
        GameUI = GameObject.Find("Game");
        EndGameUI = GameObject.Find("EndGame");

        FirstPlayerUI.SetActive(false);
        SecondPlayerUI.SetActive(false);
        GameUI.SetActive(false);
        EndGameUI.SetActive(false);
    }

    void Update()
    {
    
    }
    public void ApplyGameSettings()
    {
        if (int.TryParse(StepCountInputField.GetComponent<TMP_InputField>().text, out StepCount) &&
            int.TryParse(GameSpeedInputField.GetComponent<TMP_InputField>().text, out GameSpeed))
        {
            GameSettingsUI.SetActive(false);
            FirstPlayerUI.SetActive(true);
            GetComponent<LifeVisual>().enabled = true;
        }
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

    List<List<CellState>> ConcatFields()
    {
        List<List<CellState>> field = new();

        for (int i = 0; i < firstPlayerField.Count; i++)
        {
            field.Add(new List<CellState>());

            field[i].AddRange(firstPlayerField[i]);
            for (int j = 0; j < field[i].Count; j++)
                if (field[i][j] == CellState.Live)
                    field[i][j] = CellState.FirstPlayer;

            field[i].AddRange(secondPlayerField[i]);
            for (int j = firstPlayerField[i].Count; j < field[i].Count; j++)
                if (field[i][j] == CellState.Live)
                    field[i][j] = CellState.SecondPlayer;
        }

        return field;
    }

    public void StartGame()
    {
        List<List<CellState>> field = ConcatFields();
        GetComponent<LifeVisual>().RecreateGame(field.Count, field[0].Count, true, false,
                                                field, StepCount, GameSpeed,
                                                (GameResult gameResult) => EndGame(gameResult));
    }

    public enum GameResult
    {
        FirstWin,
        SecondWin,
        Draw
    };

    public void EndGame(GameResult gameResult)
    {
        string gameResultLabelText = string.Empty;
        switch (gameResult )
        {
            case GameResult.FirstWin:
                gameResultLabelText = "FIRST PLAYER WIN!";
                break;
            case GameResult.SecondWin:
                gameResultLabelText = "SECOND PLAYER WIN!";
                break;
            case GameResult.Draw:
                gameResultLabelText = "DRAW!";
                break;

        }

        TMP_Text text = GameResultLabel.GetComponent<TMP_Text>();
        text.text = gameResultLabelText;

        GetComponent<LifeVisual>().StopLife();
        GetComponent<LifeVisual>().DeleteField();
        GetComponent<LifeVisual>().enabled = false;

        GameUI.SetActive(false);
        EndGameUI.SetActive(true);
    }
}
