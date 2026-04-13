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
			return _leaves.Count >= CurrentGrowthStage.capacity;
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
	private EPlantGrowthStage _growthStage = EPlantGrowthStage.Seedling;
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
	private bool _isDead = false;
	public bool IsDead
	{
		get
		{
			return _isDead;
		}
	}

	public Plant(UiData uiData, PlantData plantData)
	{
		try
		{
			_capacityParent = GameObject.Instantiate(uiData.CapacityUiPrefab).GetComponent<CapacityUi>();
			_capacityParent.Initialize(uiData);
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
		SetCapacity();
		_capacityParent.gameObject.SetActive(false);
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
		_growthStage = EPlantGrowthStage.Seedling;
		SetSprite();
	}

	public void ChangeSeason(ESeason season)
	{
		Debug.Log("Plant: ChangeSeason to " + season);
		_currentSeason = season;
		if (season == ESeason.Spring)
		{
			Grow();
		}
		Produce(season);
		SetSprite();
	}

	private void EndureColdSeason()
	{
		if(_isDead)
			return;
		int requiredLeaves = Mathf.Max(1, Mathf.FloorToInt(CurrentGrowthStage.capacity * 0.5f));
		if (_leaves.Count < requiredLeaves)
		{
			Die();
		}
		else
		{
			for (int i = 0; i < requiredLeaves; i++)
			{
				RemoveLeaf();
			}
		}
	}

	private void SetSprite()
	{
		if (_isDead)
		{
			_plantSpriteRenderer.sprite = CurrentGrowthStage.deadVisual;
			return;
		}
		_plantSpriteRenderer.sprite = CurrentGrowthStage.livingVisual;
	}

	protected void Produce(ESeason season)
	{
		if(_isDead)
			return;
		if (IsAtCapacity)
			return;
		PlantGrowthStage growthStage = CurrentGrowthStage;
		if (!growthStage.productionSeasons.Contains(season))
			return;

		Debug.Log("Plant: Producing " + growthStage.production.Length + " leaves for " + season);
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

	public void RemoveLeaf()
	{
		RemoveLeaf(_leaves[0]);
		_capacityParent.RemoveResource(_leaves[0]);
	}

	public void RemoveLeaf(ELeafType leafType)
	{
		_leaves.Remove(leafType);
		_capacityParent.RemoveResource(leafType);
	}

	public bool AddLeaf(ELeafType leafType)
	{
		if (IsAtCapacity)
			return false;

		_leaves.Add(leafType);
		_capacityParent.AddResource(leafType);
		Debug.Log("Added leaf " + leafType + " to plant " + _plantData.name + " " + _leaves.Count);
		if (_isDead)
			Revive();
		return true;
	}

	public void Grow()
	{
		if (_isDead || _growthStage == EPlantGrowthStage.Mature)
			return;

		_growthStage++;
		SetSprite();
		SetCapacity();
	}

	public void Revive()
	{
		_isDead = false;
		SetSprite();
		SetCapacity();
	}

	public void Die()
	{
		_isDead = true;
		foreach (ELeafType leafType in _leaves)
		{
			_capacityParent.RemoveResource(leafType);
		}
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
		_capacityParent.gameObject.SetActive(true);
	}

	public void OnHoverEnd()
	{
		Debug.Log("OnHoverEnd");
		_capacityParent.gameObject.SetActive(false);
	}

	private void SetCapacity()
	{
		int capacity = CurrentGrowthStage.capacity;
		Debug.Log("Setting capacity: " + capacity + ", current leaves count: " + _leaves.Count + ", at growth stage: " + _growthStage);
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
