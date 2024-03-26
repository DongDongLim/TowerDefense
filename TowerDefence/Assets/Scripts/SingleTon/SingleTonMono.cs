using UnityEngine;

[DisallowMultipleComponent]
public abstract class SingleTonMono<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            var typeObjs = FindObjectsOfType<T>();

            if (typeObjs.Length != 0)
            {
                _instance = typeObjs[0];
            }

            if (typeObjs.Length > 1)
            {
                for (int i = 1; i < typeObjs.Length; i++)
                {
                    DestroyImmediate(typeObjs[i].gameObject);
                }
            }

            if (_instance == null)
            {
                var obj = new GameObject(typeof(T).Name);
                _instance = obj.AddComponent<T>();
            }

            return _instance;
        }
    }
}
