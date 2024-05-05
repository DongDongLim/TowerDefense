using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using Object = UnityEngine.Object;

public sealed class AddressablePool
{
    // 키로 생성한 오브젝트 메모리 리스트
    private static readonly Dictionary<string, GameObject> _resourceMemoryObject = new();
    private readonly Dictionary<string, List<GameMonoObject>> _activeObjects = new();
    private readonly Dictionary<string, Stack<GameMonoObject>> _deActiveObjects = new();

    private static async UniTask<GameObject> LoadResource(string addressKey)
    {
        if (_resourceMemoryObject.TryGetValue(addressKey, out var memoryObject) == true)
            return memoryObject;

        var handle = Addressables.LoadAssetAsync<GameObject>(addressKey);

        memoryObject = await handle;

        _resourceMemoryObject.Add(addressKey, memoryObject);

        return memoryObject;
    }

    public static void ReleaseAllResource()
    {
        foreach (var memoryObject in _resourceMemoryObject.Values)
        {
            Addressables.Release(memoryObject);
        }

        _resourceMemoryObject.Clear();
    }
    
    public void GetGameMonoObjects<T>(List<string> addressKeys, Transform parent, Action<List<T>> complete)
        where T : Component
    {
        GetAsyncGameMonoObjects(addressKeys, parent, objs =>
        {
            var resultDataList = new List<T>();
            foreach (var obj in objs)
            {
                T data = obj.GetComponent<T>();

                if (data == null)
                    data = obj.gameObject.AddComponent<T>();
                resultDataList.Add(data);
            }

            complete?.Invoke(resultDataList);
        }).Forget();
    }
    public void GetGameMonoObjects(List<string> addressKeys, Transform parent, Action<List<GameMonoObject>> complete)
    {
        GetAsyncGameMonoObjects(addressKeys, parent, objs =>
        {
            complete?.Invoke(objs);
        }).Forget();
    }

    private async UniTaskVoid GetAsyncGameMonoObjects(List<string> addressKeys, Transform parent,
        Action<List<GameMonoObject>> complete)
    {
        var resultList = new List<GameMonoObject>();
        foreach (var addressKey in addressKeys)
        {
            if (string.IsNullOrEmpty(addressKey) == true)
                continue;

            if (_deActiveObjects.TryGetValue(addressKey, out var deActiveObject) == true
                && deActiveObject.Count > 0)
            {
                var resultObject = deActiveObject.Pop();
                resultObject.transform.SetParent(parent, false);
                resultList.Add(resultObject);
                continue;
            }
            
            var monoObject = await InstantiateGameMonoObject(addressKey, parent);
            if (_activeObjects.TryGetValue(addressKey, out var activeObject) == false)
            {
                activeObject = new List<GameMonoObject>();
                _activeObjects.Add(addressKey, activeObject);
            }

            activeObject.Add(monoObject);
            resultList.Add(monoObject);
        }

        complete?.Invoke(resultList);
    }

    private async UniTask<GameMonoObject> InstantiateGameMonoObject(string addressKey, Transform parent)
    {
        if (_resourceMemoryObject.TryGetValue(addressKey, out var memoryObject) == false)
        {
            memoryObject = await LoadResource(addressKey);
        }

        var createObject = Object.Instantiate(memoryObject, parent, false);
        var monoObject = createObject.GetComponent<GameMonoObject>();
        if (monoObject == null)
        {
            monoObject = createObject.AddComponent<GameMonoObject>();
        }
        monoObject.Init(addressKey);
        monoObject.ActiveStateObservable
            .Where(message => message == EDialogActiveState.DeActive)
            .Subscribe(_ =>
            {
                InsertDeActiveObjects(monoObject);
            });

        return monoObject;
    }

    private void InsertDeActiveObjects(GameMonoObject gameMonoObject)
    {
        if (_deActiveObjects.TryGetValue(gameMonoObject.Addresskey, out var deActiveObjectList) == false)
        {
            deActiveObjectList = new Stack<GameMonoObject>();
            _deActiveObjects.Add(gameMonoObject.Addresskey, deActiveObjectList);
        }
        deActiveObjectList.Push(gameMonoObject);
    }
}