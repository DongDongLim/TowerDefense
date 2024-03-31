using UnityEngine;

public enum ETweenType
{
    None = 0,
    Popup,
}
public class TweenData : ScriptableObject
{
    public ETweenType tweenType;
}
