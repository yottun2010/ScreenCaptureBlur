using UnityEngine;
using UnityEngine.Rendering;
public class ScreenshotHandlerURP : ScreenshotHandlerBase
{
	void OnEnable()
	{
		RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
	}
	void OnDisable()
	{
		RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
	}
	private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		if (m_bTakeScreenshotNextFrame)
		{
			m_bTakeScreenshotNextFrame = false;
			int iWidth = m_RequestSize.x;
			int iHeight = m_RequestSize.y;
			Texture2D screenshotTexture = new Texture2D(iWidth, iHeight, TextureFormat.ARGB32, false);
			Rect rect = new Rect(0, 0, iWidth, iHeight);
			screenshotTexture.ReadPixels(rect, 0, 0);
			screenshotTexture.Apply();
			onScreenshotTaken?.Invoke(screenshotTexture);
		}
	}
}