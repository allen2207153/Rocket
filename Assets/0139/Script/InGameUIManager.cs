using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);  // �Q�[����UI��\��
    }

    public void Hide()
    {
        gameObject.SetActive(false);  // �Q�[����UI���\��
    }

    // �|�[�Y�{�^�����������ۂɌĂяo����郁�\�b�h
    public void OnPauseButtonPressed()
    {
        // �|�[�Y���j���[��\�����鏈��
        Time.timeScale = 0f;  // �Q�[�����ꎞ��~
        Debug.Log("�Q�[�����ꎞ��~����܂���");
    }

    // �ĊJ�{�^�����������ۂɌĂяo����郁�\�b�h
    public void OnResumeButtonPressed()
    {
        // �Q�[�����ĊJ���鏈��
        Time.timeScale = 1f;  // �Q�[�����ĊJ
        Debug.Log("�Q�[�����ĊJ����܂���");
    }
}
