using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);  // メインメニューを表示
    }

    public void Hide()
    {
        gameObject.SetActive(false);  // メインメニューを非表示
    }

    // Startボタンを押した際に呼び出されるメソッド
    public void OnStartButtonPressed()
    {
        // UIManagerからレベル選択メニューを表示
        UIManager.Instance.ShowLevelSelect();
    }

    // Quitボタンを押した際に呼び出されるメソッド
    public void OnQuitButtonPressed()
    {
        Application.Quit();  // ゲームを終了する
        Debug.Log("ゲーム終了");
    }
}
