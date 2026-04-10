using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantData", menuName = "PlantData", order = 1)]
public class PlantData : ScriptableObject
{
    public GameObject plantVisualPrefab;
    public string name;
    public PlantGrowthStage[] startGrowthStages;
    public Dictionary<EPlantGrowthStage, PlantGrowthStage> growthStages = new();
    public LeafGroup[] productionCost;

    private void OnEnable()
    {
        RebuildGrowthStagesDictionary();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        RebuildGrowthStagesDictionary();
    }
#endif

    private void RebuildGrowthStagesDictionary()
    {
        growthStages ??= new Dictionary<EPlantGrowthStage, PlantGrowthStage>();
        growthStages.Clear();
        if (startGrowthStages == null || startGrowthStages.Length == 0)
            return;

        foreach (PlantGrowthStage stage in startGrowthStages)
        {
            if (stage == null)
                continue;
            growthStages[stage.growthStage] = stage;
        }
    }
}
