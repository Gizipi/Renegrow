using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.Linq;

public static class BoardGeneration
{
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

		for (int i = 0; i < slotCount; i++)
		{
			int randomPositionIndex = Random.Range(0, positions.Count);
			BoardSlotPosition position = positions[randomPositionIndex];
			positions.RemoveAt(randomPositionIndex);
			int randomIndex = Random.Range(0, tileDatas.Count - 1);
			TileData tileData = tileDatas[randomIndex];
			tileDatas.RemoveAt(randomIndex);
			GameObject tileVisual = GameObject.Instantiate(coreData.boardGenerationData.tileVisualPrefab);
			GrowTile slot = new GrowTile(tileVisual, coreData.uiData, coreData.boardData, tileData);
			slot.ProvideEvents(coreData.matchEvents);
			slot.SetPosition(position);
			slots.Add(slot);
			tileCounts[tileData.tileType]++;
			if(tileData.tileType == ETileType.Grass)
			{
				Debug.Log("Seed grass");
				slot.Seed(coreData.boardGenerationData.plants[Random.Range(0, coreData.boardGenerationData.plants.Length - 1)]);
				slot.Plant.Grow();
				slot.Plant.Grow();
			}
			else if (plantIndexes.Contains(i))
			{
				Debug.Log("Seed plant");
				slot.Seed(coreData.boardGenerationData.plants[Random.Range(0, coreData.boardGenerationData.plants.Length - 1)]);
				slot.Plant.Grow();
				slot.Plant.Die();
			}
		}

		return slots.ToArray();
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
