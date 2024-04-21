using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

public sealed class SlotListDialog : Dialog
{
    [SerializeField] private AssetLabelReference _slotAddressableLabel;
    [SerializeField] private Transform _slotParent;

    private UIEventArgument_Enable _slotCreateArgument;

    private void Start()
    {
        if (_slotAddressableLabel.RuntimeKeyIsValid() == false)
            return;

        Addressables.LoadResourceLocationsAsync(_slotAddressableLabel.RuntimeKey).Completed += result =>
        {
            var resourceList = result.Result;
            
            _slotCreateArgument = new UIEventArgument_Enable()
            {
                addressKeys = resourceList.Select(item => item.PrimaryKey).ToList(),
                parentTransform = _slotParent,
            };

            InstantiateSlot();
        };
    }
    
    private void InstantiateSlot()
    {
        if(_eventObservers.TryGetValue(EControllerType.UI, out var gameObserver) == false)
            return;

        gameObserver.SendEvent(_slotCreateArgument);
    }

}
