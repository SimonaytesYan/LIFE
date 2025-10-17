using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.U2D;
using static CreateMultiplayer;
using static Life;

class LifePrefab
{
    Vector2Int size;
    List<List<bool>> cells;

    LifePrefab(List<List<bool>> cells)
    {
        size.y = cells.Count;
        for (int i = 0; i < size.y; i++)
        {
            size.x = Math.Max(size.x, cells[i].Count);
        }
        this.cells = cells;

    }
    public List<List<bool>> GetCells()
    {
        return cells;
    }

    Vector2 Size()
    {

        return size;
    }

    public static LifePrefab CreateGlider()
    {
        List<List<bool>> result = new List<List<bool>>
        {
            new List<bool> { false, true,  false},
            new List<bool> { false, false, true },
            new List<bool> { true,  true,  true },
        };

        return new LifePrefab(result);
    }
    public static LifePrefab CreateSpaceShip()
    {
        List<List<bool>> result = new List<List<bool>>
        {
            new List<bool> { true,  false, false, true,  false},
            new List<bool> { false, false, false, false, true},
            new List<bool> { true,  false, false, false, true},
            new List<bool> { false, true,  true , true,  true},
        };

        return new LifePrefab(result);
    }
    public static LifePrefab CreateMethuselah()
    {
        List<List<bool>> result = new List<List<bool>>
        {
            new List<bool> { false, true,  false},
            new List<bool> { false, true,  true },
            new List<bool> { true,  true,  false},
        };

        return new LifePrefab(result);
    }
};

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
    Dictionary<string, LifePrefab> prefabs;
    LifePrefab currentPrefab;

    void Start()
    {
        prefabs = new();
        prefabs.Add("glider", LifePrefab.CreateGlider());
        prefabs.Add("methuselah", LifePrefab.CreateMethuselah());
        prefabs.Add("spaceship", LifePrefab.CreateSpaceShip());

        life = new Life(h, w, multiplayer);

        CreateCells();
        updateSprites();

        if (!interactive)
        {
            CreateRandomField();
            game = true;
            speed = 2;
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
                Destroy(all[i][j]);
                all[i][j] = null;
            }
            all[i] = null;
        }
        all = null;
    }

    public void ClearField()
    {
        life = new Life(h, w, multiplayer);
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

    public void CreateRandomField()
    {
        life.setRandomField();
        updateSprites();
    }
    public void ChangeCell(int i, int j)
    {
        life.inverseCell(i, j);
        all[i][j].GetComponent<SpriteRenderer>().color = ColorForCellState(life.getCell(i, j));
    }

    public void BuildPrefab(int start_i, int start_j)
    {
        if (currentPrefab != null)
        {
            List<List<bool>> prefabCells = currentPrefab.GetCells();
            for (int i = 0; i < prefabCells.Count; i++)
            {
                for (int j = 0; j < prefabCells[i].Count; j++)
                {
                    if (prefabCells[i][j])
                        life.setCell(start_i + i, start_j + j, CellState.Live);
                    else
                        life.setCell(start_i + i, start_j + j, CellState.Die);
                }
            }
            updateSprites();

            currentPrefab = null;
        }
    }

    public void StartBuildingPrefab(string name)
    {
        currentPrefab = prefabs[name];
    }

    private Color ColorForCellState(CellState cellState)
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
    private void CountWinner()
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
        }
        else if (firstPlayerCounter > secondPlayerCounter)
        {
            result = GameResult.FirstWin;
        }

        StopLife();
        endGameAction(result);
    }

    void CreateCells()
    {
        all = new();
        GameObject sprite = Resources.Load("Cell") as GameObject;
        sprite.GetComponent<Cell>().interactive = interactive;

        for (int i = 0; i < h; i++)
        {
            all.Add(new List<GameObject>());
            for (int j = 0; j < w; j++)
            {
                GameObject Sprite = Instantiate(sprite);
                Sprite.transform.position = new Vector2(j - w / 2, i - h / 2);
                Sprite.GetComponent<Cell>().i = i;
                Sprite.GetComponent<Cell>().j = j;
                all[i].Add(Sprite);
            }
        }
    }
}
