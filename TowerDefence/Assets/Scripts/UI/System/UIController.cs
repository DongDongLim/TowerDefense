using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class UIController : MonoBehaviour
{
    [SerializeField] private RectTransform _uiParent;

    void Awake()
    {
        UIEventManager.Instance.eventObservable.
        Where(eventArg => eventArg.Key == EUIEventType.Open).
        Subscribe(eventArg =>
        {
            AddressableUtil.InstantiateResource(eventArg.Value, _uiParent, null).Forget();
        }).AddTo(gameObject);
    }
}
