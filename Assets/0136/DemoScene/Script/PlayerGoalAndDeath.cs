using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 引入 UI 命名空间

public class PlayerGoalAndDeath : MonoBehaviour
{
    public TextMeshProUGUI timerText;         // 计时器 UI
    public GameObject resultUI;               // 结果 UI
    public TextMeshProUGUI resultTimeText;    // 结果时间 UI
    public GameObject deathUI;                // 死亡 UI
    public Transform respawnPoint;            // 重生点
    public RawImage cubeRawImage;             // 旋转方块的 UI 画面

    private float timer = 0f;                 // 计时器
    private bool isCountingUp = true;         // 是否在计时
    private bool isPlayerDead = false;        // 玩家是否死亡
    private Rigidbody playerRigidbody;        // 玩家刚体
    private Vector3 initialPlayerPosition;    // 玩家初始位置
    private Quaternion initialPlayerRotation; // 玩家初始旋转

    private void Start()
    {
        resultUI.SetActive(false);
        deathUI.SetActive(false);
        resultTimeText.gameObject.SetActive(false);

        // 确保 cubeRawImage 默认隐藏
        if (cubeRawImage != null)
        {
            cubeRawImage.gameObject.SetActive(false);
        }

        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("玩家缺少 Rigidbody 组件！");
        }

        initialPlayerPosition = transform.position;
        initialPlayerRotation = transform.rotation;
    }

    private void Update()
    {
        if (isPlayerDead && Input.GetKeyDown(KeyCode.Return))
        {
            ResetAllObjects();
        }

        if (isCountingUp)
        {
            timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            int milliseconds = Mathf.FloorToInt((timer * 100) % 100);

            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GoalPlatform"))
        {
            OnReachGoal();
        }
        else if (collision.gameObject.CompareTag("DeathPlatform"))
        {
            Die();
        }
    }

    private void OnReachGoal()
    {
        isCountingUp = false; // 停止计时
        resultUI.SetActive(true);
        resultTimeText.gameObject.SetActive(true);

        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        int milliseconds = Mathf.FloorToInt((timer * 100) % 100);

        resultTimeText.text = string.Format("Time : {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

        // 显示旋转方块的 UI 画面
        if (cubeRawImage != null)
        {
            cubeRawImage.gameObject.SetActive(true);
        }

        // 锁住角色所有操作
        LockPlayerMovement();
    }

    private void LockPlayerMovement()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.isKinematic = true;
        }
    }

    private void Die()
    {
        isCountingUp = false;
        isPlayerDead = true;
        deathUI.SetActive(true);
        Time.timeScale = 0f;

        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void ResetAllObjects()
    {
        transform.position = respawnPoint != null ? respawnPoint.position : initialPlayerPosition;
        transform.rotation = initialPlayerRotation;

        timer = 0f;
        isCountingUp = true;

        resultUI.SetActive(false);
        resultTimeText.gameObject.SetActive(false);
        deathUI.SetActive(false);

        // 关闭旋转物体 UI
        if (cubeRawImage != null)
        {
            cubeRawImage.gameObject.SetActive(false);
        }

        Time.timeScale = 1f;
        isPlayerDead = false;

        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.isKinematic = false; // 重新启用物理效果
        }

        //重新启用玩家控制脚本
        var playerController = GetComponent<Player_Movement>(); // 替换为你的控制脚本
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

}
