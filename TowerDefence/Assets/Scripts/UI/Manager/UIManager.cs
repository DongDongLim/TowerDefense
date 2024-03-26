using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : SingleTonMono<UIManager>
{
    public RectTransform rectTransfrom;
    public Canvas uiCanvas;
    public Camera uiCamera;

    

    void Awake()
    {
        // RectTransform 세팅
        rectTransfrom = GetComponent<RectTransform>();
        if (rectTransfrom == null)
            rectTransfrom = gameObject.AddComponent<RectTransform>();

        // UI카메라 세팅
        uiCamera = GetComponentInChildren<Camera>();
        if (uiCamera == null)
        {
            var uiCameraObj = new GameObject(typeof(Camera).Name);
            var rect = uiCameraObj.AddComponent<RectTransform>();
            rect.SetParent(rectTransfrom);
            rect.localPosition.Set(0, 0, -10);
            uiCamera = uiCameraObj.AddComponent<Camera>();
            uiCamera.clearFlags = CameraClearFlags.Depth;
            uiCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
            uiCamera.orthographic = true;
            uiCamera.orthographicSize = 5f;
        }

        // UI캔버스 세팅
        uiCanvas = GetComponent<Canvas>();
        if (uiCanvas == null)
        {
            uiCanvas = gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<CanvasScaler>();
            gameObject.AddComponent<GraphicRaycaster>();
        }
        uiCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        uiCanvas.worldCamera = uiCamera;

        // 이벤트 시스템 세팅
        var eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            var eventSystemObj = new GameObject(typeof(EventSystem).Name);
            eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();
        }

        // 레이어 세팅
        gameObject.layer = LayerMask.NameToLayer("UI");
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("UI");
        }
    }
}
