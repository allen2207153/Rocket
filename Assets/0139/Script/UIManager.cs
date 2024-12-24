using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenu; // メインメニューの参照
    [SerializeField] private PauseMenuUI pauseMenu; // ポーズメニューの参照

    // メニューを開く
    public void OpenMenu(IUIElement menu)
    {
        menu.Show(); // メニューを表示
    }

    // メニューを閉じる
    public void CloseMenu(IUIElement menu)
    {
        menu.Hide(); // メニューを非表示
    }

}

