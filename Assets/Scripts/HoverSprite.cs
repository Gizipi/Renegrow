using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class CapacityUi : MonoBehaviour
{
	public Sprite CapacitySprite;
	public GameObject CapacityPrefab;
	public GameObject ResourcePrefab;

	private UiData _uiData;
	private Dictionary<ELeafType, List<GameObject>> _resources = new();
	private List<GameObject> _capacity = new();


	public void AddResource(ELeafType leafType)
	{
		if (!_resources.ContainsKey(leafType))
		{
			_resources[leafType] = new List<GameObject>();
		}
		GameObject resource = GameObject.Instantiate(ResourcePrefab);
		resource.transform.SetParent(transform);
		_resources[leafType].Add(resource);
	}

	public void RemoveResource(ELeafType leafType)
	{
		_resources[leafType].RemoveAt(0);
		Destroy(_resources[leafType][0]);
		_resources[leafType].RemoveAt(0);
	}

	public void SetCapacity(int capacity)
	{
		int previousCapacity = _capacity.Count;
		int difference = capacity - previousCapacity;
		if (difference > 0)
		{
			for (int i = 0; i < difference; i++)
			{
				GameObject capacityObject = GameObject.Instantiate(CapacityPrefab);
				capacityObject.transform.SetParent(transform);
				_capacity.Add(capacityObject);
			}
		}
		else if (difference < 0)
		{
			for (int i = 0; i < -difference; i++)
			{
				GameObject capacityObject = _capacity[0];
				_capacity.Remove(capacityObject);
				Destroy(capacityObject);
			}
		}
	}
}
