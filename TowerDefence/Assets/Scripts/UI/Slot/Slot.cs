using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using System.Collections.Generic;

public class Slot : MonoBehaviour
{
    [SerializeField] protected Button _clickButton;
    [SerializeField] private EUIEventType _uiEventKey;
    [SerializeField] private string _uiEventValue;

    protected KeyValuePair<EUIEventType, string> _slotEvent;

    private void Awake()
    {
        _slotEvent = new KeyValuePair<EUIEventType, string>(_uiEventKey, _uiEventValue);
        _clickButton.OnClickAsObservable().Subscribe(_ => 
        {
            UIEventManager.Instance.SendEvent(_slotEvent);
        });
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
