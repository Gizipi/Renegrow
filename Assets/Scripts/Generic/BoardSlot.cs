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

    public BoardSlotPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
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
    protected readonly GameObject _slotVisual;
    protected readonly BoardData _boardData;
    protected Dictionary<EDirection, BoardSlot> _neighbours;
    public Dictionary<EDirection, BoardSlot> neighbours
    {
        get
        {
            return _neighbours;
        }
    }
    
    public BoardSlot(GameObject slotVisual, BoardData boardData)
    {
        _slotVisual = slotVisual;
        _boardData = boardData;
        _neighbours = new Dictionary<EDirection, BoardSlot>();
    }
    

    public virtual void SetPosition(BoardSlotPosition position)
    {
        _position = position;
        if (_slotVisual != null && _boardData != null)
            _slotVisual.transform.position = _boardData.GridToWorld(position);
    }

    public virtual void SetNeighbour(EDirection direction, BoardSlot neighbour)
    {
        _neighbours[direction] = neighbour;
    }

    public virtual BoardSlot RemoveNeighbour(EDirection direction)
    {
        BoardSlot neighbour = _neighbours[direction];
        _neighbours.Remove(direction);
        return neighbour;
    }
    
    public virtual void Added()
    {
        _slotVisual.SetActive(true);
    }

    public virtual void Removed()
    {
        _slotVisual.SetActive(false);
    }

    public virtual void OnClicked()
    {

    }

    public virtual void OnHoverStart()
    {
        
    }

    public virtual void OnHoverEnd()
    {
        
    }
}
