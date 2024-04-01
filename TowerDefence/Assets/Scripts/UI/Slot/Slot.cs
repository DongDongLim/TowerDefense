using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class Slot : MonoBehaviour
{
    [SerializeField] protected Button _clickButton;
    protected Action _clickAction;

    private void Awake()
    {
        _clickButton.OnClickAsObservable().Subscribe(_ => _clickAction?.Invoke());
    }

    public void RegisterClickAction(Action action)
    {
        _clickAction = action;
    }
}
