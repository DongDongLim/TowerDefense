using UnityEngine;

public sealed class SlotListDialog : Dialog
{
    [SerializeField] private Transform _slotParent;

    private EventArgument _startUIEventArgument;

    public override void Init(string addressKey)
    {
        base.Init(addressKey);
        _startUIEventArgument = GetComponent<EventArgument>();
    }

    protected override void EveActive()
    {
        base.EveActive();

        if (_startUIEventArgument == null
            || _eventObservers.TryGetValue(EControllerType.UI, out var gameObserver) == false)
            return;

        gameObserver.SendEvent(_startUIEventArgument);
    }
}