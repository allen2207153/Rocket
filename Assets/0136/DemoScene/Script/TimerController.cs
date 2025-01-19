//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro; // TextMeshPro ���O��Ԃ̃C���|�[�g

//public class TimerController : MonoBehaviour
//{
//    public TextMeshProUGUI timerText;       // �J�E���g�_�E������уJ�E���g�A�b�v��\�����邽�߂�TextMeshPro UI�v�f
//    public GameObject resultUI;             // ���ʕ\���p��UI�I�u�W�F�N�g
//    public TextMeshProUGUI resultTimeText;  // ���ʉ�ʂŎ��Ԃ�\�����邽�߂�TextMeshPro UI�v�f

//    public float countdownTime = 3f;  // �J�E���g�_�E���̏�������

//    private float timer = 0f;         // �J�E���g�A�b�v�̏����l
//    private bool isCountingUp = false; // ���݃J�E���g�A�b�v�����ǂ����̃t���O

//    private void Start()
//    {
//        resultUI.SetActive(false);              // �Q�[���J�n���Ɍ��ʕ\��UI���\���ɂ���
//        resultTimeText.gameObject.SetActive(false); // ���ʎ��Ԃ̃e�L�X�g���\���ɂ���
//        StartCoroutine(StartCountdown());       // �J�E���g�_�E�����J�n����
//    }

//    // �J�E���g�_�E���̃R���[�`��
//    private IEnumerator StartCountdown()
//    {
//        // countdownTime ���� 0 �܂ŃJ�E���g�_�E������
//        while (countdownTime > 0)
//        {
//            timerText.text = countdownTime.ToString("0"); // �J�E���g�_�E���̃e�L�X�g���X�V
//            yield return new WaitForSeconds(1f); // 1�b���ƂɌ���������
//            countdownTime--;
//        }

//        timerText.text = "0"; // "0" ��\��
//        yield return new WaitForSeconds(1f); // 1�b�ҋ@

//        // �J�E���g�A�b�v���J�n
//        isCountingUp = true;
//    }

//    // ���t���[���X�V�����֐�
//    private void Update()
//    {
//        // �J�E���g�A�b�v���̏ꍇ
//        if (isCountingUp)
//        {
//            timer += Time.deltaTime; // �o�ߎ��Ԃ����Z

//            // ���A�b�A�~���b�̌v�Z
//            int minutes = Mathf.FloorToInt(timer / 60); // �����v�Z
//            int seconds = Mathf.FloorToInt(timer % 60); // �b�����v�Z
//            int milliseconds = Mathf.FloorToInt((timer * 100) % 100); // �~���b���v�Z

//            // ���m�Ȏ��Ԃ�\��
//            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
//        }
//    }

//    // �v���C���[���ڕW�v���b�g�t�H�[���ɓ��B�����Ƃ��ɌĂяo�����֐�
//    public void OnReachGoal()
//    {
//        isCountingUp = false; // �J�E���g�A�b�v���~
//        resultUI.SetActive(true); // ����UI��\��
//        resultTimeText.gameObject.SetActive(true); // ���ʎ��Ԃ̃e�L�X�g��\��

//        // ����UI�ɕ\�����鎞�Ԃ�ݒ�
//        int minutes = Mathf.FloorToInt(timer / 60); // �����v�Z
//        int seconds = Mathf.FloorToInt(timer % 60); // �b�����v�Z
//        int milliseconds = Mathf.FloorToInt((timer * 100) % 100); // �~���b���v�Z

//        // ���ʎ��Ԃ��X�V
//        resultTimeText.text = string.Format("Time : {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
//    }

//    // �v���C���[���ڕW�v���b�g�t�H�[���ɐڐG�����Ƃ��Ɍ��o����
//    private void OnTriggerEnter(Collider other)
//    {
//        // �v���C���[���ڕW�v���b�g�t�H�[���ɐG�ꂽ���ǂ������m�F
//        if (other.gameObject.CompareTag("GoalPlatform"))
//        {
//            OnReachGoal(); // ���ʏ������Ăяo��
//        }
//    }
//}
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // IEnumerator ���g�p���邽�߂ɕK�v

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject resultUI;
    public TextMeshProUGUI resultTimeText;

    public float countdownTime = 3f;

    private float timer = 0f;
    private bool isCountingUp = false;

    private void Start()
    {
        resultUI.SetActive(false);
        resultTimeText.gameObject.SetActive(false);
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            timerText.text = countdownTime.ToString("0");
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        timerText.text = "0";
        yield return new WaitForSeconds(1f);

        isCountingUp = true;
    }

    private void Update()
    {
        if (isCountingUp)
        {
            timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            int milliseconds = Mathf.FloorToInt((timer * 100) % 100);

            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }

    public void OnPlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnReachGoal()
    {
        isCountingUp = false;
        resultUI.SetActive(true);
        resultTimeText.gameObject.SetActive(true);

        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        int milliseconds = Mathf.FloorToInt((timer * 100) % 100);

        resultTimeText.text = string.Format("Time : {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoalPlatform"))
        {
            OnReachGoal();
        }

        if (other.CompareTag("DeathPlatform"))
        {
            OnPlayerDeath();
        }
    }
}
