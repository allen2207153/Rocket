using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);  // ゲーム内UIを表示
    }

    public void Hide()
    {
        gameObject.SetActive(false);  // ゲーム内UIを非表示
    }

    // ポーズボタンを押した際に呼び出されるメソッド
    public void OnPauseButtonPressed()
    {
        // ポーズメニューを表示する処理
        Time.timeScale = 0f;  // ゲームを一時停止
        Debug.Log("ゲームが一時停止されました");
    }

    // 再開ボタンを押した際に呼び出されるメソッド
    public void OnResumeButtonPressed()
    {
        // ゲームを再開する処理
        Time.timeScale = 1f;  // ゲームを再開
        Debug.Log("ゲームが再開されました");
    }
}
