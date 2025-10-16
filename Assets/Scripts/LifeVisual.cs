using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.U2D;
using static CreateMultiplayer;
using static Life;

public class LifeVisual : MonoBehaviour
{
    public int h = 60;
    public int w = 30;
    public bool interactive = true;
    public bool multiplayer = false;

    const uint maxspeed = 25;

    bool game = false;
    int speed = 1;
    int frameNum = 0;
    int stepCount = 0;

    Life life;
    List<List<GameObject>> all;

    Action<GameResult> endGameAction;

    Color ColorForCellState(CellState cellState)
    {
        switch (cellState)
        {
            case CellState.Die:
                return Color.white;
            case CellState.Live:
                return Color.black;
            case CellState.FirstPlayer:
                return Color.green;
            case CellState.SecondPlayer:
                return Color.blue;
        }

        return Color.white;
    }

    void Start()
    {
        life = new Life(h + 1, w + 1, multiplayer);

        CreateCells();
        updateSprites();

        if (!interactive)
        {
            CreateRandomField();
            game = true;
            speed = 2;
        }
    }

    void CreateCells()
    {
        all = new();
        GameObject sprite = Resources.Load("Cell") as GameObject;
        sprite.GetComponent<Cell>().interactive = interactive;

        for (int i = -h / 2; i <= h / 2; i++)
        {
            all.Add(new List<GameObject>());
            for (int j = -w / 2; j <= w / 2; j++)
            {
                GameObject Sprite = Instantiate(sprite);
                Sprite.transform.position = new Vector2(j, i);
                Sprite.GetComponent<Cell>().i = i + h / 2;
                Sprite.GetComponent<Cell>().j = j + w / 2;
                all[i + h / 2].Add(Sprite);
            }
        }
    }

    public void DeleteField()
    {
        game = false;
        for (int i = 0; i < all.Count; i++)
        {
            for (int j = 0; j < all[i].Count; j++)
            {
                all[i][j].SetActive(false);
                all[i][j] = null;
            }
            all[i] = null;
        }
        all = null;
    }

    public void ClearField()
    {
        life = new Life(h + 1, w + 1, multiplayer);
        updateSprites();
    }


    public void RecreateGame(int new_h, int new_w, bool multiplayer, bool interactive,
                             List<List<CellState>> field, int stepCount, int gameSpeed,
                             Action<GameResult> endGameAction)
    {
        this.multiplayer = multiplayer;
        this.interactive = interactive;

        DeleteField();

        h = new_h;
        w = new_w;
        life = new Life(h, w, multiplayer, field);
        CreateCells();
        updateSprites();

        game = true;
        speed = gameSpeed;
        this.stepCount = stepCount;
        this.endGameAction = endGameAction;
    }

    public List<List<CellState>> getField()
    {
        var field = life.getField();
        List<List<CellState>> deepCopy = new();
        for (int i = 0; i < field.Count; i++)
            deepCopy.Add(new List<CellState>(field[i]));
        return deepCopy;
    }

    void Update()
    {
        if (game)
        {
            if (frameNum >= 150 / speed)
            {
                life.Step();
                updateSprites();

                frameNum = 0;

                if (multiplayer && stepCount <= life.getStep())
                {
                    CountWinner();
                }
            }
            frameNum++;
        }
    }

    void CountWinner()
    {
        int firstPlayerCounter = 0;
        int secondPlayerCounter = 0;

        List<List<CellState>> field = life.getField();
        for (int i = 0; i < field.Count; i++)
        {
            for (int j = 0; j < field[i].Count; j++)
            {
                if (field[i][j] == CellState.FirstPlayer)
                    firstPlayerCounter++;
                if (field[i][j] == CellState.SecondPlayer)
                    secondPlayerCounter++;
            }
        }

        GameResult result = GameResult.Draw;
        if (firstPlayerCounter < secondPlayerCounter)
        {
            result = GameResult.SecondWin;
            Debug.Log("Second player win!");
        }
        else if (firstPlayerCounter > secondPlayerCounter)
        {
            result = GameResult.FirstWin;
            Debug.Log("First player win!");
        }

        StopLife();
        endGameAction(result);
    }

    public void StartLife()
    {
        game = true;
    }

    public void StopLife()
    {
        game = false;
    }

    public void SpeedUp()
    {
        if (speed < maxspeed)
        {
            speed++;
        }
    }
    public void SpeedDown()
    {
        if (speed > 1)
            speed--;
    }

    private void updateSprites()
    {
        List<List<CellState>> field = life.getField();
        for (int i = 0; i < field.Count; i++)
        {
            for (int j = 0; j < field[i].Count; j++)
            {
                all[i][j].GetComponent<SpriteRenderer>().color = ColorForCellState(field[i][j]);
            }
        }
    }
    public void CreateRandomField()
    {
        if (!game)
        {
            life.setRandomField();
            updateSprites();
        }
    }
    public void ChangeCell(int i, int j)
    {
        life.inverseCell(i, j);
        all[i][j].GetComponent<SpriteRenderer>().color = ColorForCellState(life.getCell(i, j));
    }
}
