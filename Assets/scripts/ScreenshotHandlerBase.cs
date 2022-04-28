using UnityEngine;
public abstract class ScreenshotHandlerBase : MonoBehaviour
{
	protected bool m_bTakeScreenshotNextFrame;
	protected System.Action<Texture2D> onScreenshotTaken;
	protected Vector2Int m_RequestSize;
	protected Camera m_Camera;
	[SerializeField] private GameObject m_RenderPanel;

	protected virtual void setCameraTargetTexture(int _iWidth, int _iHeight)
	{
		m_RequestSize = new Vector2Int(_iWidth, _iHeight);
		if (m_Camera != null)
		{
			m_Camera.targetTexture = RenderTexture.GetTemporary(
				_iWidth,
				_iHeight, 16);
		}
	}
	public bool TakeScreenshot(int _iWidth, int _iHeight, System.Action<Texture2D> _onTaken)
	{
		if (m_bTakeScreenshotNextFrame)
		{
			Debug.LogError("Screen Shot can 1 time by 1 Render!");
			return false;
		}
		setCameraTargetTexture(_iWidth, _iHeight);
		m_bTakeScreenshotNextFrame = true;
		onScreenshotTaken = _onTaken;
		return true;
	}
	public bool TakeScreenshot(int _iWidth, int _iHeight)
	{
		return TakeScreenshot(_iWidth, _iHeight, (tex2d) => { });
	}
	public bool RenderScreenshotScreenSize(System.Action<Texture2D> _onTake)
	{
		return TakeScreenshot(Screen.width, Screen.height, _onTake);
	}
	public bool RenderScreenshot(int _iWidth, int _iHeight, System.Action _onFinished)
	{
		return RenderScreenshotScreenSize((tex2d) => {
			FilterProcessing processingInstance = new FilterProcessing();
			Texture2D outTex = processingInstance.imageProcessing(tex2d);
			Sprite texture_sprite = Sprite.Create(outTex, new Rect(0, 0, outTex.width, outTex.height), Vector2.zero);
			m_RenderPanel.GetComponent<UnityEngine.UI.Image>().sprite = texture_sprite;
            _onFinished.Invoke();
		});
	}
	public bool RenderScreenshotScreenSize(System.Action _onFinished)
	{
		return RenderScreenshot(Screen.width, Screen.height, _onFinished);
	}
	public void RenderScreenshotToPanel()
	{
		RenderScreenshotScreenSize(() => { });
	}

}