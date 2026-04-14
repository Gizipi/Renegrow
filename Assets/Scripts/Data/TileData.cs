using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileAnimation
{
    public ETileAnimation animation;
    public Sprite[] sprites;
}

[System.Serializable]
public class TileSprites
{
    public ESeason season;
    public Sprite sprite;
    public TileAnimation[] animations;
    public Dictionary<ETileAnimation, TileAnimation> animationsDictionary = new();

    private void OnEnable()
    {
        RebuildAnimationsDictionary();
    }

    private void RebuildAnimationsDictionary()
    {
        animationsDictionary ??= new Dictionary<ETileAnimation, TileAnimation>();
        animationsDictionary.Clear();
        if (animations == null || animations.Length == 0)
            return;
        foreach (TileAnimation animation in animations)
        {
            animationsDictionary.Add(animation.animation, animation);
        }
    }
}

[CreateAssetMenu(fileName = "TileData", menuName = "TileData")]
public class TileData : ScriptableObject
{
    public ETileType tileType;
    [SerializeField]
    public TileSprites[] startTileSprites;
    public Dictionary<ESeason, TileSprites> tileSprites = new();
    public List<ETileType> spreadableTiles = new();

    private void OnEnable()
    {
        RebuildTileSpritesDictionary();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        RebuildTileSpritesDictionary();
    }
#endif

    private void RebuildTileSpritesDictionary()
    {
        tileSprites ??= new Dictionary<ESeason, TileSprites>();
        tileSprites.Clear();
        if (startTileSprites == null || startTileSprites.Length == 0)
            return;

        foreach (TileSprites tileSprite in startTileSprites)
        {
            tileSprites.Add(tileSprite.season, tileSprite);
        }
    }
}
