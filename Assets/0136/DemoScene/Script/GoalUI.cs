using System.Collections;
using UnityEngine;
using TMPro;

public class GoalUI : MonoBehaviour
{
    public GameObject endingUI; // �I����ʂ�UI
    public TextMeshProUGUI timerText; // �Q�[�����̃^�C�}�[�\��
    public TextMeshProUGUI resultTimeText; // �I����ʂł̌��ʎ��ԕ\��

    private float timer; // �Q�[�����̌o�ߎ���
    private bool isCounting = true; // �^�C�}�[�������Ă��邩�ǂ���

    private void Start()
    {
        endingUI.SetActive(false); // ���������ɏI����ʂ��\���ɂ���
        timer = 0f; // �^�C�}�[��������
    }

    private void Update()
    {
        if (isCounting)
        {
            timer += Time.deltaTime; // �o�ߎ��Ԃ��v�Z�iTime.timeScale�̉e�����󂯂�j
            UpdateTimerUI(timer);
        }
    }

    private void UpdateTimerUI(float time)
    {
        int seconds = Mathf.FloorToInt(time); // �b�����v�Z
        int milliseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100f); // �~���b���v�Z
        timerText.text = $"{seconds:00}:{milliseconds:00}"; // �^�C�}�[��UI�ɕ\��
    }

    private void EndGame()
    {
        isCounting = false; // �^�C�}�[���~

        // ���ʉ�ʂɕ\�����鎞�Ԃ�ݒ�
        resultTimeText.text = $"Final Time: {Mathf.FloorToInt(timer)}:{Mathf.FloorToInt((timer - Mathf.FloorToInt(timer)) * 100):00}";

        // �f�o�b�O���O�Ŏ��Ԃ�\��
        Debug.Log($"Game Over! Final Time: {timer:F2} seconds");

        // �I����ʂ�\��
        endingUI.SetActive(true);

        // �Q�[�����̎��Ԃ����S��~
        Time.timeScale = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoalPlatform"))
        {
            EndGame(); // �v���C���[���ڕW�v���b�g�t�H�[���ɐG���ƃQ�[���I��
        }
    }
}
