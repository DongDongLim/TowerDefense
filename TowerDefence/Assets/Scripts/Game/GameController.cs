using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    private void Start()
    {
        UIEventManager.Instance.SendEvent(new KeyValuePair<EUIEventType, string>(EUIEventType.Open, "MainMenu"));
    }

    private void OnDestroy()
    {
        AddressableUtil.ReleaseAllResource();
    }
}
