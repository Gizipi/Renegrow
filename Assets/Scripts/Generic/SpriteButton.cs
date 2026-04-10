using UnityEngine;
using System;
using static MatchEvents;

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

public class SpriteButton : MonoBehaviour
{
    private readonly SpriteButtonEvents _events = new();
    public SpriteButtonEvents events
    {
        get
        {
            return _events;
        }
    }
    
    public void OnClick()
    {
        Debug.Log("SpriteButton: OnClick");
        events.Click();
    }
    public void OnHoverStart()
    {
        events.HoverStart();
    }
    public void OnHoverEnd()
    {
        events.HoverEnd();
    }
}
