using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Life
{
    public enum CellState
    {
        Die,
        Live,
        FirstPlayer,
        SecondPlayer
    };

    public Life(int height, int width, bool multiplayer)
    {
        h = height;
        w = width;
        this.multiplayer = multiplayer;
        steps = 0;
        field = new List<List<CellState>>(h);

        for (int i = 0; i < h; i++)
        {
            field.Add(new List<CellState>(w));
            for (int j = 0; j < w; j++)
            {
                field[i].Add(CellState.Die);
            }
        }
    }

    public Life(int height, int width, bool multiplayer, List<List<CellState>> field)
    {
        h = height;
        w = width;
        this.multiplayer = multiplayer;
        // TODO deep copy ?
        this.field = field;
    }

    public List<List<CellState>> getField()
    {
        return field;
    }

    public int getStep()
    {
        return steps;
    }

    public void Step()
    {
        List<List<CellState>> old_field = new();
        for (int i = 0; i < field.Count; i++)
            old_field.Add(new List<CellState>(field[i]));

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                CellState life = CalcCellNextState(old_field, i, j);
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
                if (life)
                {
                    field[i][j] = CellState.Live;
                }
                else
                {
                    field[i][j] = CellState.Die;
                }
            }
        }
    }
    public void inverseCell(int i, int j)
    {
        if (field[i][j] == CellState.Live)
            field[i][j] = CellState.Die;
        else
            field[i][j] = CellState.Live;
    }
    public CellState getCell(int i, int j)
    {
        return field[i][j];
    }
    public void setCell(int i, int j, CellState new_state)
    {
        field[i][j] = new_state;
    }

    private CellState CalcCellNextState(List<List<CellState>> old_field, int i, int j)
    {
        if (multiplayer)
            return CalcCellNextStateMuliplayer(old_field, i, j);
        else
            return CalcCellNextStateLocal(old_field, i, j);
    }
    private CellState CalcCellNextStateLocal(List<List<CellState>> old_field, int i, int j)
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
                    if (old_field[neighbor_i][neighbor_j] == CellState.Live)
                        CntLifeNeigh++;
                }
            }
        }

        if (old_field[i][j] == CellState.Live)
        {
            if (CntLifeNeigh == 2 || CntLifeNeigh == 3)
                return CellState.Live;
            return CellState.Die;
        }
        else
        {
            if (CntLifeNeigh == 3)
                return CellState.Live;
            return CellState.Die;
        }
    }

    private CellState CalcCellNextStateMuliplayer(List<List<CellState>> old_field, int i, int j)
    {
        int CntFirstPlayerNeigh = 0;
        int CntSecondPlayerNeigh = 0;
        for (int ind = 0; ind < neighbors.Count(); ind++)
        {
            int neighbor_i = i + neighbors[ind].x;
            int neighbor_j = j + neighbors[ind].y;
            if (0 <= neighbor_i && neighbor_i < old_field.Count)
            {
                if (0 <= neighbor_j && neighbor_j < old_field[neighbor_i].Count)
                {
                    if (old_field[neighbor_i][neighbor_j] == CellState.FirstPlayer)
                        CntFirstPlayerNeigh++;
                    else if (old_field[neighbor_i][neighbor_j] == CellState.SecondPlayer)
                        CntSecondPlayerNeigh++;
                }
            }
        }

        int CntLifeNeigh = CntFirstPlayerNeigh + CntSecondPlayerNeigh;
        if (old_field[i][j] == CellState.Die)
        {
            if (CntLifeNeigh == 3)
            {
                if (CntFirstPlayerNeigh < CntSecondPlayerNeigh)
                    return CellState.SecondPlayer;
                else
                    return CellState.FirstPlayer;
            }
            return CellState.Die;
        }
        else
        {
            if (CntLifeNeigh == 2 || CntLifeNeigh == 3)
            {
                if (old_field[i][j] == CellState.FirstPlayer)
                    CntFirstPlayerNeigh += 1;
                else
                    CntSecondPlayerNeigh += 1;

                if (CntFirstPlayerNeigh < CntSecondPlayerNeigh)
                    return CellState.SecondPlayer;
                else if (CntFirstPlayerNeigh > CntSecondPlayerNeigh)
                    return CellState.FirstPlayer;
                else 
                    return old_field[i][j];
            }
            return CellState.Die;
        }
    }

    int h;
    int w;
    int steps;
    bool multiplayer;
    List<List<CellState>> field;

    static readonly Vector2Int[] neighbors = new Vector2Int[]{new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int(-1, -1),
                                                              new Vector2Int(0,  1), new Vector2Int(0,  -1),
                                                              new Vector2Int(1,  1), new Vector2Int(1,  0), new Vector2Int(1,  -1)};
}
