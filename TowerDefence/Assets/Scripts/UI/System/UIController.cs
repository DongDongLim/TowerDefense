using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Unity.VisualScripting;

public sealed class UIController : Controller
{
    #region Variables

    [SerializeField] private RectTransform _uiParent;
    private AddressablePool _uiObjectPool;
    private const string StartUIAddressKey = "MainMenu";
    private readonly List<IDisposable> _disposables = new();

    #endregion

    #region Methods

    private void OnDestroy()
    {
        foreach (var disposable in _disposables)
        {
            disposable?.Dispose();
        }
    }

    public override IGameObserver GetObserver()
    {
        var subject = new UIEventSubject();
        Subscribe(subject);
        return subject;
    }

    private void Subscribe(UIEventSubject subject)
    {
        _disposables.Add(
            subject.GetEventSubject().Where(eventArgs => eventArgs is UIEventArgument_Enable)
                .Select(eventArgs => eventArgs as UIEventArgument_Enable).Subscribe(ActivateObject)
                .AddTo(gameObject));
    }

    public override void Init(string addressKey)
    {
        base.Init(addressKey);
        _uiObjectPool = new AddressablePool();
    }

    public override void Active()
    {
        base.Active();
        
        var startEvent = new UIEventArgument_Enable()
        {
            addressKeys = new List<string>() { StartUIAddressKey },
        };

        ActivateObject(startEvent);
    }

    private void ActivateObject(UIEventArgument_Enable eventArgument)
    {
        if (eventArgument.addressKeys == null || eventArgument.addressKeys.Count == 0)
            return;

        var parentTransform = eventArgument.parentTransform == null ? _uiParent : eventArgument.parentTransform;

        var addressKeys = eventArgument.addressKeys;

        _uiObjectPool.GetGameMonoObjects(eventArgument.addressKeys, parentTransform, resultList =>
        {
            if (resultList == null || resultList.Count == 0)
            {
                return;
            }

            foreach (var activeObject in resultList)
            {
                activeObject.SetEventObservers(_eventObservers, eventArgument);
                activeObject.Active();
            }
        });
    }

    #endregion
}