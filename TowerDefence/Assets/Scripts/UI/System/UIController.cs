using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;

public sealed class UIController : Controller
{
    #region Variables

    [SerializeField] private RectTransform _uiParent;
    private AddressablePool _uiObjectPool;
    private UIEventSubject _uiEventObserver;
    private EventArgument _startEventArgument;

    #endregion

    #region Methods

    private void OnDestroy()
    {
        _uiEventObserver?.Dispose();
    }

    public override IGameObserver GetObserver()
    {
        return _uiEventObserver ??= new UIEventSubject();
    }

    private void Subscribe()
    {
        _uiEventObserver.GetEventSubject().Where(eventArgs => eventArgs is UIEventArgument_Enable)
            .Select(eventArgs => eventArgs as UIEventArgument_Enable).Subscribe(ActivateObject)
            .AddTo(gameObject);
    }

    public override void Init(string addressKey)
    {
        base.Init(addressKey);
        _uiObjectPool = new AddressablePool();
        _uiEventObserver ??= new UIEventSubject();
        _startEventArgument = GetComponent<EventArgument>();
        Subscribe();
    }

    public override void Active()
    {
        base.Active();
        ActivateObject(_startEventArgument as UIEventArgument_Enable);
    }

    private void ActivateObject(UIEventArgument_Enable eventArgument)
    {
        if (eventArgument == null)
            return;

        
        if (eventArgument.AddressableLabel.RuntimeKeyIsValid() == true)
        {
            Addressables.LoadResourceLocationsAsync(eventArgument.AddressableLabel.RuntimeKey).Completed += result =>
            {
                eventArgument.AddressKeys ??= new List<string>();
                eventArgument.AddressKeys.AddRange(result.Result.Select(resource => resource.PrimaryKey));
                eventArgument.AddressableLabel.labelString = string.Empty;
                ActivateObject(eventArgument);
            };
            return;
        }
        
        if (eventArgument.AddressKeys == null || eventArgument.AddressKeys.Count == 0)
            return;
        
        var addressKeys = eventArgument.AddressKeys;
        var parentTransform = eventArgument.ParentTransform == null ? _uiParent : eventArgument.ParentTransform;

        _uiObjectPool.GetGameMonoObjects(eventArgument.AddressKeys, parentTransform, resultList =>
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