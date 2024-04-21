using System.Collections.Generic;
using UnityEngine;

public abstract class GameMonoObject : MonoBehaviour
{
    protected IReadOnlyDictionary<EControllerType, IGameObserver> _eventObservers;
    protected string _addressKey;

    public virtual void Init(IReadOnlyDictionary<EControllerType, IGameObserver> eventObservers, EventArgument argument, string addressKey)
    {
        _eventObservers = eventObservers;
        _addressKey = addressKey;
    }

    public virtual void Active()
    {
        gameObject.SetActive(true);
    }
    public virtual void DeActive()
    {
        gameObject.SetActive(false);
    }
}
