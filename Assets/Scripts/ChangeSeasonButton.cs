using UnityEngine;
using UnityEngine.UI;

public class ChangeSeasonButton : MonoBehaviour
{
    public SeasonData seasonData;
    public Image image;

    private void Start()
    {
        if (seasonData == null || image == null)
            return;
        SetSeasonSprite();
    }

    public void ChangeSeason()
    {
        if (seasonData == null || image == null)
            return;
        seasonData.ChangeSeason();
        SetSeasonSprite();
    }

    private void SetSeasonSprite()
    {
        Sprite sprite = seasonData.GetSprite(seasonData.CurrentSeason);
        if (sprite != null)
            image.sprite = sprite;
    }
}
