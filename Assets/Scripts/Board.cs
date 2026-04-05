using UnityEngine;
using System.Collections.Generic;

public abstract class Board<TBoardSlot> where TBoardSlot : BoardSlot
{
    private Dictionary<int, Dictionary<int, TBoardSlot>> _slots;

    public Board(TBoardSlot[] slots)
    {
        foreach (TBoardSlot slot in slots)
        {
            setSlot(slot.position, slot);
        }
    }

    public void replaceAllSlots(TBoardSlot[] slots)
    {
        foreach (Dictionary<int, TBoardSlot> line in _slots.Values)
        {
            foreach (TBoardSlot slot in line.Values)
            {
                slot.removed();
            }
        }
        _slots.Clear();
        foreach (TBoardSlot slot in slots)
        {
            setSlot(slot.position, slot);
            slot.added();
        }
    }

    public TBoardSlot getSlot(BoardSlotPosition position)
    {
        return _slots[position.x][position.y];
    }

    public void setSlot(BoardSlotPosition position, TBoardSlot slot)
    {
        if (!_slots.ContainsKey(position.x))
        {
            _slots[position.x] = new Dictionary<int, TBoardSlot>();
        }
        _slots[position.x][position.y] = slot;
    }

    public TBoardSlot removeSlot(BoardSlotPosition position)
    {
        TBoardSlot slot = _slots[position.x][position.y];
        _slots[position.x].Remove(position.y);
        if (_slots[position.x].Count == 0)
        {
            _slots.Remove(position.x);
        }
        slot.removed();
        return slot;
    }
}
