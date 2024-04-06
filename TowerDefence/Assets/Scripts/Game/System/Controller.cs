using UnityEngine;

public enum EControllerType
{
    UI = 0,
    Scene,
    Sound,
}

public abstract class Controller : MonoBehaviour
{
    protected EControllerType _controllerType;
    public EControllerType ControllerType => _controllerType;

    protected abstract void Awake();
}
