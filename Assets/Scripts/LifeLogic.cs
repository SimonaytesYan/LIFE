using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Life
{
    int h;
    int w;
    int steps;
    List<List<bool>> field;

    static readonly Vector2Int[] neighbors = new Vector2Int[]{new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int(-1, -1),
                                                              new Vector2Int(0,  1), new Vector2Int(0,  -1),
                                                              new Vector2Int(1,  1), new Vector2Int(1,  0), new Vector2Int(1,  -1)};
    public Life(int height, int width)
    {
        h = height;
        w = width;
        steps = 0;
        field = new List<List<bool>>(h);

        for (int i = 0; i < h; i++)
        {
            field.Add(new List<bool>(w));
            for (int j = 0; j < w; j++)
            {
                field[i].Add(false);
            }
        }
    }

    public Life(int height, int width, List<List<bool>> field)
    {
        h = height;
        w = width;
        this.field = field;
    }

    public List<List<bool>> getField()
    {
        return field;
    }

    public int getStep()
    {
        return steps;
    }

    public void Step()
    {
        List<List<bool>> old_field = copyList(field);
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                bool life = IsItLife(old_field, i, j);
                field[i][j] = life;
            }
        }
        steps++;
    }
    public void setRandomField()
    {
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                bool life = UnityEngine.Random.Range(0, 7) == 0;
                field[i][j] = life;
            }
        }
    }
    public void changeCell(int i, int j)
    {
        field[i][j] = !field[i][j];
    }
    public bool getCell(int i, int j)
    {
        return field[i][j];
    }
    private List<List<bool>> copyList(List<List<bool>> list)
    {
        List<List<bool>> copy_list = new List<List<bool>>();
        for (int i = 0; i < list.Count; i++)
        {
            copy_list.Add(new List<bool>());
            for (int j = 0; j < list[i].Count; j++)
            {
                copy_list[i].Add(list[i][j]);
            }
        }
        return copy_list;
    }

    private bool IsItLife(List<List<bool>> old_field, int i, int j)
    {
        int CntLifeNeigh = 0;
        for (int ind = 0; ind < neighbors.Count(); ind++)
        {
            int neighbor_i = i + neighbors[ind].x;
            int neighbor_j = j + neighbors[ind].y;
            if (0 <= neighbor_i && neighbor_i < old_field.Count)
            {
                if (0 <= neighbor_j && neighbor_j < old_field[neighbor_i].Count)
                {
                    if (old_field[neighbor_i][neighbor_j])
                        CntLifeNeigh++;
                }
            }
        }

        if (old_field[i][j])
        {
            if (CntLifeNeigh == 2 || CntLifeNeigh == 3)
                return true;
            return false;
        }
        else
        {
            if (CntLifeNeigh == 3)
                return true;
            return false;
        }
    }
}
