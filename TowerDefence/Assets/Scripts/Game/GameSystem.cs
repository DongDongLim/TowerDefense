using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Linq;

public sealed class GameSystem : MonoBehaviour
{
    [SerializeField] private AssetLabelReference _controllerLabel;
    private readonly Dictionary<EControllerType, IGameObserver> _eventObserver = new();
    private AddressablePool _controllerPool;
    
    private void Awake()
    {
        _controllerPool = new AddressablePool();
        if (_controllerLabel.RuntimeKeyIsValid() == false)
        {
            Debug.LogError("Not Setting ControllerLabel");
            return;
        }

        Addressables.LoadResourceLocationsAsync(_controllerLabel.RuntimeKey).Completed += result =>
        {
            var resourceList = result.Result;

            if (resourceList == null || resourceList.Count == 0)
                return;

            var addressKeys = resourceList.Select(item => item.PrimaryKey).ToList();

            _controllerPool.GetGameMonoObjects<Controller>(addressKeys, null, controllers =>
            {
                foreach (var controller in controllers)
                {
                    _eventObserver.TryAdd(controller.ControllerType, controller.GetObserver());
                    controller.SetEventObservers(_eventObserver, null);
                    controller.Active();
                }
            });
        };
    }

    private void OnDestroy()
    {
        AddressablePool.ReleaseAllResource();
    }
}