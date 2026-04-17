using UnityEngine;
using System.Collections.Generic;
using System;

public class RangeBehaviour
{
	private GrowTile _slot;
	private int _range;
	private List<GrowTile> _targets;
	private TargetDisplay _targetDisplay;

	public RangeBehaviour(TargetDisplay targetDisplay)
	{
		_targetDisplay = targetDisplay;
	}

	public void ProvideTile(GrowTile slot)
	{
		if (_slot == null)
		{
			if (slot.Plant == null)
				return;
			if (slot.Plant.CurrentGrowthStage.range == 0)
				return;
			if (slot.Plant.Leaves.Count == 0)
				return;
			Prepare(slot, slot.Plant.CurrentGrowthStage.range);
		}
		else
		{
			Execute(slot);
		}
	}

	public void Unprepare()
	{
		_targets.Clear();
		_slot = null;
		_range = 0;
	}

	private void Prepare(GrowTile slot, int range)
	{
		Debug.Log("Attempt Prepare");
		if (slot.Plant == null)
		{
			Debug.Log("Prepare: Plant is null");
			return;
		}

		Debug.Log("Prepare " + slot.Plant.PlantData.name + " " + range);
		_slot = slot;
		_range = range;

		_targets = new List<GrowTile>();
		_targets.Add(_slot);  // Add the starting slot
		List<GrowTile> nextTargets = new List<GrowTile>();
		List<GrowTile> newTargets = new List<GrowTile>();
		List<GrowTile> oldTargets = new List<GrowTile>();
		newTargets.Add(_slot);
		for (int i = 0; i < _range; i++)
		{
			foreach (GrowTile target in newTargets)
			{
				foreach (EDirection direction in Enum.GetValues(typeof(EDirection)))
				{
					if (!target.neighbours.ContainsKey(direction))
						continue;

					GrowTile neighbour = (GrowTile)target.neighbours[direction];
					if (neighbour != null && !_targets.Contains(neighbour) && !oldTargets.Contains(neighbour))
					{
						_targets.Add(neighbour);
						nextTargets.Add(neighbour);
					}
				}
			}
			oldTargets.AddRange(newTargets);
			newTargets = nextTargets;
			nextTargets.Clear();
		}
		_targetDisplay.SetTargets(_targets);
	}

	private void Execute(GrowTile targetSlot)
	{
		Debug.Log("Attempt Execute");
		if (!_targets.Contains(targetSlot))
		{
			ClearTargets();
			return;
		}

		if (targetSlot == _slot)
		{
			targetSlot.Spread();
		}
		else
		{
			targetSlot.Seed(_slot.Plant);
		}
		ClearTargets();
	}

	private void ClearTargets()
	{
		Debug.Log("Clearing targets");
		_targets.Clear();
		_slot = null;
		_range = 0;
		_targetDisplay.ClearTargets();
	}
}
