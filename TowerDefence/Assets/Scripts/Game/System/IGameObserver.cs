using System;

public interface IGameObserver
{
    public IObservable<EventArgument> GetEventSubject();
    public void SendEvent(EventArgument eventParameter);
}
