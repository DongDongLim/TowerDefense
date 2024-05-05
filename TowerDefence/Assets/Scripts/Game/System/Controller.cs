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

public abstract class Controller : GameMonoObject
{
    #region  Variables
    protected EControllerType _controllerType;
    public EControllerType ControllerType => _controllerType;
    #endregion

    #region 

    public abstract IGameObserver GetObserver();
    #endregion
}
