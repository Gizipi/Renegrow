using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

[System.Serializable]
public class ResourceSprites
{
	public ELeafType ResourceType;
	public Sprite ResourceSprite;
}

[System.Serializable]
[CreateAssetMenu(fileName = "UiData", menuName = "UiData")]
public class UiData : ScriptableObject
{
	public CapacityUi CapacityUiPrefab;
	public ResourceSprites[] ResourceSprites;
	public Dictionary<ELeafType, Sprite> ResourceSpritesDictionary = new();
	public GameObject OptionsParent;

	private void OnEnable()
	{
		RebuildResourceSpritesDictionary();
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		RebuildResourceSpritesDictionary();
	}
#endif

	private void RebuildResourceSpritesDictionary()
	{
		ResourceSpritesDictionary ??= new Dictionary<ELeafType, Sprite>();
		ResourceSpritesDictionary.Clear();
		if (ResourceSprites == null || ResourceSprites.Length == 0)
			return;

		foreach (ResourceSprites resourceSprite in ResourceSprites)
		{
			ResourceSpritesDictionary.Add(resourceSprite.ResourceType, resourceSprite.ResourceSprite);
		}
	}
}
