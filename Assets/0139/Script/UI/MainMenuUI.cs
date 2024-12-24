using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement; // 必須包含以支持場景切換

public class MainMenuUI : MonoBehaviour, IUIElement
{
    [SerializeField] private CanvasGroup canvasGroup; // UIのCanvasGroupコンポーネント
    [SerializeField] private string sceneName;

    // UIを表示する
    public void Show()
    {
        canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutCubic); // 透明度をフェードイン
        canvasGroup.interactable = true; // インタラクティブを有効化
        canvasGroup.blocksRaycasts = true; // レイキャストを有効化
    }

    // UIを非表示にする
    public void Hide()
    {
        canvasGroup.DOFade(0f, 0.5f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            canvasGroup.interactable = false; // インタラクティブを無効化
            canvasGroup.blocksRaycasts = false; // レイキャストを無効化
        });
    }

    // シーンを切り替えるメソッド
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // 指定されたシーンをロード
    }

    // ゲームを終了するメソッド
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // エディタモードの場合、プレイモードを停止
#else
        Application.Quit(); // ビルド済みゲームの場合、ゲームを終了
#endif
    }
}
