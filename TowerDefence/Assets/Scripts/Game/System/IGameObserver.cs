using System;

public interface IGameObserver
{
    public IObservable<IEventArgument> GetEventSubject();
    public void SendEvent(IEventArgument eventParameter);
}
