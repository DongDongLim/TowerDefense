using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Slot : UIMonoObject
{
    [SerializeField] protected Button _clickButton;
    [SerializeField] private EventArgument _clickEventArgument;


    protected virtual void Awake()
    {
        _clickButton.OnClickAsObservable().Subscribe(_ =>
        {
            if (_eventObservers.TryGetValue(EControllerType.UI, out var gameObserver) == false)
            {
                Debug.LogError("Not Initialized");
                return;
            }

            gameObserver.SendEvent(_clickEventArgument);
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