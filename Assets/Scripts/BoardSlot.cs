using UnityEngine;
using System.Collections.Generic;

public enum EDirection
{
    up,
    down,
    upLeft,
    upRight,
    downLeft,
    downRight,
}

public class BoardSlotPosition
{
    public int x;
    public int y;
}

public abstract class BoardSlot
{
    private BoardSlotPosition _position;
    public BoardSlotPosition position
    {
        get
        {
            return _position;
        }
    }
    protected Dictionary<EDirection, BoardSlot> _neighbours;
    public Dictionary<EDirection, BoardSlot> neighbours
    {
        get
        {
            return _neighbours;
        }
    }

    public virtual void setPosition(BoardSlotPosition position)
    {
        _position = position;
    }

    public virtual void setNeighbour(EDirection direction, BoardSlot neighbour)
    {
        _neighbours[direction] = neighbour;
    }

    public virtual BoardSlot removeNeighbour(EDirection direction)
    {
        BoardSlot neighbour = _neighbours[direction];
        _neighbours.Remove(direction);
        return neighbour;
    }

    public abstract void added();
    public abstract void removed();
}
