using UnityEngine;


public class GrassWarningOption : IWarningCondition
{
    private GameObject _warningObject;
    private bool _isWarningConditionMet = false;
    public GrassWarningOption(GameObject warningObject)
    {
        _warningObject = warningObject;
    }

    public void ShowWarning()
    {
        if (!_isWarningConditionMet)
            return;
        _warningObject.SetActive(true);
    }
    public bool CheckWarningCondition(GrowTile slot)
    {
        _isWarningConditionMet = !BoardUtility.HasWaterNeighbour(slot);
        return _isWarningConditionMet;
    }
    public void HideWarning()
    {
        _warningObject.SetActive(false);
    }
}


public class PlantWarningOption : IWarningCondition
{
    private GameObject _warningObject;
    private bool _isWarningConditionMet = false;
    public PlantWarningOption(GameObject warningObject)
    {
        _warningObject = warningObject;
    }
    public void ShowWarning()
    {
        if (!_isWarningConditionMet)
            return;
        _warningObject.SetActive(true);
    }
    public bool CheckWarningCondition(GrowTile slot)
    {
        Plant plant = slot.Plant;
        if (plant == null)
        {
            _isWarningConditionMet = false;
            return _isWarningConditionMet;
        }

        _isWarningConditionMet = plant.IsUnhealthy();
        return _isWarningConditionMet;
    }
    public void HideWarning()
    {
        _warningObject.SetActive(false);
    }
}