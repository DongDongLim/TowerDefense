using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public sealed class UIEventArgument_Enable : MonoBehaviour, IEventArgument
{
    public AssetLabelReference AddressableLabel;
    [AddressKeyDropDown]
    public List<string> AddressKeys;
    public Transform ParentTransform;
}
