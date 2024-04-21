using System;
using System.Collections.Generic;
using UniRx;

public class FSMMachine<T> : IDisposable where T : Enum
{
    #region  Variables

    protected Dictionary<T, FSMState<T>> _stateDictionary;
    protected FSMState<T> _currentState;

    public T CurrentStateType => _currentStateType.Value;
    private ReactiveProperty<T> _currentStateType;

    protected IDisposable _stateObservable;

    private bool _isStateChanging;
    private bool _isDisposed;


    #endregion

    #region  Methods

    public FSMMachine()
    {
        _stateDictionary = new Dictionary<T, FSMState<T>>();
        _currentState = null;
        _currentStateType = new ReactiveProperty<T>();
        _isStateChanging = false;
        _isDisposed = false;
        _stateObservable = _currentStateType.Subscribe(value =>
        {
            if (_stateDictionary.TryGetValue(value, out var nextState) == false)
                return;

            _isStateChanging = true;

            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = nextState;
            _currentState.Enter();

            _isStateChanging = false;
        });
    }

    ~FSMMachine()
    {
        Dispose(false);
    }

    public virtual void Init()
    {

    }

    public virtual void AddFSM(FSMState<T> fsmState)
    {
        if (fsmState == null)
            return;

        if (_stateDictionary.ContainsKey(fsmState.State) == true)
            return;

        _stateDictionary.Add(fsmState.State, fsmState);
    }

    public virtual void SetState(T state)
    {
        if (_isStateChanging == true)
            return;

        _currentStateType.Value = state;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool isDispose)
    {
        if (_isDisposed == true)
            return;

        _isDisposed = true;

        if (isDispose == true)
        {
            _stateObservable?.Dispose();
            _stateObservable = null;
            _currentState.Exit();
            _currentState = null;
            _stateDictionary.Clear();
            _stateDictionary = null;
            _currentStateType = null;
        }

    }

    #endregion
}
