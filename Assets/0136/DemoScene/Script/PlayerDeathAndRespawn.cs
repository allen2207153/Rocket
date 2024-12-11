using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerDeathAndRespawn : MonoBehaviour
{
    public Transform respawnPoint; // ���X�|�[���n�_�̈ʒu
    public GameObject deathUI; // ���S���ɕ\������Ԃ�UI���
    public TextMeshProUGUI timerText; // ���Ԃ�\������TextMeshPro�R���|�[�l���g

    private float timer = 0f; // �^�C�}�[
    private bool isCounting = true; // �v�������ǂ���

    private void Start()
    {
        deathUI.SetActive(false); // �Q�[���J�n���Ɏ��SUI���\���ɂ���
        ResetTimer(); // �^�C�}�[��������
    }

    private void Update()
    {
        if (isCounting)
        {
            timer += Time.deltaTime; // �o�ߎ��Ԃ��v�Z
            int seconds = Mathf.FloorToInt(timer); // �b�����v�Z
            int milliseconds = Mathf.FloorToInt((timer - seconds) * 100); // �~���b���v�Z
            timerText.text = string.Format("{0:0}:{1:00}", seconds, milliseconds); // ���Ԃ�\��
        }

        // �v���C���[�����S���Ă���ꍇ�AEnter�L�[�������ƃ��X�^�[�g
        if (deathUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            Respawn();
        }
    }

    // �v���C���[������̏��ɐG�ꂽ�Ƃ��Ɏ��S���g���K�[����
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeathPlatform"))
        {
            Die();
        }
    }

    // �v���C���[�̎��S����
    private void Die()
    {
        isCounting = false; // �v�����~
        deathUI.SetActive(true); // ���SUI��\��
        Time.timeScale = 0f; // �Q�[�����ꎞ��~
    }

    // �v���C���[�̃��X�|�[������
    private void Respawn()
    {
        transform.position = respawnPoint.position; // �v���C���[�����X�|�[���n�_�Ɉړ�
        deathUI.SetActive(false); // ���SUI���\��
        Time.timeScale = 1f; // �Q�[�����ĊJ
        ResetTimer(); // �^�C�}�[�����Z�b�g
    }

    // �^�C�}�[�����Z�b�g
    private void ResetTimer()
    {
        timer = 0f; // ���Ԃ�0�Ƀ��Z�b�g
        isCounting = true; // �v�����J�n
    }
}