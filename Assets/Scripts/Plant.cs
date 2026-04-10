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
				return _leaves.Count >= CurrentGrowthStage.capacity;
			}
			return true;
		}
	}

	private readonly List<ELeafType> _leaves = new();
	public List<ELeafType> Leaves
	{
		get
		{
			return _leaves;
		}
	}
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
	public PlantGrowthStage CurrentGrowthStage
	{
		get
		{
			return _plantData.growthStages[_growthStage];
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
		_capacityParent.transform.SetParent(plantVisual.transform);
		_plantData = plantData;
		_plantVisual = plantVisual;
		_plantSpriteRenderer = _plantVisual.GetComponent<SpriteRenderer>();
	}

	public void SetPosition(Vector3 position)
	{
		if (_plantVisual == null || _capacityParent == null)
			return;
		_plantVisual.transform.position = position;
		_capacityParent.transform.position = position;
	}

	public void Seed()
	{
		Debug.Log("Seed");
		_previousGrowthStage = CurrentGrowthStage;
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
		_plantSpriteRenderer.sprite = CurrentGrowthStage.livingVisual;
		Debug.Log("Set sprite " + _plantSpriteRenderer.sprite.name);
	}

	protected void Produce(ESeason season)
	{
		if (IsAtCapacity)
			return;
		PlantGrowthStage growthStage = CurrentGrowthStage;
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

	public void RemoveLeaf(ELeafType leafType)
	{
		_leaves.Remove(leafType);
		_capacityParent.RemoveResource(leafType);

		Debug.Log("Removed leaf " + leafType + " from plant " + _plantData.name + " " + _leaves.Count);
	}

	public bool AddLeaf(ELeafType leafType)
	{
		if (IsAtCapacity)
			return false;

		_leaves.Add(leafType);
		_capacityParent.AddResource(leafType);
		Debug.Log("Added leaf " + leafType + " to plant " + _plantData.name + " " + _leaves.Count);
		if (_growthStage == EPlantGrowthStage.Dead)
			Revive();
		return true;
	}

	public void Grow()
	{
		if (_growthStage == EPlantGrowthStage.Dead || _growthStage == EPlantGrowthStage.Mature)
			return;

		_growthStage++;
		SetSprite();
	}

	public void Revive()
	{
		_previousGrowthStage = CurrentGrowthStage;
		_growthStage = EPlantGrowthStage.Seed;
		SetSprite();
		SetCapacity();
	}

	public void Die()
	{
		_previousGrowthStage = CurrentGrowthStage;
		_growthStage = EPlantGrowthStage.Dead;
		_leaves.Clear();
		SetSprite();
		SetCapacity();
	}

	public void OnClicked()
	{
		Debug.Log("OnClicked plant");
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
		int capacity = CurrentGrowthStage.capacity;
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
