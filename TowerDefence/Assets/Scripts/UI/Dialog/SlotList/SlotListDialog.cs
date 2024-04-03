using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SlotListDialog : Dialog
{
    [SerializeField] protected AssetLabelReference slotAddressableLabel;
    [SerializeField] protected Transform _slotParent;

    protected override void Awake()
    {
        base.Awake();
        InstantiateSlot();
    }

    public void InstantiateSlot()
    {
        AddressableUtil.InstantiateResources<Slot>(slotAddressableLabel.labelString, _slotParent, null).Forget();
    }

}
