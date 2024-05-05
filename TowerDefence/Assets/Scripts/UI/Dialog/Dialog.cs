using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Dialog : UIMonoObject
{
    #region Varialbles

    [SerializeField] protected Button[] _closeButtons;

    #endregion

    #region  Mathods

    protected virtual void Awake()
    {
        foreach (var closeButton in _closeButtons)
        {
            closeButton.OnClickAsObservable().Subscribe(_ => DeActive());
        }
    }

    public sealed override void Active()
    {
        if (_activeState.Value == EDialogActiveState.Activating)
            return;
        EveActive();
        base.Active();
    }

    protected virtual void EveActive()
    {
        _activeState.Value = EDialogActiveState.Activating;
    }

    public sealed override void DeActive()
    {
        if (_activeState.Value == EDialogActiveState.DeActivating)
            return;
        EveDeActive();
        base.DeActive();
    }

    protected virtual void EveDeActive()
    {
        _activeState.Value = EDialogActiveState.DeActivating;
    }

    #endregion
}
