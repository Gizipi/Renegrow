using UnityEngine;

public enum ETileAnimation
{
	idle,
	spread,
	setTileData,
}

public class TileAnimationPackage
{
	public ETileAnimation animation = ETileAnimation.idle;
	public GrowTile tile;
	public int frameIndex = 0;
	
	public TileAnimationPackage(GrowTile tile)
	{
		this.tile = tile;
	}
}
