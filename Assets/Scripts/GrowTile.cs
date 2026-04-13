using UnityEngine;
using static SeasonEvents;
using System;

public class GrowTile : BoardSlot
{
	private Plant _plant;
	public Plant Plant
	{
		get
		{
			return _plant;
		}
	}
	private readonly CapacityUi _capacityParent;
	private readonly SpriteRenderer _tileSpriteRenderer;
	private TileData _tileData;
	public TileData TileData
	{
		get
		{
			return _tileData;
		}
	}
	private readonly UiData _uiData;
	private SeasonEvents _matchEvents;
	private ESeason _currentSeason = ESeason.Spring;
	private RangeBehaviour _rangeBehaviour;

	public GrowTile(GameObject tileVisual, UiData uiData, BoardData boardData, TileData tileData, RangeBehaviour rangeBehaviour) : base(tileVisual, boardData)
	{
		_tileData = tileData;
		_uiData = uiData;

		_tileSpriteRenderer = _slotVisual.GetComponent<SpriteRenderer>();
		_rangeBehaviour = rangeBehaviour;
	}

	public void ProvideEvents(SeasonEvents events)
	{
		Debug.Log("GrowTile: ProvideEvents");
		_matchEvents = events;
		SubscribeToEvents();
	}

	private void SubscribeToEvents()
	{
		_matchEvents.onSeasonChange += OnSeasonChange;
	}

	private void OnSeasonChange(ESeason season)
	{
		Debug.Log("GrowTile: OnSeasonChange to " + season);
		ChangeSeason(season);
	}

	public void ChangeSeason(ESeason season)
	{
		_tileSpriteRenderer.sprite = _tileData.tileSprites[season];
		_currentSeason = season;
		if (_plant != null)
			_plant.ChangeSeason(season);
	}

	public void Seed(Plant plant)
	{
		if (_tileData.tileType == ETileType.dirt)
			return;

		if (_plant != null)
		{
			TransferLeaf(plant);
			return;
		}

		_plant = new Plant(_uiData, plant.PlantData);
		if (plant.Leaves.Count > 0)
		{
			plant.RemoveLeaf(plant.Leaves[0]);
		}
		_plant.Seed();
		SetPlantPosition();
	}

	public void Seed(PlantData plantData)
	{
		_plant = new Plant(_uiData, plantData);
		_plant.Seed();
		SetPlantPosition();
	}

	public void Spread()
	{
		Debug.Log("attempting to spread");
		if (_plant == null)
			return;
		if (_plant.Leaves.Count <= 0)
			return;

		Debug.Log("spreading");
		foreach (EDirection direction in Enum.GetValues(typeof(EDirection)))
		{
			if (!neighbours.ContainsKey(direction))
				continue;
			GrowTile neighbour = (GrowTile)neighbours[direction];
			if (neighbour == null)
				continue;

			if (_tileData.spreadableTiles.Contains(neighbour.TileData.tileType))
			{
				neighbour.SetTileData(_tileData);
			}
		}
		_plant.RemoveLeaf(_plant.Leaves[0]);
	}

	public void SetTileData(TileData tileData)
	{
		_tileData = tileData;
		_tileSpriteRenderer.sprite = _tileData.tileSprites[_currentSeason];
	}

	public void TransferLeaf(Plant plant)
	{
		if (_plant.Leaves.Count >= _plant.CurrentGrowthStage.capacity || plant.Leaves.Count <= 0)
		{
			return;
		}

		ELeafType leafType = plant.Leaves[0];
		plant.RemoveLeaf(leafType);
		_plant.AddLeaf(leafType);
	}

	public override void SetPosition(BoardSlotPosition position)
	{
		base.SetPosition(position);
		SetPlantPosition();
	}

	private void SetPlantPosition()
	{
		if (_plant != null)
			_plant.SetPosition(_boardData.GridToWorld(position));
	}

	public override void Added()
	{
		base.Added();
		SetPlantPosition();
	}

	public override void Removed()
	{
		base.Removed();
		_plant = null;
	}

	public override void OnClicked()
	{
		if (_plant != null)
		{
			_plant.OnClicked();
		}
		_rangeBehaviour.ProvideTile(this);
	}

	public override void OnHoverStart()
	{
		_tileSpriteRenderer.color = new Color(1, 1, 1, 0.5f);
		if (_plant != null)
		{
			_plant.OnHoverStart();
		}
	}

	public override void OnHoverEnd()
	{
		_tileSpriteRenderer.color = new Color(1, 1, 1, 1f);
		if (_plant != null)
		{
			_plant.OnHoverEnd();
		}
	}
}
