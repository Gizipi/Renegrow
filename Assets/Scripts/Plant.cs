using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plant
{
	private readonly PlantData _plantData;
	public PlantData PlantData
	{
		get
		{
			return _plantData;
		}
	}

	public bool IsAtCapacity
	{
		get
		{
			if (_plantData.growthStages.ContainsKey(_growthStage))
			{
				return _leaves.Count >= _plantData.growthStages[_growthStage].capacity;
			}
			return true;
		}
	}

	private readonly List<ELeafType> _leaves = new();
	private EPlantGrowthStage _growthStage = EPlantGrowthStage.Seed;
	private PlantGrowthStage _previousGrowthStage;
	private readonly GameObject _plantVisual;
	public GameObject PlantVisual
	{
		get
		{
			return _plantVisual;
		}
	}
	private readonly SpriteRenderer _plantSpriteRenderer;
	private readonly CapacityUi _capacityParent;
	private ESeason _currentSeason = ESeason.Spring;

	public Plant(UiData uiData, PlantData plantData)
	{
		try
		{
			_capacityParent = GameObject.Instantiate(uiData.CapacityUiPrefab).GetComponent<CapacityUi>();
		}
		catch (Exception e)
		{
			Debug.LogError("Error instantiating capacity parent: " + e.Message);
		}
		GameObject plantVisual = GameObject.Instantiate(plantData.plantVisualPrefab);
		_plantData = plantData;
		_plantVisual = plantVisual;
		_plantSpriteRenderer = _plantVisual.GetComponent<SpriteRenderer>();
	}

	public void Seed()
	{
		Debug.Log("Seed");
		_previousGrowthStage = _plantData.growthStages[_growthStage];
		_growthStage = EPlantGrowthStage.Seed;
		SetSprite();
	}

	public void ChangeSeason(ESeason season)
	{
		_currentSeason = season;
		if (season == ESeason.Spring)
		{
			Grow();
		}
		Produce(season);
		SetSprite();
	}

	private void SetSprite()
	{
		if (_growthStage == EPlantGrowthStage.Dead)
		{
			_plantSpriteRenderer.sprite = _plantData.growthStages[_previousGrowthStage.growthStage].deadVisual;
			return;
		}
		_plantSpriteRenderer.sprite = _plantData.growthStages[_growthStage].livingVisual;
		Debug.Log("Set sprite " + _plantSpriteRenderer.sprite.name);
	}

	protected void Produce(ESeason season)
	{
		if (IsAtCapacity)
			return;
		PlantGrowthStage growthStage = _plantData.growthStages[_growthStage];
		if (!growthStage.productionSeasons.Contains(season))
			return;

		foreach (LeafGroup production in growthStage.production)
		{
			for (int i = 0; i < production.count; i++)
			{
				bool added = AddLeaf(production.type);
				if (!added)
					return;
			}
		}
	}

	public bool AddLeaf(ELeafType leafType)
	{
		if (IsAtCapacity)
			return false;

		_leaves.Add(leafType);
		return true;
	}

	public void Grow()
	{
		if (_growthStage == EPlantGrowthStage.Dead || _growthStage == EPlantGrowthStage.Mature)
			return;

		_growthStage++;
		SetSprite();
	}

	public void Die()
	{
		_previousGrowthStage = _plantData.growthStages[_growthStage];
		_growthStage = EPlantGrowthStage.Dead;
		_leaves.Clear();
		SetSprite();
		SetCapacity();
	}

	public void OnClicked()
	{
		Debug.Log("OnClicked");
	}

	public void OnHoverStart()
	{
		Debug.Log("OnHover");
	}

	public void OnHoverEnd()
	{
		Debug.Log("OnHoverEnd");
	}

	private void SetCapacity()
	{
		int capacity = _plantData.growthStages[_growthStage].capacity;
		_capacityParent.SetCapacity(capacity);
		if (capacity < _leaves.Count)
		{
			for (int i = 0; i < _leaves.Count - capacity; i++)
			{
				_capacityParent.RemoveResource(_leaves[i]);
				_leaves.RemoveAt(i);
			}
		}
	}
}
