using System.Collections.Generic;
using UnityEngine;

public class GrowTileEvents
{
	public delegate void OnTileDataChanged(TileData tileData);
	public event OnTileDataChanged onTileDataChanged;
	public void TileDataChanged(TileData tileData)
	{
		if (onTileDataChanged != null)
			onTileDataChanged(tileData);
	}

	public delegate void OnPlantSeeded(Plant plant);
	public event OnPlantSeeded onPlantSeeded;
	public void PlantSeeded(Plant plant)
	{
		if (onPlantSeeded != null)
			onPlantSeeded(plant);
	}

	public delegate void OnSpread();
	public event OnSpread onSpread;
	public void Spread()
	{
		if (onSpread != null)
			onSpread();
	}

	public delegate void OnTileRemoved();
	public event OnTileRemoved onTileRemoved;
	public void TileRemoved()
	{
		if (onTileRemoved != null)
			onTileRemoved();
	}

	public delegate void OnTileSelected(GrowTile growTile);
	public event OnTileSelected onTileSelected;
	public void TileSelected(GrowTile growTile)
	{
		if (onTileSelected != null)
			onTileSelected(growTile);
	}
}
