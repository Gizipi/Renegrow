using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class CapacityUi : MonoBehaviour
{
	public Sprite CapacitySprite;
	public GameObject CapacityPrefab;
	public GameObject ResourcePrefab;

	public float OrbitRadius = 0.1f;
	public float MinNeighborAngleDegrees = 14f;

	private UiData _uiData;
	private List<ResourceUiPacket> _resources = new();
	private List<GameObject> _capacity = new();

	public void Initialize(UiData uiData)
	{
		_uiData = uiData;
	}

	public void AddResource(ELeafType leafType)
	{
		GameObject resource = GameObject.Instantiate(ResourcePrefab);

		resource.transform.SetParent(transform);
		ResourceUiPacket resourceUiPacket = new ResourceUiPacket(resource, leafType);
		_resources.Add(resourceUiPacket);
		SpriteRenderer spriteRenderer = resource.GetComponent<SpriteRenderer>();
		if(_uiData.ResourceSpritesDictionary.ContainsKey(leafType))
		{
			spriteRenderer.sprite = _uiData.ResourceSpritesDictionary[leafType];
		}
		else
		{
			Debug.LogError("Resource sprite not found for leaf type: " + leafType);
		}
		Debug.Log("Adding resource: " + leafType + ", current Resource count: " + _resources.Count + ", capacity count: " + _capacity.Count);
		resource.transform.localPosition = _capacity[_resources.Count - 1].transform.localPosition;
	}

	public void RemoveResource(ELeafType leafType)
	{
		if (_resources.Count <= 0)
			return;

		int count = _resources.Count;
		int removeIndex = -1;
		for (int i = count - 1; i >= 0; i--)
		{
			if (_resources[i].leafType == leafType)
			{
				removeIndex = i;
				break;
			}
		}
		if (removeIndex != -1)
		{
			ResourceUiPacket resourceUiPacket = _resources[removeIndex];
			Destroy(resourceUiPacket.resourceObject);
			_resources.RemoveAt(removeIndex);
		}
		UpdatePositions();
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
			for (int i = 0; i < math.abs(difference); i++)
			{
				GameObject capacityObject = _capacity[0];
				_capacity.Remove(capacityObject);
				Destroy(capacityObject);
			}
		}
		UpdatePositions();
	}

	private void UpdatePositions()
	{
		int n = _capacity.Count;
		for (int i = 0; i < n; i++)
		{
			Vector3 pos = GetCapacityLocalPosition(i, n);
			_capacity[i].transform.localPosition = pos;
			if (i < _resources.Count)
				_resources[i].resourceObject.transform.localPosition = pos;
		}
	}

	private Vector3 GetCapacityLocalPosition(int index, int count)
	{
		if (count <= 0)
			return Vector3.zero;

		float angle = GetOrbitAngleRadians(index, count);
		float r = OrbitRadius;
		return new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0f);
	}

	private float GetOrbitAngleRadians(int index, int count)
	{
		if (count == 1)
			return 1.5f * Mathf.PI;

		float minStepRad = MinNeighborAngleDegrees * Mathf.Deg2Rad;
		bool useFullCircle = count > 1 && (Mathf.PI / (count - 1f)) < minStepRad;

		if (useFullCircle)
			return Mathf.PI + (2f * Mathf.PI * index) / count;

		return Mathf.PI + (Mathf.PI * index) / (count - 1f);
	}
}
