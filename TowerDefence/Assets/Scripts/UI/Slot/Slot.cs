using System;
using UnityEngine;
using UniRx;

public class Slot : UIMonoObject
{
    [SerializeField] protected UIButton _clickButton;
    public IEventArgument ClickEventArgument;
    private IDisposable _clickEvent;

    public override void Init(string addressKey)
    {
        base.Init(addressKey);
     
        _clickEvent?.Dispose();
        _clickEvent = _clickButton.OnClickAsObservable().Subscribe(_ =>
        {
            if (_eventObservers.TryGetValue(EControllerType.UI, out var gameObserver) == false)
            {
                Debug.LogError("Not Initialized");
                return;
            }

            gameObserver.SendEvent(ClickEventArgument);
        }).AddTo(gameObject);   
    }

    private void OnEnable()
    {
        ResetLocalPosition();
    }

    protected virtual void ResetLocalPosition()
    {
        transform.localPosition = Vector2.zero;
    }
}