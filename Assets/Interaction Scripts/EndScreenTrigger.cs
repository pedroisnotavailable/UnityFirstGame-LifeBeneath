using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenTrigger : PlayerActivatable
{
    public float fadeToBlackDuration = 2f;
    public float titleFadeDuration = 1.5f;
    public string titleText = "THE END";
    public Vector2Int skyboxRenderSize = new Vector2Int(1920, 1080);

    private bool isShowingEndScreen;
    private Texture2D skyboxTexture;

    protected override void OnActivate()
    {
        if (!isShowingEndScreen)
        {
            StartCoroutine(ShowEndScreen());
        }
    }

    protected override bool IsActivated()
    {
        return isShowingEndScreen || base.IsActivated();
    }

    private IEnumerator ShowEndScreen()
    {
        isShowingEndScreen = true;

        Canvas canvas = CreateCanvas();
        Image blackPanel = CreateBlackPanel(canvas.transform);
        CanvasGroup contentGroup = CreateEndContent(canvas.transform);
        RawImage skyboxImage = contentGroup.GetComponentInChildren<RawImage>(true);
        skyboxTexture = RenderSkyboxToTexture();
        skyboxImage.texture = skyboxTexture;

        yield return FadeCanvasGroup(blackPanel.GetComponent<CanvasGroup>(), 0f, 1f, fadeToBlackDuration);

        contentGroup.gameObject.SetActive(true);

        yield return FadeCanvasGroup(contentGroup, 0f, 1f, titleFadeDuration);
    }

    private Canvas CreateCanvas()
    {
        GameObject canvasObject = new GameObject("EndScreenCanvas");
        DontDestroyOnLoad(canvasObject);

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        canvasObject.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    private Image CreateBlackPanel(Transform parent)
    {
        GameObject panelObject = CreateUIObject("BlackFade", parent);

        Image panel = panelObject.AddComponent<Image>();
        panel.color = Color.black;
        panel.raycastTarget = true;
        StretchToParent(panel.rectTransform);

        CanvasGroup group = panelObject.AddComponent<CanvasGroup>();
        group.alpha = 0f;
        group.blocksRaycasts = true;
        return panel;
    }

    private CanvasGroup CreateEndContent(Transform parent)
    {
        GameObject contentObject = CreateUIObject("EndContent", parent);
        contentObject.SetActive(false);
        StretchToParent(contentObject.GetComponent<RectTransform>());

        CanvasGroup group = contentObject.AddComponent<CanvasGroup>();
        group.alpha = 0f;

        GameObject imageObject = CreateUIObject("SkyboxImage", contentObject.transform);
        RawImage image = imageObject.AddComponent<RawImage>();
        image.color = Color.white;
        image.raycastTarget = false;
        StretchToParent(image.rectTransform);

        GameObject shadeObject = CreateUIObject("TextShade", contentObject.transform);
        Image shade = shadeObject.AddComponent<Image>();
        shade.color = new Color(0f, 0f, 0f, 0.35f);
        shade.raycastTarget = false;
        StretchToParent(shade.rectTransform);

        GameObject textObject = CreateUIObject("EndText", contentObject.transform);
        Text text = textObject.AddComponent<Text>();
        text.text = titleText;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.fontSize = 96;
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 48;
        text.resizeTextMaxSize = 140;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (text.font == null)
        {
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        RectTransform textTransform = text.rectTransform;
        textTransform.anchorMin = new Vector2(0.1f, 0.35f);
        textTransform.anchorMax = new Vector2(0.9f, 0.65f);
        textTransform.offsetMin = Vector2.zero;
        textTransform.offsetMax = Vector2.zero;

        return group;
    }

    private Texture2D RenderSkyboxToTexture()
    {
        int width = Mathf.Max(1, skyboxRenderSize.x);
        int height = Mathf.Max(1, skyboxRenderSize.y);

        RenderTexture renderTexture = new RenderTexture(width, height, 24)
        {
            name = "EndScreenSkybox"
        };

        GameObject cameraObject = new GameObject("EndScreenSkyboxCamera");
        Camera camera = cameraObject.AddComponent<Camera>();
        Skybox skybox = cameraObject.AddComponent<Skybox>();
        skybox.material = RenderSettings.skybox;

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            cameraObject.transform.SetPositionAndRotation(mainCamera.transform.position, mainCamera.transform.rotation);
            camera.fieldOfView = mainCamera.fieldOfView;
        }
        else
        {
            camera.fieldOfView = 75f;
        }

        camera.clearFlags = CameraClearFlags.Skybox;
        camera.cullingMask = 0;
        camera.backgroundColor = Color.black;
        camera.targetTexture = renderTexture;
        camera.Render();

        RenderTexture previousActiveTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false)
        {
            name = "EndScreenSkyboxImage"
        };
        texture.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
        texture.Apply();

        RenderTexture.active = previousActiveTexture;
        camera.targetTexture = null;
        Destroy(cameraObject);
        renderTexture.Release();
        Destroy(renderTexture);

        return texture;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
    {
        if (duration <= 0f)
        {
            group.alpha = to;
            yield break;
        }

        float elapsed = 0f;
        group.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        group.alpha = to;
    }

    private GameObject CreateUIObject(string name, Transform parent)
    {
        GameObject uiObject = new GameObject(name, typeof(RectTransform));
        uiObject.transform.SetParent(parent, false);
        return uiObject;
    }

    private void StretchToParent(RectTransform rectTransform)
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.localScale = Vector3.one;
    }
}
