using System;
using Unity.Mathematics;
using UnityEngine;

/// <summary>How axial coordinates (q, r) map to the X/Y plane.</summary>
public enum HexOrientation
{
	/// <summary>Hexagon has a vertex at the top; common for "column" layouts.</summary>
	PointyTop,
	/// <summary>Hexagon has a flat edge at the top; common for "row" layouts.</summary>
	FlatTop,
}

[CreateAssetMenu(fileName = "BoardData", menuName = "BoardData")]
public class BoardData : ScriptableObject
{
	public Vector3 center;

	[Tooltip("Circumradius: distance from hex center to any vertex.")]
	public float radius = 1f;

	public HexOrientation orientation = HexOrientation.FlatTop;

	/// <summary>
	/// World position for a slot. Grid (x, y) is odd-r offset (column, row), not raw axial—same convention as <c>BoardGeneration.ConnectHexNeighbours</c>.
	/// </summary>
	public Vector3 GridToWorld(BoardSlotPosition grid)
	{
		return GridToWorld(grid.x, grid.y);
	}

	public Vector3 GridToWorld(int q, int r)
	{
		float sqrt3 = Mathf.Sqrt(3f);
		float x;
		float y;
		int modifiedR = r - (int)math.floor(q*0.5f);
		if (orientation == HexOrientation.PointyTop)
		{
			x = radius * sqrt3 * (q + modifiedR * 0.5f);
			y = radius * 1.5f * modifiedR;
		}
		else
		{
			x = radius * 1.5f * q;
			y = radius * sqrt3 * (modifiedR + q * 0.5f);
		}

		return center + new Vector3(x, y, 0f);
	}

	public BoardSlotPosition WorldToGrid(Vector3 world)
	{
		float x = world.x - center.x;
		float y = world.y - center.y;
		float sqrt3 = Mathf.Sqrt(3f);
		float q = x / (radius * sqrt3);
		float r = y / (radius * 1.5f);
		int modifiedR = (int)Math.Floor(r + (int)math.floor(q*0.5f));
		return new BoardSlotPosition((int)math.floor(q), modifiedR);
	}
}
