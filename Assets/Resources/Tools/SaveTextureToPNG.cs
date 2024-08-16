using UnityEngine;
using System.IO;
using UnityEditor;

public class SaveCameraRenderToPNG : Editor
{
    
    [MenuItem("Tools/Save Camera Render To PNG")]
    private static void SaveRenderTextureToPNG()
    {
       var cameraToRender = FindObjectOfType<Camera>();
        int textureWidth = cameraToRender.pixelWidth;
        int textureHeight = cameraToRender.pixelHeight;

        // Создаем RenderTexture с размерами камеры
        RenderTexture renderTexture = new RenderTexture(textureWidth, textureHeight, 24);
        cameraToRender.targetTexture = renderTexture;

        // Рендерим сцену с камеры
        cameraToRender.Render();
        // Активируем RenderTexture
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        // Создаем Texture2D
        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height);
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        // Сбрасываем активный RenderTexture
        RenderTexture.active = currentActiveRT;

        // Конвертируем Texture2D в PNG
        byte[] pngData = texture2D.EncodeToPNG();
        if (pngData != null)
        {
            var savedtexturePNG = Application.dataPath + "/SavedTexture.png";
            File.WriteAllBytes(savedtexturePNG, pngData);
            Debug.Log("Saved Texture to PNG");
            Debug.Log(savedtexturePNG);
        }
        cameraToRender.targetTexture = null;

        // Освобождаем ресурсы
        // DestroyImmediate(renderTexture);
        // DestroyImmediate(texture2D);
    }
}
