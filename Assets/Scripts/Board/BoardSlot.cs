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

public abstract class BoardSlot : ISpriteButton
{
    public SpriteButtonEvents spriteButtonEvents { get; } = new();
    private readonly SpriteButton _spriteButton;
    private BoardSlotPosition _position;
    public BoardSlotPosition position => _position;
    protected readonly GameObject _slotVisual;
    protected readonly BoardData _boardData;
    protected Dictionary<EDirection, BoardSlot> _neighbours;
    public Dictionary<EDirection, BoardSlot> neighbours => _neighbours;
    
    public BoardSlot(GameObject slotVisual, BoardData boardData)
    {
        _slotVisual = slotVisual;
        _boardData = boardData;
        _spriteButton = slotVisual.GetComponent<SpriteButton>();
        _neighbours = new Dictionary<EDirection, BoardSlot>();
        SubscribeToEvents();
    }
    
    private void SubscribeToEvents()
    {
        if (_spriteButton == null)
            return;
        _spriteButton.spriteButtonEvents.onClick += OnClicked;
        _spriteButton.spriteButtonEvents.onHoverStart += OnHoverStart;
        _spriteButton.spriteButtonEvents.onHoverEnd += OnHoverEnd;
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
        GameObject.Destroy(_slotVisual);
    }

    public virtual void OnClicked()
    {
        spriteButtonEvents.Click();
    }

    public virtual void OnHoverStart()
    {
        spriteButtonEvents.HoverStart();
    }

    public virtual void OnHoverEnd()
    {
        spriteButtonEvents.HoverEnd();
    }
}
