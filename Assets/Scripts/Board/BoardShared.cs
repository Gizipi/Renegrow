using System;
using UnityEngine;

public static class BoardUtility {
	public static bool HasWaterNeighbour(BoardSlot slot)
	{
		foreach (EDirection direction in Enum.GetValues(typeof(EDirection)))
		{
			if (!slot.neighbours.ContainsKey(direction))
				continue;
			GrowTile neighbour = (GrowTile)slot.neighbours[direction];
			if (neighbour.TileData.tileType == ETileType.Water)
				return true;
		}
		return false;
	}
}

public interface IBoard
{
	BoardEvents Events { get; }
	BoardData BoardData { get; }
	BoardSlot[] Slots { get; }
	void SetSlot(BoardSlotPosition position, BoardSlot slot);
	BoardSlot GetSlot(BoardSlotPosition position);
	BoardSlot RemoveSlot(BoardSlotPosition position);
}

