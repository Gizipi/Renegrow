using UnityEngine;

public class BoardEvents
{
	public delegate void OnTileAdded(BoardSlot slot);
	public event OnTileAdded onTileAdded;
	public void TileAdded(BoardSlot slot)
	{
		if (onTileAdded != null)
			onTileAdded(slot);
	}

	public delegate void OnTileRemoved(BoardSlot slot);
	public event OnTileRemoved onTileRemoved;
	public void TileRemoved(BoardSlot slot)
	{
		if (onTileRemoved != null)
			onTileRemoved(slot);
	}
}
