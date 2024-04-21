using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System;


public static class AddressableUtil
{

    // 키로 생성한 오브젝트 메모리 리스트
    private static readonly Dictionary<string, GameObject> _resourceMemoryObject = new();

    private static async UniTask<GameObject> LoadResource(string addressKey)
    {
        if (_resourceMemoryObject.TryGetValue(addressKey, out var memoryObject) == true)
            return memoryObject;

        var handle = Addressables.LoadAssetAsync<GameObject>(addressKey);

        memoryObject = await handle;

        _resourceMemoryObject.Add(addressKey, memoryObject);

        return memoryObject;
    }

    public static void ReleaseResource(string addressKey)
    {
        if (_resourceMemoryObject.TryGetValue(addressKey, out var memoryObject) == false)
            return;

        Addressables.Release(memoryObject);

        _resourceMemoryObject.Remove(addressKey);
    }

    public static void ReleaseAllResource()
    {
        foreach (var memoryObject in _resourceMemoryObject.Values)
        {
            Addressables.Release(memoryObject);
        }
        
        _resourceMemoryObject.Clear();
    }

    public static void InstantiateResource<T>(string addressKey, Transform parent, Action<T> complete)
        where T : Component
    {
        InstantiateResource(addressKey, parent, obj =>
        {
            T data = obj.GetComponent<T>();

            if (data == null)
                data = obj.AddComponent<T>();

            complete?.Invoke(data);
        }).Forget();
    }

    public static async UniTask InstantiateResource(string addressKey, Transform parent, Action<GameObject> complete)
    {
        if (_resourceMemoryObject.TryGetValue(addressKey, out var memoryObject) == false)
        {
            memoryObject = await LoadResource(addressKey);
        }

        var gameObject = GameObject.Instantiate(memoryObject, parent, false);

        complete?.Invoke(gameObject);
    }
}