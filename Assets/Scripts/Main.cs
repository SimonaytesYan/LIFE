using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Windows;
public class Main : MonoBehaviour
{
    const uint maxspeed = 25;
    const int h = 60;
    const int w = 30;

    bool game = false;
    int speed = 1;
    int frameNum = 0;

    GameObject Steps;

    Life life = new Life(h + 1, w + 1);
    List<List<GameObject>> all = new List<List<GameObject>>();
    void Start()
    {
        Steps = GameObject.Find("Steps");

        GameObject sprite = GameObject.Find("1");
        for (int i = -h/2; i <= h/2; i++)
        {
            all.Add(new List<GameObject>());
            for (int j = -w/2; j <= w/2; j++)
            {
                GameObject Sprite = Instantiate(sprite);
                Sprite.transform.position = new Vector2(i, j);
                Sprite.GetComponent<Cell>().i = i + h/2;
                Sprite.GetComponent<Cell>().j = j + w/2;
                all[i + h/2].Add(Sprite);
            }
        }
    }

    void Update()
    {
        if (game)
        {
            if (frameNum >= 150 / speed)
            {
                life.Step();
                updateSprites();

                Steps.GetComponent<TMP_Text>().text = "Steps: " + life.getStep().ToString();

                frameNum = 0;
            }
            frameNum++;
        }
    }

    public void Go()
    {
        game = true;
    }

    public void Stop()
    {
        game = false;
    }

    public void SpeedUp()
    {
        if (speed < maxspeed) 
        {
            speed++;
            GameObject.Find("Speed").GetComponent<TMP_Text>().text = "Speed: " + speed.ToString();
        }
    }

    private void updateSprites()
    {
        List<List<bool>> field = life.getField();
        for (int i = 0; i < field.Count; i++)
        {
            for (int j = 0; j < field[i].Count; j++)
            {
                Color new_color = Color.white;
                if (field[i][j])
                {
                    new_color = Color.black;
                }
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
            GameObject.Find("Speed").GetComponent<TMP_Text>().text = "Speed: " + speed.ToString();
        }
    }
    public void ChangeCell(int i, int j)
    {
        life.changeCell(i, j);
        if (life.getCell(i, j))
            all[i][j].GetComponent<SpriteRenderer>().color = Color.black;
        else
            all[i][j].GetComponent<SpriteRenderer>().color = Color.white;
    }
}
