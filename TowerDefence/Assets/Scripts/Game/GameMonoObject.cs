using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum EDialogActiveState
{
    None = 0,
    Activating,
    Active,
    DeActivating,
    DeActive,
}
public abstract class GameMonoObject : MonoBehaviour
{
    protected readonly ReactiveProperty<EDialogActiveState> _activeState = new();
    public IObservable<EDialogActiveState> ActiveStateObservable => _activeState.AsObservable();
    
    protected IReadOnlyDictionary<EControllerType, IGameObserver> _eventObservers;
    private string _addressKey;
    public string Addresskey => _addressKey;

    public virtual void Init(string addressKey)
    {
        _addressKey = addressKey;
    }
    public virtual void SetEventObservers(IReadOnlyDictionary<EControllerType, IGameObserver> eventObservers, IEventArgument argument)
    {
        _eventObservers = eventObservers;
    }

    public virtual void Active()
    {
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);
        _activeState.Value = EDialogActiveState.Active;
    }

    public virtual void DeActive()
    {
        if (gameObject.activeSelf == true)
            gameObject.SetActive(false);
        _activeState.Value = EDialogActiveState.DeActive;
    }
}