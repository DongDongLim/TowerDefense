using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

public sealed class GameSystem : MonoBehaviour
{
    [SerializeField] private AssetLabelReference _controllerLabel;
    private readonly Dictionary<EControllerType, IGameObserver> _eventObserver = new();

    private void Awake()
    {
        if (_controllerLabel.RuntimeKeyIsValid() == false)
        {
            Debug.LogError("Not Setting ControllerLobel");
            return;
        }

        Addressables.LoadResourceLocationsAsync(_controllerLabel.RuntimeKey).Completed += result =>
        {
            var resourceList = result.Result;

            if (resourceList == null || resourceList.Count == 0)
                return;

            foreach (var resource in resourceList)
            {
                if (string.IsNullOrEmpty(resource.PrimaryKey) == true)
                    continue;

                AddressableUtil.InstantiateResource<Controller>(resource.PrimaryKey, null, controller =>
                {
                    _eventObserver.TryAdd(controller.ControllerType, controller.GetObserver());
                    controller.Init(_eventObserver);
                });
            }
        };
    }

    private void OnDestroy()
    {
        AddressableUtil.ReleaseAllResource();
    }
}