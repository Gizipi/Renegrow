using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "TileCountWinCondition", menuName = "WinCondition/TileCount")]
public class TileCountWinCondition : WinCondition
{
	public ETileType tileType;
	public int requiredTileCount;
	[NonSerialized]
	public List<GrowTile> correctTiles = new();

	public override void Initialize(IBoard board)
	{
		if (_board != null)
		{
			_board.Events.onTileAdded -= (slot) => OnTileAdded((GrowTile)slot);
			_board.Events.onTileRemoved -= (slot) => OnTileRemoved((GrowTile)slot);
		}

		base.Initialize(board);
		foreach (GrowTile slot in _board.Slots)
		{
			OnTileAdded(slot);
		}
		_board.Events.onTileAdded += (slot) => OnTileAdded((GrowTile)slot);
		_board.Events.onTileRemoved += (slot) => OnTileRemoved((GrowTile)slot);
	}

	private void OnTileAdded(GrowTile slot)
	{
		slot.events.onTileDataChanged += (tileData) => OnTileDataChanged(tileData, slot);
		slot.events.onTileRemoved += () => OnTileRemoved(slot);
		OnTileDataChanged(slot.TileData, slot);
	}

	private void OnTileRemoved(GrowTile slot)
	{
		slot.events.onTileDataChanged -= (tileData) => OnTileDataChanged(tileData, slot);
		slot.events.onTileRemoved -= () => OnTileRemoved(slot);
		if (correctTiles.Contains(slot))
		{
			correctTiles.Remove(slot);
		}
	}

	private void OnTileDataChanged(TileData tileData, GrowTile slot)
	{
		if (correctTiles.Contains(slot))
		{
			if (tileData.tileType != tileType)
			{
				correctTiles.Remove(slot);
			}
		}
		else if (tileData.tileType == tileType)
		{
			correctTiles.Add(slot);
		}
		else
		{
			return;
		}

		float progress = Mathf.Clamp01((float)correctTiles.Count / requiredTileCount);
		ProgressChanged(progress);
	}
}
