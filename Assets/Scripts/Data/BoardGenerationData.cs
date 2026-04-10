using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BoardGenerationData", menuName = "BoardGenerationData", order = 1)]
public class BoardGenerationData : ScriptableObject
{
    public int width;
    public int height;
    public TileGenerationData[] tiles;
    public PlantData[] plants;
    public GameObject tileVisualPrefab;
    public int deadPlantCount;
}
