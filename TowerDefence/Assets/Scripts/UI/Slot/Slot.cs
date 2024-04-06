using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Slot : GameMonoObject
{
    [SerializeField] protected Button _clickButton;
    [SerializeField] private UIEventArgument uIEventArgument;


    public override void Init(EventArgument argument)
    {
        
    }

    private void Awake()
    {
        _clickButton.OnClickAsObservable().Subscribe(_ => 
        {
            GameManager.Instance.UIEventSubject.SendEvent(uIEventArgument);
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
