using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro ������Ԃ̓���

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText; // �^�C�}�[��\������ TextMeshPro UI �v�f
    public float countdownTime = 3f;  // �J�E���g�_�E���̊J�n����

    private float timer = 0f;         // ���̃^�C�}�[�̏����l
    private bool isCountingUp = false; // ���̃J�E���g���s���Ă��邩�ǂ���

    private void Start()
    {
        StartCoroutine(StartCountdown()); // �J�E���g�_�E�����J�n����
    }

    // �J�E���g�_�E���̃R���[�`��
    private IEnumerator StartCountdown()
    {
        // �J�E���g�_�E����0�ɂȂ�܂Ń��[�v����
        while (countdownTime > 0)
        {
            timerText.text = countdownTime.ToString("0"); // �J�E���g�_�E���̃e�L�X�g���X�V����
            yield return new WaitForSeconds(1f); // 1�b�҂�
            countdownTime--; // �J�E���g�_�E�������炷
        }

        timerText.text = "0"; // 0��\������
        yield return new WaitForSeconds(1f); // 1�b�҂�

        // ���̃J�E���g���J�n����
        isCountingUp = true;
    }

    // ���t���[���X�V����鏈��
    private void Update()
    {
        // ���̃J�E���g���s���Ă���ꍇ
        if (isCountingUp)
        {
            timer += Time.deltaTime; // �^�C�}�[�Ɍo�ߎ��Ԃ����Z����
            int seconds = Mathf.FloorToInt(timer); // �b�����v�Z����
            int milliseconds = Mathf.FloorToInt((timer - seconds) * 100); // �~���b���v�Z����i�����_���������o����100�{�j
            timerText.text = string.Format("{0:0}:{1:00}", seconds, milliseconds); // �b�ƃ~���b���e�L�X�g�ɍX�V����
        }
    }
}
