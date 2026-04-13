using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class BoardGeneration
{
	/// <summary>
	/// Axial (dq, dr) steps in cube space (q + r + s = 0). Stored grid (x,y) is odd-r offset (column, row)
	/// matching <see cref="BoardData.GridToWorld"/>; use <see cref="NeighborInStoredGrid"/> to apply these deltas.
	/// </summary>
	private static readonly (int dq, int dr, EDirection dir)[] HexAxialNeighbours =
	{
		(1, 0, EDirection.upRight),
		(1, -1, EDirection.up),
		(0, -1, EDirection.upLeft),
		(-1, 0, EDirection.downLeft),
		(-1, 1, EDirection.down),
		(0, 1, EDirection.downRight),
	};

	private static int AxialRFromStored(int col, int row)
	{
		return row - Mathf.FloorToInt(col * 0.5f);
	}

	private static BoardSlotPosition StoredFromAxial(int qAxial, int rAxial)
	{
		int row = rAxial + Mathf.FloorToInt(qAxial * 0.5f);
		return new BoardSlotPosition(qAxial, row);
	}

	private static BoardSlotPosition NeighborInStoredGrid(BoardSlotPosition p, int dqAxial, int drAxial)
	{
		int rAx = AxialRFromStored(p.x, p.y);
		return StoredFromAxial(p.x + dqAxial, rAx + drAxial);
	}

	public static BoardSlot[] GenerateSlots(CoreData coreData, SeasonData seasonData)
	{
		List<GrowTile> slots = new List<GrowTile>();
		Dictionary<ETileType, int> tileCapacities = new Dictionary<ETileType, int>();
		Dictionary<ETileType, int> tileCounts = new Dictionary<ETileType, int>();
		List<TileData> tileDatas = new List<TileData>();
		List<BoardSlotPosition> positions = new List<BoardSlotPosition>();

		for (int x = 0; x < coreData.boardGenerationData.width; x++)
		{
			for (int y = 0; y < coreData.boardGenerationData.height; y++)
			{
				positions.Add(new BoardSlotPosition(x, y));
			}
		}

		int slotCount = 0;
		foreach (TileGenerationData tile in coreData.boardGenerationData.tiles)
		{
			tileCapacities[tile.tileData.tileType] = tile.count;
			tileCounts[tile.tileData.tileType] = 0;
			for (int i = 0; i < tile.count; i++)
			{
				tileDatas.Add(tile.tileData);
			}
			slotCount += tile.count;
		}

		List<int> plantIndexes = GeneratePlantIndexes(coreData.boardGenerationData.deadPlantCount, slotCount);
		RangeBehaviour rangeBehaviour = new RangeBehaviour();
		bool hasPlantedStartingPlant = false;

		for (int i = 0; i < slotCount; i++)
		{
			int randomPositionIndex = Random.Range(0, positions.Count);
			BoardSlotPosition position = positions[randomPositionIndex];
			positions.RemoveAt(randomPositionIndex);
			int randomIndex = Random.Range(0, tileDatas.Count - 1);
			TileData tileData = tileDatas[randomIndex];
			tileDatas.RemoveAt(randomIndex);
			GameObject tileVisual = GameObject.Instantiate(coreData.boardGenerationData.tileVisualPrefab);
			GrowTile slot = new GrowTile(tileVisual, coreData.uiData, coreData.boardData, tileData, rangeBehaviour);
			slot.ProvideEvents(seasonData.Events);
			slot.SetPosition(position);
			slots.Add(slot);
			tileCounts[tileData.tileType]++;
			if (tileData.tileType == ETileType.Grass && !hasPlantedStartingPlant)
			{
				Debug.Log("Seed grass");
				PlantData plantData = coreData.boardGenerationData.plants[Random.Range(0, coreData.boardGenerationData.plants.Length - 1)];
				slot.Seed(plantData);
				slot.Plant.Grow();
				slot.Plant.Grow();
				slot.Plant.Produce(ESeason.Spring);
				hasPlantedStartingPlant = true;
			}
			else if (plantIndexes.Contains(i))
			{
				Debug.Log("Seed plant");
				if (slot.Plant != null)
				{
					plantIndexes.Remove(i);
					plantIndexes.Add(GetRandomPlantIndex(i, slotCount));
					continue;
				}
				slot.Seed(coreData.boardGenerationData.plants[Random.Range(0, coreData.boardGenerationData.plants.Length - 1)]);
				slot.Plant.Grow();
				slot.Plant.Die();
			}
		}

		ConnectHexNeighbours(slots);
		return slots.ToArray();
	}

	private static void ConnectHexNeighbours(List<GrowTile> slots)
	{
		var byGrid = new Dictionary<(int x, int y), GrowTile>(slots.Count);
		foreach (GrowTile slot in slots)
		{
			BoardSlotPosition p = slot.position;
			byGrid[(p.x, p.y)] = slot;
		}

		foreach (GrowTile slot in slots)
		{
			BoardSlotPosition p = slot.position;
			foreach ((int dq, int dr, EDirection dir) in HexAxialNeighbours)
			{
				BoardSlotPosition n = NeighborInStoredGrid(p, dq, dr);
				if (byGrid.TryGetValue((n.x, n.y), out GrowTile neighbour))
					slot.SetNeighbour(dir, neighbour);
			}
		}
	}

	private static List<int> GeneratePlantIndexes(int deadPlantCount, int tileCount)
	{
		Debug.Log("Generating plant indexes " + deadPlantCount + " " + tileCount);
		List<int> plantIndexes = new List<int>();
		for (int i = 0; i < deadPlantCount + 1; i++)
		{
			int randomIndex = GetRandomPlantIndex(0, tileCount);
			Debug.Log("Generate plant index " + i + " " + randomIndex);
			plantIndexes.Add(randomIndex);
		}
		return plantIndexes;
	}

	private static int GetRandomPlantIndex(int min, int max) {
		return Random.Range(min, max);
	}
}
