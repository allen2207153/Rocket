using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // ������i�Ǘ��������

public class PlayerDeathAndRespawn : MonoBehaviour
{
    public Transform respawnPoint; // ���X�|�[���n�_�̈ʒu
    public GameObject deathUI; // ���S���ɕ\������Ԃ�UI���
    public TextMeshProUGUI timerText; // ���Ԃ�\������TextMeshPro�R���|�[�l���g

    private float timer = 0f; // �^�C�}�[
    private bool isCounting = true; // �v�������ǂ���
    private Rigidbody playerRigidbody; // �v���C���[��Rigidbody

    private void Start()
    {
        deathUI.SetActive(false); // �Q�[���J�n���Ɏ��SUI���\���ɂ���
        ResetTimer(); // �^�C�}�[��������
        playerRigidbody = GetComponent<Rigidbody>(); // Rigidbody���擾

        if (playerRigidbody == null)
        {
            Debug.LogError("Rigidbody component is missing from the player object.");
        }
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
            ResetScene(); // �V�[���̃��Z�b�g�������Ăяo��
        }
    }

    // �v���C���[������̃g���K�[�ɐG�ꂽ�Ƃ��Ɏ��S���g���K�[����
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathPlatform"))
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

        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero; // �v���C���[�̑��x�����Z�b�g
            playerRigidbody.angularVelocity = Vector3.zero; // ��]���x�����Z�b�g
        }
    }

    // �v���C���[�̃��X�|�[������
    private void Respawn()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero; // �v���C���[�̑��x�����Z�b�g
            playerRigidbody.angularVelocity = Vector3.zero; // ��]���x�����Z�b�g
            playerRigidbody.MovePosition(respawnPoint.position); // Rigidbody�ňʒu���X�V
        }
        else
        {
            // Rigidbody���Ȃ��ꍇ�ATransform�ňʒu���X�V
            transform.position = respawnPoint.position;
        }

        Debug.Log("Respawned at position: " + respawnPoint.position); // �f�o�b�O�p�o��

        deathUI.SetActive(false); // ���SUI���\��
        Time.timeScale = 1f; // �Q�[�����ĊJ
        ResetTimer(); // �^�C�}�[�����Z�b�g
    }

    // �V�[�������Z�b�g���鏈���i�S�ẴI�u�W�F�N�g�����ɖ߂��j
    private void ResetScene()
    {
        // �V�[�����ēǂݍ��݂��邱�ƂőS�ẴI�u�W�F�N�g�̏�Ԃ����Z�b�g
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // �^�C�}�[�����Z�b�g
    private void ResetTimer()
    {
        timer = 0f; // ���Ԃ�0�Ƀ��Z�b�g
        isCounting = true; // �v�����J�n
    }
}
