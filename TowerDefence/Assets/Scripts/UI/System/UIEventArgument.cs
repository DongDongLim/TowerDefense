using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class UIEventArgument : EventArgument
{
    public override Type GetArgumentType()
    {
        return typeof(UIEventArgument);
    }
}

[Serializable]
public sealed class UIEventArgument_Enable : UIEventArgument
{
    public List<string> addressKeys;
    public Transform parentTransform;
}