using System.Collections.Generic;
using UnityEngine;

public class UIMonoObject : GameMonoObject
{
    private UIEventArgument_Disable _disableEventArgument;

    public override void Init(IReadOnlyDictionary<EControllerType, IGameObserver> eventObservers, EventArgument argument, string addressKey)
    {
        base.Init(eventObservers, argument, addressKey);
        
        _disableEventArgument = new UIEventArgument_Disable()
        {
            addressKey = addressKey,
            deActivateObject = this,
        };
    }

    protected virtual void OnDisable()
    {
        if (_eventObservers.TryGetValue(EControllerType.UI, out var gameObserver) == false)
        {
            Debug.LogError("Not initialized");
            return;
        }

        if (_disableEventArgument == null)
        {
            Debug.LogError("There was a problem with the initialization process.");
            return;
        }

        gameObserver.SendEvent(_disableEventArgument);
    }
}
