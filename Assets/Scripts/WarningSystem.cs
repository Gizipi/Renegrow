using System.Collections.Generic;
using UnityEngine;

public interface IWarningCondition
{
    public void ShowWarning();
    public bool CheckWarningCondition(GrowTile slot);
    public void HideWarning();
}

public class WarningSystem
{
    private List<IWarningCondition> _warningConditions = new();

    public WarningSystem(Board board, List<IWarningCondition> warningConditions)
    {
        _warningConditions = warningConditions;
        foreach(BoardSlot slot in board.Slots)
        {
            OnTileAdded(slot);
        }
        board.Events.onTileAdded += OnTileAdded;
        board.Events.onTileRemoved += OnTileRemoved;
    }

    private void OnTileAdded(BoardSlot slot)
    {
        slot.spriteButtonEvents.onHoverStart += () => OnHoverStart(slot);
        slot.spriteButtonEvents.onHoverEnd += () => OnHoverEnd(slot);
    }

    private void OnTileRemoved(BoardSlot slot)
    {
        slot.spriteButtonEvents.onHoverStart -= () => OnHoverStart(slot);
        slot.spriteButtonEvents.onHoverEnd -= () => OnHoverEnd(slot);
    }

    private void OnHoverStart(BoardSlot slot)
    {
        foreach (IWarningCondition warningCondition in _warningConditions)
        {
            if (warningCondition.CheckWarningCondition((GrowTile)slot))
            {
                warningCondition.ShowWarning();
            }
        }
    }

    private void OnHoverEnd(BoardSlot slot)
    {
        foreach (IWarningCondition warningCondition in _warningConditions)
        {
            warningCondition.HideWarning();
        }
    }

}
