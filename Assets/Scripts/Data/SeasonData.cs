using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SeasonPacket
{
	public ESeason season;
	public Sprite sprite;
	public AudioSource audioSource;
}

[CreateAssetMenu(fileName = "SeasonData", menuName = "SeasonData")]
public class SeasonData : ScriptableObject
{
	[SerializeField]
	public SeasonPacket[] seasonSprites;

	[System.NonSerialized]
	private Dictionary<ESeason, Sprite> _seasonSpritesLookup;

	public ESeason StartSeason;
	private ESeason _currentSeason;
	public ESeason CurrentSeason => _currentSeason;
	public SeasonEvents Events { get; } = new();

	private void OnEnable()
	{
		RebuildSeasonSprites();
		Events.onSeasonChange += OnSeasonChange;
	}

	private void OnDisable()
	{
		Events.onSeasonChange -= OnSeasonChange;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		RebuildSeasonSprites();
	}
#endif
	private void RebuildSeasonSprites()
	{
		_seasonSpritesLookup ??= new Dictionary<ESeason, Sprite>();
		_seasonSpritesLookup.Clear();
		if (seasonSprites == null || seasonSprites.Length == 0)
			return;
		foreach (SeasonPacket seasonSprite in seasonSprites)
		{
			if (seasonSprite.sprite == null)
				continue;
			_seasonSpritesLookup[seasonSprite.season] = seasonSprite.sprite;
		}
	}

	public Sprite GetSprite(ESeason season)
	{
		if (_seasonSpritesLookup == null)
			RebuildSeasonSprites();
		return _seasonSpritesLookup != null && _seasonSpritesLookup.TryGetValue(season, out Sprite sprite)
			? sprite
			: null;
	}

	public void ChangeSeason()
	{
		_currentSeason++;
		if (_currentSeason > ESeason.Winter)
		{
			_currentSeason = ESeason.Spring;
		}
		Events.SeasonChange(_currentSeason);
	}

	private void OnSeasonChange(ESeason season)
	{
		_currentSeason = season;
	}
}
