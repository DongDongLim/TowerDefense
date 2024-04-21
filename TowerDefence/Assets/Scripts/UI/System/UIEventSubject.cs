using System;
using UniRx;
using UnityEngine;

public class UIEventSubject : IDisposable, IGameObserver
{
    #region  Variables
    private Subject<UIEventArgument> _eventSubject = new();

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

    public void SendEvent(EventArgument eventParameter)
    {
        var parameter = eventParameter as UIEventArgument;

        if (parameter == null)
        {
            Debug.LogError($"Invalid Cast Argument. type : {eventParameter.GetArgumentType()}");
            return;
        }

        _eventSubject.OnNext(parameter);
    }

    public IObservable<EventArgument> GetEventSubject()
    {
        return _eventSubject.AsObservable();
    }

    #endregion
}
