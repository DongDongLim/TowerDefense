using System;

public enum EUIEventType
{
    Enable = 0,

    Count,
}

[Serializable]
public class UIEventArgument : EventArgument
{
    public EUIEventType eUIEventType;
    public UIEventArgument(EUIEventType uIEventType)
    {
        eUIEventType = uIEventType;
    }
}

[Serializable]
public class UIEventArgument_Enable : UIEventArgument
{
    public bool isActive;
    public string addressKey;

    public UIEventArgument_Enable() : base(EUIEventType.Enable)
    {

    }
}