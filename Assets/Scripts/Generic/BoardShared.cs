using UnityEngine;


public interface IBoard
{
	BoardEvents Events { get; }
	BoardData BoardData { get; }
	BoardSlot[] Slots { get; }
	void SetSlot(BoardSlotPosition position, BoardSlot slot);
	BoardSlot GetSlot(BoardSlotPosition position);
	BoardSlot RemoveSlot(BoardSlotPosition position);
}