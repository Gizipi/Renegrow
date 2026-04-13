using UnityEngine;
public enum EPlantGrowthStage
{
    Seedling,
    Young,
    Mature,
}

public enum ELeafType
{
    Green,
    Yellow,
    Purple,
}

[System.Serializable]
public class LeafGroup
{
    public ELeafType type;
    public int count;
}

[System.Serializable]
public class PlantGrowthStage
{
    public EPlantGrowthStage growthStage;
    public Sprite livingVisual;
    public Sprite deadVisual;
    public int capacity;
    public LeafGroup[] production;
    public int range;
    public ESeason[] productionSeasons;
}