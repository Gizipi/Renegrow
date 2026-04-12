using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CoreData", menuName = "CoreData")]
public class CoreData : ScriptableObject
{
  public UiData uiData;
	public BoardGenerationData boardGenerationData;
	public BoardData boardData;
	public SeasonEvents matchEvents = new();
	public AudioBank audioBank = new();
}
