using DG.Tweening;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour, IUIElement
{
    [SerializeField] private CanvasGroup canvasGroup; // UIのCanvasGroupコンポーネント

    // UIを表示する
    public void Show()
    {
        canvasGroup.DOFade(1f, 0.3f).SetEase(Ease.OutCubic); // 透明度をフェードイン
        canvasGroup.interactable = true; // インタラクティブを有効化
        canvasGroup.blocksRaycasts = true; // レイキャストを有効化
    }

    // UIを非表示にする
    public void Hide()
    {
        canvasGroup.DOFade(0f, 0.3f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            canvasGroup.interactable = false; // インタラクティブを無効化
            canvasGroup.blocksRaycasts = false; // レイキャストを無効化
        });
    }
}