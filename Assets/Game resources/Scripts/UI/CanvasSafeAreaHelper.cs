using UnityEngine;

public class CanvasSafeAreaHelper : MonoBehaviour
{
    [SerializeField]  private RectTransform _safeAreaTransform;
    
    private Canvas _canvas;
    private Rect _lastBannerAdjustedSafeArea;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        UpdateSafeArea();
    }

    private void UpdateSafeArea()
    {
        var bannerAdjustedSafeArea = GetBannerAdjustedSafeArea();

        if (bannerAdjustedSafeArea == _lastBannerAdjustedSafeArea)
            return;

        _lastBannerAdjustedSafeArea = bannerAdjustedSafeArea;
        SetSafeAreaRectTransform(_lastBannerAdjustedSafeArea);
    }

    private Rect GetBannerAdjustedSafeArea()
    {
        var safeArea = Screen.safeArea;
        return safeArea;
    }
        
    private void SetSafeAreaRectTransform(Rect newRect)
    {
        if (! _safeAreaTransform)
            return;
        
        var anchorMin = newRect.position;
        var anchorMax = newRect.position + newRect.size;
        
        var canvasPixelRect = _canvas.pixelRect;
        anchorMin.x /= canvasPixelRect.width;
        anchorMin.y /= canvasPixelRect.height;
        anchorMax.x /= canvasPixelRect.width;
        anchorMax.y /= canvasPixelRect.height;

        _safeAreaTransform.anchorMin = anchorMin;
        _safeAreaTransform.anchorMax = anchorMax;
    }
}