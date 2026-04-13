using UnityEngine;
using System.Collections.Generic;

public class Board : IBoard
{
    private readonly BoardEvents _events = new();
    public BoardEvents Events => _events;
    private readonly BoardData _boardData;
    public BoardData BoardData => _boardData;
    private readonly Dictionary<int, Dictionary<int, BoardSlot>> _slots = new();
    private readonly List<BoardSlot> _slotsList = new();
    public BoardSlot[] Slots => _slotsList.ToArray();

    public Board(BoardData boardData)
    {
        _boardData = boardData;
    }

    public void ReplaceAllSlots(BoardSlot[] slots)
    {
        foreach (Dictionary<int, BoardSlot> line in _slots.Values)
        {
            foreach (BoardSlot slot in line.Values)
            {
                slot.Removed();
            }
        }
        _slots.Clear();
        _slotsList.Clear();
        SetSlots(slots);
    }

    private void SetSlots(BoardSlot[] slots)
    {
        foreach (BoardSlot slot in slots)
        {
            SetSlot(slot.position, slot);
        }
    }

    public BoardSlot GetSlot(BoardSlotPosition position)
    {
        return _slots[position.x][position.y];
    }

    public void SetSlot(BoardSlotPosition position, BoardSlot slot)
    {
        if (!_slots.ContainsKey(position.x))
        {
            _slots[position.x] = new Dictionary<int, BoardSlot>();
        }
        _slots[position.x][position.y] = slot;
        _slotsList.Add(slot);
        _events.TileAdded(slot);
    }

    public BoardSlot RemoveSlot(BoardSlotPosition position)
    {
        BoardSlot slot = _slots[position.x][position.y];
        _slots[position.x].Remove(position.y);
        if (_slots[position.x].Count == 0)
        {
            _slots.Remove(position.x);
        }
        _slotsList.Remove(slot);
        slot.Removed();
        _events.TileRemoved(slot);
        return slot;
    }
}
