using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System;


public static class AddressableUtil
{
    // 라벨로 생성한 오브젝트 메모리 리스트
    private static Dictionary<string, List<GameObject>> _resourceGameObjects = new();
    // 키로 생성한 오브젝트 메모리 리스트
    private static Dictionary<string, GameObject> _resourceGameObject = new();

    public static async UniTask<List<GameObject>> LoadResources(string label)
    {
        var resourceList = await Addressables.LoadResourceLocationsAsync(label);
        foreach (var resource in resourceList)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(resource.PrimaryKey);

            if (_resourceGameObjects.TryGetValue(label, out var gameObjects) == false)
            {
                gameObjects = new List<GameObject>();
                _resourceGameObjects.Add(label, gameObjects);
            }

            gameObjects.Add(await handle);
        }

        return _resourceGameObjects[label];
    }

    public static async UniTask<GameObject> LoadResource(string addressKey)
    {
        if (_resourceGameObject.TryGetValue(addressKey, out var gameObject) == true)
            return gameObject;

        var handle = Addressables.LoadAssetAsync<GameObject>(addressKey);

        gameObject = await handle;

        _resourceGameObject.Add(addressKey, gameObject);

        return gameObject;
    }

    public static void ReleaseResources(string label)
    {
        if (_resourceGameObjects.TryGetValue(label, out var gameObjects) == false)
            return;

        if (gameObjects == null)
            return;

        foreach (var handle in gameObjects)
        {
            Addressables.Release(handle);
        }

        _resourceGameObjects.Remove(label);
    }

    public static void ReleaseResource(string addressKey)
    {
        if (_resourceGameObject.TryGetValue(addressKey, out var gameObject) == false)
            return;

        Addressables.Release(gameObject);

        _resourceGameObject.Remove(addressKey);
    }

    public static void ReleaseAllResource()
    {
        foreach(var handles in _resourceGameObjects.Values)
        {
            foreach(var handle in handles)
            {
                Addressables.Release(handle);
            }
        }
        foreach(var handle in _resourceGameObject.Values)
        {
            Addressables.Release(handle);
        }

        _resourceGameObjects.Clear();
        _resourceGameObject.Clear();
    }

    public static async UniTask InstantiateResources<T>(string label, Transform parent, Action<List<T>> complete) where T : Component
    {
        if (_resourceGameObjects.TryGetValue(label, out var gameObjects) == false)
        {
            gameObjects = await LoadResources(label);
        }

        List<T> resultList = new();

        foreach (var handle in gameObjects)
        {
            var obj = GameObject.Instantiate(handle, parent, false);
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
        if (_resourceGameObject.TryGetValue(addressKey, out var handle) == false)
        {
            handle = await LoadResource(addressKey);
        }

        var obj = GameObject.Instantiate(handle, parent, false);

        complete?.Invoke(obj);
    }
}
