using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class TileSprites
{
    public ESeason season;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "TileData", menuName = "TileData")]
public class TileData : ScriptableObject
{
    public ETileType tileType;
    [SerializeField]
    public TileSprites[] startTileSprites;
    public Dictionary<ESeason, Sprite> tileSprites = new();
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
        tileSprites ??= new Dictionary<ESeason, Sprite>();
        tileSprites.Clear();
        if (startTileSprites == null || startTileSprites.Length == 0)
            return;

        foreach (TileSprites tileSprite in startTileSprites)
        {
            tileSprites.Add(tileSprite.season, tileSprite.sprite);
        }
    }
}
