using System.Collections.Generic;
using UnityEngine;

public class UIMonoObject : GameMonoObject
{
    public override void Active()
    {
        transform.SetAsLastSibling();
        base.Active();
    }
}
