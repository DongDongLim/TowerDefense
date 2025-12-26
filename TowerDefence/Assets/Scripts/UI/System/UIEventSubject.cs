using System;
using UniRx;
using UnityEngine;

public class UIEventSubject : IDisposable, IGameObserver
{
    #region  Variables
    private readonly Subject<IEventArgument> _eventSubject = new();

    private bool _isDisposed = false;
    #endregion

    #region  Methods

    ~UIEventSubject()
    {
        Dispose(false);
    }


    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool isDispose)
    {
        if (_isDisposed == true)
            return;
        _isDisposed = true;

        if (isDispose == true)
        {
            _eventSubject.OnCompleted();
        }
    }

    public void SendEvent(IEventArgument eventParameter)
    {
        _eventSubject.OnNext(eventParameter);
    }

    public IObservable<IEventArgument> GetEventSubject()
    {
        return _eventSubject.AsObservable();
    }

    #endregion
}
