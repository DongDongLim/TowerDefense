using System;
using UniRx;

public class FSMState<T> where T : Enum
{
    #region Variables

    public T State => _state;
    protected T _state;

    protected IDisposable _updateObservable;

    #endregion

    #region Methods
    
    private FSMState() { }

    public FSMState(T state)
    {
        _state = state;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {
        _updateObservable?.Dispose();
        _updateObservable = null;
    }

    #endregion
}
