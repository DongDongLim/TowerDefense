using System;
using System.Collections.Generic;
using UniRx;

public class UIEventSubject : IDisposable
{
    #region  Variables
    private Dictionary<EUIEventType, Subject<UIEventArgument>> _eventSubjects = new();

    private bool _isDisposed = false;
    #endregion

    #region  Methods

    public UIEventSubject()
    {
        for (EUIEventType eventType = EUIEventType.Enable; eventType < EUIEventType.Count; eventType++)
        {
            _eventSubjects.Add(eventType, new());
        }
    }

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
            foreach (var subject in _eventSubjects.Values)
            {
                subject.OnCompleted();
            }
        }
    }


    public void SendEvent(UIEventArgument eventParameter)
    {
        if (_eventSubjects.TryGetValue(eventParameter.eUIEventType, out var subject) == false)
            throw new KeyNotFoundException($"Not Found {eventParameter.eUIEventType}");

        subject.OnNext(eventParameter);
    }

    public IObservable<object> GetUIEventSubject(EUIEventType eventType)
    {
        if (_eventSubjects.TryGetValue(eventType, out var subject) == false)
            throw new KeyNotFoundException($"Not Found {eventType}");

        return subject.AsObservable();
    }

    #endregion
}
