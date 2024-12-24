using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIC : MonoBehaviour
{
    [SerializeField] private UIManager uiManager; // UIManager の参照
    [SerializeField] private MainMenuUI mainMenu; // MainMenu の参照
    [SerializeField] private PauseMenuUI pauseMenu; // PauseMenu の参照

    private void Update()
    {
        // エスケープキーでポーズメニューを開く
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.OpenMenu(pauseMenu);
        }

        // メニューを閉じる例
        if (Input.GetKeyDown(KeyCode.M))
        {
            uiManager.CloseMenu(mainMenu);
        }
    }
}
