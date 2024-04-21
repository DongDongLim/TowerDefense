using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public sealed class UIController : Controller
{
    #region Variables

    [SerializeField] private RectTransform _uiParent;
    private readonly Dictionary<string, List<GameMonoObject>> _activeUIObjects = new();
    private readonly Dictionary<string, Stack<GameMonoObject>> _deActiveUIObjects = new();
    
    private const string StartUIAddressKey = "MainMenu";

    #endregion

    #region Methods

    public override IGameObserver GetObserver()
    {
        var subject = new UIEventSubject();
        Subscribe(subject);
        return subject;
    }

    private void Subscribe(UIEventSubject subject)
    {
        subject.GetEventSubject().Where(eventArgs => eventArgs is UIEventArgument_Enable)
            .Select(eventArgs => eventArgs as UIEventArgument_Enable).Subscribe(ActivateObject)
            .AddTo(gameObject);
        subject.GetEventSubject().Where(eventArgs => eventArgs is UIEventArgument_Disable)
            .Select(eventArgs => eventArgs as UIEventArgument_Disable).Subscribe(DeActivateObject)
            .AddTo(gameObject);
    }

    public override void Init(IReadOnlyDictionary<EControllerType, IGameObserver> eventObservers)
    {
        base.Init(eventObservers);

        var startEvent = new UIEventArgument_Enable()
        {
            addressKeys = new List<string>() { StartUIAddressKey },
        };

        ActivateObject(startEvent);
    }

    private void ActivateObject(UIEventArgument_Enable eventArgument)
    {
        var parentTransform = eventArgument.parentTransform == null ? _uiParent : eventArgument.parentTransform;

        var addressKeys = eventArgument.addressKeys;

        foreach (var addressKey in addressKeys)
        {
            if (string.IsNullOrEmpty(addressKey) == true)
                continue;

            if (_activeUIObjects.TryGetValue(addressKey, out var activeObjects) == false)
            {
                activeObjects = new List<GameMonoObject>();
                _activeUIObjects.Add(addressKey, activeObjects);
            }

            if (_deActiveUIObjects.TryGetValue(addressKey, out var deActiveObjects) == false ||
                deActiveObjects.TryPop(out var activeObject) == false)
            {
                AddressableUtil.InstantiateResource<GameMonoObject>(addressKey, parentTransform, gameMonoObject =>
                {
                    gameMonoObject.Init(_eventObservers, eventArgument, addressKey);
                    gameMonoObject.Active();

                    activeObjects.Add(gameMonoObject);
                });
                continue;
            }

            activeObject.Init(_eventObservers, eventArgument, addressKey);
            activeObject.Active();

            activeObjects.Add(activeObject);   
        }
    }

    private void DeActivateObject(UIEventArgument_Disable eventArgument)
    {
        var addressKey = eventArgument.addressKey;
        var deActiveObject = eventArgument.deActivateObject;

        if (_deActiveUIObjects.TryGetValue(addressKey, out var deActiveObjects) == false)
        {
            deActiveObjects = new Stack<GameMonoObject>();
            _deActiveUIObjects.Add(addressKey, deActiveObjects);
        }

        if (_activeUIObjects.TryGetValue(addressKey, out var activeObjects) == false ||
            activeObjects.Contains(deActiveObject) == false)
            throw new InvalidProgramException($"Not Active Object. {deActiveObject.gameObject.name}");

        deActiveObjects.Push(deActiveObject);
        activeObjects.Remove(deActiveObject);

        if (activeObjects.Count == 0)
        {
            _activeUIObjects.Remove(addressKey);
        }
    }

    #endregion
}