using UnityEngine;

public class ExitGameScript : MonoBehaviour
{
    private void Update()
    {
        // Esc�L�[�������ƃQ�[�����I������
        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            Debug.Log("Game is exiting");
            Application.Quit();
        }
    }
}