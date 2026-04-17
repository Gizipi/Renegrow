using UnityEngine;
using System;
using static SeasonEvents;

public interface ISpriteButton
{
    public SpriteButtonEvents spriteButtonEvents { get; }
}

public class SpriteButtonEvents
{
    public delegate void OnClick();
    public event OnClick onClick;
    public void Click()
    {
        if (onClick != null)
            onClick();
    }

    public delegate void OnHoverStart();
    public event OnHoverStart onHoverStart;
    public void HoverStart()
    {
        if (onHoverStart != null)
            onHoverStart();
    }

    public delegate void OnHoverEnd();
    public event OnHoverEnd onHoverEnd;
    public void HoverEnd()
    {
        if (onHoverEnd != null)
            onHoverEnd();
    }
}

public class SpriteButton : MonoBehaviour, ISpriteButton
{
    public SpriteButtonEvents spriteButtonEvents { get; } = new();
    
    public void OnClick()
    {
        Debug.Log("SpriteButton: OnClick");
        spriteButtonEvents.Click();
    }
    public void OnHoverStart()
    {
        spriteButtonEvents.HoverStart();
    }
    public void OnHoverEnd()
    {
        spriteButtonEvents.HoverEnd();
    }
}
