using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public enum EControllerType
{
    UI = 0,
    Scene,
    Sound,
}

public abstract class Controller : MonoBehaviour
{
    #region  Variables
    protected EControllerType _controllerType;
    public EControllerType ControllerType => _controllerType;
    protected IReadOnlyDictionary<EControllerType, IGameObserver> _eventObservers;
    #endregion

    #region 

    public abstract IGameObserver GetObserver();

    public virtual void Init(IReadOnlyDictionary<EControllerType, IGameObserver> eventObservers)
    {
        _eventObservers = eventObservers;
    }
    #endregion
}
