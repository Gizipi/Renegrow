using UnityEngine;
using System.Collections.Generic;

public class Board 
{
    private Dictionary<int, Dictionary<int, BoardSlot>> _slots = new();
    private MatchEvents _events;
    private BoardData _boardData;

    public Board(BoardSlot[] slots, BoardData boardData, MatchEvents events)
    {
        _boardData = boardData;
        _events = events;
        SubscribeToEvents();
        foreach (BoardSlot slot in slots)
        {
            SetSlot(slot.position, slot);
        }
    }

    private void SubscribeToEvents()
    {
        _events.onSeasonChange += OnSeasonChange;
    }

    private void OnSeasonChange(ESeason season)
    {

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
        foreach (BoardSlot slot in slots)
        {
            SetSlot(slot.position, slot);
            slot.Added();
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
    }

    public BoardSlot RemoveSlot(BoardSlotPosition position)
    {
        BoardSlot slot = _slots[position.x][position.y];
        _slots[position.x].Remove(position.y);
        if (_slots[position.x].Count == 0)
        {
            _slots.Remove(position.x);
        }
        slot.Removed();
        return slot;
    }
}
