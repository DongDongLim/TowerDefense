using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

public sealed class UIController : Controller
{
    [SerializeField] private RectTransform _uiParent;
    private Dictionary<string, List<GameObject>> _activeUIObjects = new();
    private Dictionary<string, Stack<GameObject>> _deActiveUIObjects = new();

    protected override void Awake()
    {
        _controllerType = EControllerType.UI;
        
        GameManager.Instance.UIEventSubject.GetUIEventSubject(EUIEventType.Enable).
        Select(eventArg => eventArg as UIEventArgument_Enable).
        Where(eventArg => eventArg.isActive == true).
        Subscribe(eventArg =>
        {
            ActivateObject(eventArg.addressKey);
        }).AddTo(gameObject);
    }

    private void ActivateObject(string key)
    {
        if (_activeUIObjects.TryGetValue(key, out var activeObjects) == false)
        {
            _activeUIObjects.Add(key, new());
        }

        if (_deActiveUIObjects.TryGetValue(key, out var deActiveObjects) == false ||
            deActiveObjects.TryPop(out var obj) == false)
        {
            AddressableUtil.InstantiateResource(key, _uiParent, activeObject => activeObjects.Add(activeObject)).Forget();
            return;
        }

        obj.SetActive(true);

        activeObjects.Add(obj);
    }

    private void DeActivateObject(string key, GameObject gameObject)
    {
        if(_deActiveUIObjects.TryGetValue(key, out var deActiveObjects) == false)
        {
            _deActiveUIObjects.Add(key, new());
        }

        if(_activeUIObjects.TryGetValue(key, out var activeObjects) == false ||
            activeObjects.Contains(gameObject) == false)
            throw new InvalidProgramException($"Not Active Object. {gameObject.name}");
        
        deActiveObjects.Push(gameObject);
        activeObjects.Remove(gameObject);
    }
}
