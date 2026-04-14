using System.Collections.Generic;
using UnityEngine;

public class TileAnimator : IMatchBehaviour
{
	public EMatchState[] ActiveStates => new[] { EMatchState.Playing, EMatchState.Animating };
	private MatchState _gameState;
	private AnimationData _animationData;
	private Board _board;
	private Dictionary<GrowTile, TileAnimationPackage> _tiles = new();
	private List<GrowTile> _actionAnimations = new();
	private SeasonData _seasonData;

	public TileAnimator(MatchState state, AnimationData animationData, Board board, SeasonData seasonData)
	{
		_gameState = state;
		_animationData = animationData;
		_board = board;
		_seasonData = seasonData;
		SubscribeToEvents();
		foreach (GrowTile growTile in _board.Slots)
		{
			OnTileAdded(growTile);
		}
	}

	private void SubscribeToEvents()
	{
		_board.Events.onTileAdded += OnTileAdded;
		_board.Events.onTileRemoved += OnTileRemoved;
	}

	private void OnTileAdded(BoardSlot slot)
	{
		GrowTile growTile = (GrowTile)slot;
		TileAnimationPackage animation = new TileAnimationPackage(growTile);
		_tiles.Add(growTile, animation);
		growTile.events.onTileDataChanged += (tileData) => OnTileDataChanged(growTile);
		growTile.events.onSpread += () => OnSpread(growTile);
	}

	private void OnTileRemoved(BoardSlot slot)
	{
		GrowTile growTile = (GrowTile)slot;
		_tiles.Remove(growTile);
		growTile.events.onTileDataChanged -= (tileData) => OnTileDataChanged(growTile);
		growTile.events.onSpread -= () => OnSpread(growTile);
	}

	private void OnTileDataChanged(GrowTile growTile)
	{
		AddNewActionAnimation(growTile, ETileAnimation.setTileData);
	}

	private void OnSpread(GrowTile growTile)
	{
		AddNewActionAnimation(growTile, ETileAnimation.spread);
	}

	private void AddNewActionAnimation(GrowTile growTile, ETileAnimation animation)
	{
		_tiles[growTile].animation = animation;
		_tiles[growTile].frameIndex = 0;
		_actionAnimations.Add(growTile);
		_gameState.SetState(EMatchState.Animating);
	}

	public void Update(float deltaTime)
	{
		UpdateAnimation(deltaTime);
	}

	private void UpdateAnimation(float deltaTime)
	{
		foreach (KeyValuePair<GrowTile, TileAnimationPackage> tile in _tiles)
		{
			TileSprites tileSprites = tile.Value.tile.TileData.tileSprites[_seasonData.CurrentSeason];
			Sprite[] sprites = tileSprites.animationsDictionary[tile.Value.animation].sprites;

			tile.Value.tile.TileSpriteRenderer.sprite = sprites[tile.Value.frameIndex];

			tile.Value.frameIndex++;
			if (tile.Value.frameIndex >= sprites.Length)
			{
				if (_actionAnimations.Contains(tile.Value.tile))
				{
					tile.Value.animation = ETileAnimation.idle;
					tile.Value.frameIndex = 0;
					_actionAnimations.Remove(tile.Key);
					if (_actionAnimations.Count == 0)
					{
						_gameState.SetState(EMatchState.Playing);
					}
				}
				else
				{
					tile.Value.frameIndex = 0;
				}
			}
		}
	}

}
