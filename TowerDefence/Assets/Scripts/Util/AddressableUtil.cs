using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System;


public static class AddressableUtil
{
    // 라벨로 생성한 오브젝트 메모리 리스트
    private static Dictionary<string, List<GameObject>> _resourceMemoryObjects = new();
    // 키로 생성한 오브젝트 메모리 리스트
    private static Dictionary<string, GameObject> _resourceMemoryObject = new();

    public static async UniTask<List<GameObject>> LoadResources(string label)
    {
        var resourceList = await Addressables.LoadResourceLocationsAsync(label);
        foreach (var resource in resourceList)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(resource.PrimaryKey);

            if (_resourceMemoryObjects.TryGetValue(label, out var gameObjects) == false)
            {
                gameObjects = new List<GameObject>();
                _resourceMemoryObjects.Add(label, gameObjects);
            }

            gameObjects.Add(await handle);
        }

        return _resourceMemoryObjects[label];
    }

    public static async UniTask<GameObject> LoadResource(string addressKey)
    {
        if (_resourceMemoryObject.TryGetValue(addressKey, out var memoryObject) == true)
            return memoryObject;

        var handle = Addressables.LoadAssetAsync<GameObject>(addressKey);

        memoryObject = await handle;

        _resourceMemoryObject.Add(addressKey, memoryObject);

        return memoryObject;
    }

    public static void ReleaseResources(string label)
    {
        if (_resourceMemoryObjects.TryGetValue(label, out var memoryObjects) == false)
            return;

        if (memoryObjects == null)
            return;

        foreach (var memoryObject in memoryObjects)
        {
            Addressables.Release(memoryObject);
        }

        _resourceMemoryObjects.Remove(label);
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
        foreach(var memoryObjects in _resourceMemoryObjects.Values)
        {
            foreach(var memoryObject in memoryObjects)
            {
                Addressables.Release(memoryObject);
            }
        }
        foreach(var memoryObject in _resourceMemoryObject.Values)
        {
            Addressables.Release(memoryObject);
        }

        _resourceMemoryObjects.Clear();
        _resourceMemoryObject.Clear();
    }

    public static async UniTask InstantiateResources<T>(string label, Transform parent, Action<List<T>> complete) where T : Component
    {
        if (_resourceMemoryObjects.TryGetValue(label, out var memoryObjects) == false)
        {
            memoryObjects = await LoadResources(label);
        }

        List<T> resultList = new();

        foreach (var memoryObject in memoryObjects)
        {
            var obj = GameObject.Instantiate(memoryObject, parent, false);
            T data = obj.GetComponent<T>();

            if (data == null)
                data = obj.AddComponent<T>();

            resultList.Add(data);
        }

        complete?.Invoke(resultList);
    }

    public static void InstantiateResource<T>(string addressKey, Transform parent, Action<T> complete) where T : Component
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
