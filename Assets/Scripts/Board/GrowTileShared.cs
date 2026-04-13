using UnityEngine;

public enum ETileType
{
	dirt,
	Grass,
	Rock,
	Water,
	Sand,
}

[System.Serializable]
public class TileGenerationData
{
	[SerializeField]
	public TileData tileData;
	public int count;
}