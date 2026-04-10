using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class BoardGeneration
{
	/// <summary>Axial (q, r) hex neighbors; must stay in sync with <see cref="BoardData.GridToWorld"/>.</summary>
	private static readonly (int dq, int dr, EDirection dir)[] HexAxialNeighbours =
	{
		(1, 0, EDirection.upRight),
		(1, -1, EDirection.up),
		(0, -1, EDirection.upLeft),
		(-1, 0, EDirection.downLeft),
		(-1, 1, EDirection.down),
		(0, 1, EDirection.downRight),
	};
	public static Board GenerateBoard(CoreData coreData)
	{
		Board board = new Board(GenerateSlots(coreData), coreData.boardData, coreData.matchEvents);
		return board;
	}

	private static BoardSlot[] GenerateSlots(CoreData coreData)
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

		int[] plantIndexes = GeneratePlantIndexes(coreData.boardGenerationData.deadPlantCount, slotCount);
		RangeBehaviour rangeBehaviour = new RangeBehaviour();

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
			slot.ProvideEvents(coreData.matchEvents);
			slot.SetPosition(position);
			slots.Add(slot);
			tileCounts[tileData.tileType]++;
			if (tileData.tileType == ETileType.Grass)
			{
				Debug.Log("Seed grass");
				slot.Seed(new Plant(coreData.uiData, coreData.boardGenerationData.plants[Random.Range(0, coreData.boardGenerationData.plants.Length - 1)]));
				slot.Plant.Grow();
				slot.Plant.Grow();
			}
			else if (plantIndexes.Contains(i))
			{
				Debug.Log("Seed plant");
				slot.Seed(new Plant(coreData.uiData, coreData.boardGenerationData.plants[Random.Range(0, coreData.boardGenerationData.plants.Length - 1)]));
				slot.Plant.Grow();
				slot.Plant.Die();
			}
		}

		ConnectHexNeighbours(slots);
		return slots.ToArray();
	}

	private static void ConnectHexNeighbours(List<GrowTile> slots)
	{
		var byAxial = new Dictionary<(int q, int r), GrowTile>(slots.Count);
		foreach (GrowTile slot in slots)
		{
			BoardSlotPosition p = slot.position;
			byAxial[(p.x, p.y)] = slot;
		}

		foreach (GrowTile slot in slots)
		{
			BoardSlotPosition p = slot.position;
			foreach ((int dq, int dr, EDirection dir) in HexAxialNeighbours)
			{
				if (byAxial.TryGetValue((p.x + dq, p.y + dr), out GrowTile neighbour))
					slot.SetNeighbour(dir, neighbour);
			}
		}
	}

	private static int[] GeneratePlantIndexes(int deadPlantCount, int tileCount)
	{
		Debug.Log("Generating plant indexes " + deadPlantCount + " " + tileCount);
		List<int> plantIndexes = new List<int>();
		for (int i = 0; i < deadPlantCount + 1; i++)
		{
			int randomIndex = Random.Range(0, tileCount);
			Debug.Log("Generate plant index " + i + " " + randomIndex);
			plantIndexes.Add(randomIndex);
		}
		return plantIndexes.ToArray();
	}
}
