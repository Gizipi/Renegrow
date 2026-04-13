using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ProgressData", menuName = "ProgressData")]
public class ProgressData : ScriptableObject
{
	public WinCondition[] WinConditions;
}
