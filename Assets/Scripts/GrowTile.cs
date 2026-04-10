using UnityEngine;
using static MatchEvents;
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
	private readonly TileData _tileData;
	public TileData TileData
	{
		get
		{
			return _tileData;
		}
	}
	private readonly UiData _uiData;
	private MatchEvents _matchEvents;
	private ESeason _currentSeason = ESeason.Spring;

	public GrowTile(GameObject tileVisual, UiData uiData, BoardData boardData, TileData tileData) : base(tileVisual, boardData)
	{
		_tileData = tileData;
		_uiData = uiData;
		
		_tileSpriteRenderer = _slotVisual.GetComponent<SpriteRenderer>();
	}

	public void ProvideEvents(MatchEvents events)
	{
		_matchEvents = events;
		SubscribeToEvents();
	}

	private void SubscribeToEvents()
	{
		_matchEvents.onSeasonChange += OnSeasonChange;
	}

	private void OnSeasonChange(ESeason season)
	{
		ChangeSeason(season);
	}

	public void ChangeSeason(ESeason season)
	{
		_tileSpriteRenderer.sprite = _tileData.tileSprites[season];
		_currentSeason = season;
		if (_plant != null)
			_plant.ChangeSeason(season);
	}

	public void Seed(PlantData plantData)
	{
		if (_plant != null)
			return;

		_plant = new Plant(_uiData, plantData);
		_plant.Seed();
		SetPlantPosition();
	}

	public override void SetPosition(BoardSlotPosition position)
	{
		base.SetPosition(position);
		SetPlantPosition();
	}

	private void SetPlantPosition()
	{
		if (_plant != null)
			_plant.PlantVisual.transform.position = _boardData.GridToWorld(position);
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
