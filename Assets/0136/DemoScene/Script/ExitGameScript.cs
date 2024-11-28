using UnityEngine;

public class ExitGameScript : MonoBehaviour
{
    private void Update()
    {
        // Escキーを押すとゲームを終了する
        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            Debug.Log("Game is exiting");
            Application.Quit();
        }
    }
}