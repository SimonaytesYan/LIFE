using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class LifeVisual : MonoBehaviour
{
    public int h = 60;
    public int w = 30;
    public bool interactive = true;

    const uint maxspeed = 25;

    bool game = false;
    int speed = 1;
    int frameNum = 0;

    Life life;
    List<List<GameObject>> all;

    Color DeadCellColor = Color.black;
    Color LiveCellColor = Color.white;
    void Start()
    {
        life = new Life(h + 1, w + 1);

        CreateCells();

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

    void DeleteField()
    {
        game = false;
        for (int i = 0; i <= h; i++)
        {
            for (int j = 0; j <= w; j++)
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
        life = new Life(h + 1, w + 1);
        updateSprites();
    }


    public void RecreateGame(int new_h, int new_w, List<List<bool>> field)
    {
        DeleteField();

        h = new_h;
        w = new_w;
        life = new Life(h + 1, w + 1, field);
        CreateCells();
        updateSprites();
    }

    public List<List<bool>> getField()
    {
        var field = life.getField();
        List<List<bool>> deepCopy = new();
        for (int i = 0; i < field.Count; i++)
        {
            deepCopy.Add(new List<bool>(field[i]));
        }
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

    private void updateSprites()
    {
        List<List<bool>> field = life.getField();
        for (int i = 0; i < field.Count; i++)
        {
            for (int j = 0; j < field[i].Count; j++)
            {
                Color new_color = LiveCellColor;
                if (field[i][j])
                    new_color = DeadCellColor;
                all[i][j].GetComponent<SpriteRenderer>().color = new_color;
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

    public void SpeedDown()
    {
        if (speed > 1)
        {
            speed--;
        }
    }
    public void ChangeCell(int i, int j)
    {
        life.changeCell(i, j);
        if (life.getCell(i, j))
            all[i][j].GetComponent<SpriteRenderer>().color = DeadCellColor;
        else
            all[i][j].GetComponent<SpriteRenderer>().color = LiveCellColor;
    }
}
