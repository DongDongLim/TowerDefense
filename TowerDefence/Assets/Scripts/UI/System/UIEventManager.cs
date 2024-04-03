using System;
using System.Collections.Generic;
using UniRx;

public enum EUIEventType
{
    Open,
    Close,
}

public class UIEventManager : SingleTon<UIEventManager>
{
    private Subject<KeyValuePair<EUIEventType, string>> _eventSubject = new();
    public IObservable<KeyValuePair<EUIEventType, string>> eventObservable => _eventSubject.AsObservable();
    
    public void SendEvent(KeyValuePair<EUIEventType, string> eventArg)
    {
        _eventSubject.OnNext(eventArg);
    }
}
