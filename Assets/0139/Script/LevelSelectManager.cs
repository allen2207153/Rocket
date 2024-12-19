using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);  // レベル選択画面を表示
    }

    public void Hide()
    {
        gameObject.SetActive(false);  // レベル選択画面を非表示
    }

    // レベルを選択する際に呼び出されるメソッド
    public void OnLevelSelected(int levelIndex)
    {
        // 選択されたレベルに基づき、レベルをロードする
        Debug.Log("レベル " + levelIndex + " が選択されました");
        // シーンをロード（仮のシーン名を使用）
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level" + levelIndex);
    }

    // 戻るボタンを押した際に呼び出されるメソッド
    public void OnBackButtonPressed()
    {
        // メインメニューに戻る
        UIManager.Instance.ShowMainMenu();
    }
}
