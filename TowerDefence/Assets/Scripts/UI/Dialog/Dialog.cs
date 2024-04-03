using UnityEngine;
using UnityEngine.UI;
using UniRx;

public enum EDialogActiveState
{
    None = 0,
    Activating,
    Active,
    DeActivating,
    DeActive,
}

public class Dialog : MonoBehaviour
{
    #region Varialbles

    [SerializeField] protected Button[] _closeButtons;

    protected EDialogActiveState _activeState;
    public EDialogActiveState ActiveState => _activeState;

    #endregion

    #region  Mathods

    protected virtual void Awake()
    {
        foreach (var closeButton in _closeButtons)
        {
            closeButton.OnClickAsObservable().Subscribe(_ => DeActive());
        }
    }

    public virtual void Active()
    {
        if (_activeState == EDialogActiveState.Activating)
            return;
        EveActive();
        gameObject.SetActive(true);
        _activeState = EDialogActiveState.Active;
    }

    protected virtual void EveActive()
    {
        _activeState = EDialogActiveState.Activating;
    }

    public virtual void DeActive()
    {
        if (_activeState == EDialogActiveState.DeActivating)
            return;
        EveDeActive();
        gameObject.SetActive(false);
        _activeState = EDialogActiveState.DeActive;
    }

    protected virtual void EveDeActive()
    {
        _activeState = EDialogActiveState.DeActivating;
    }

    #endregion
}
