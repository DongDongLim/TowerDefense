using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

public sealed class GameManager : SingleTonMono<GameManager>
{
    [SerializeField] private AssetLabelReference _controllerLabel;
    private UIEventSubject _uIEventSubject;
    public UIEventSubject UIEventSubject => _uIEventSubject;

    private const string StartUIAddressKey = "MainMenu";


    private void Awake()
    {
        _uIEventSubject = new UIEventSubject();
        AddressableUtil.InstantiateResources<Controller>(_controllerLabel.labelString, null, _ =>
        {
            var startEvent = new UIEventArgument_Enable()
            {
                isActive = true,
                addressKey = StartUIAddressKey,
            };

            _uIEventSubject.SendEvent(startEvent);
        }).Forget();
    }

    private void OnDestroy()
    {
        AddressableUtil.ReleaseAllResource();
    }
}
