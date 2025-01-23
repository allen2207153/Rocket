using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerGoalAndDeath : MonoBehaviour
{
    public TextMeshProUGUI timerText;          // 用于显示计时器
    public GameObject resultUI;               // 结果UI
    public TextMeshProUGUI resultTimeText;    // 结果时间显示
    public GameObject deathUI;                // 死亡UI

    public Transform respawnPoint;            // 重生点

    private float timer = 0f;                 // 计时器
    private bool isCountingUp = true;         // 是否在计时
    private bool isPlayerDead = false;        // 玩家是否死亡
    private Rigidbody playerRigidbody;        // 玩家刚体

    private Vector3 initialPlayerPosition;    // 玩家初始位置
    private Quaternion initialPlayerRotation; // 玩家初始旋转

    private void Start()
    {
        // 初始化UI
        resultUI.SetActive(false);
        deathUI.SetActive(false);
        resultTimeText.gameObject.SetActive(false);

        // 获取玩家的刚体
        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("玩家缺少 Rigidbody 组件！");
        }

        // 记录玩家初始位置和旋转
        initialPlayerPosition = transform.position;
        initialPlayerRotation = transform.rotation;
    }

    private void Update()
    {
        // 如果玩家死亡，按下 Enter 键进行重置
        if (isPlayerDead && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("重置所有物体和状态...");
            ResetAllObjects();
        }

        // 正常计时
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
        Debug.Log($"Collision detected with: {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("GoalPlatform"))
        {
            Debug.Log("Player reached GoalPlatform");
            OnReachGoal();
        }
        else if (collision.gameObject.CompareTag("DeathPlatform"))
        {
            Debug.Log("Player hit DeathPlatform");
            Die();
        }
    }


    private void OnReachGoal()
    {
        isCountingUp = false; // 停止计时
        resultUI.SetActive(true); // 显示结果UI
        resultTimeText.gameObject.SetActive(true); // 显示结果时间

        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        int milliseconds = Mathf.FloorToInt((timer * 100) % 100);

        resultTimeText.text = string.Format("Time : {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    private void Die()
    {
        isCountingUp = false; // 停止计时
        isPlayerDead = true;  // 标记玩家死亡
        deathUI.SetActive(true); // 显示死亡UI
        Time.timeScale = 0f; // 暂停游戏

        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero; // 重置速度
            playerRigidbody.angularVelocity = Vector3.zero; // 重置角速度
        }
    }

    private void ResetAllObjects()
    {
        // 重置玩家位置和旋转
        transform.position = respawnPoint != null ? respawnPoint.position : initialPlayerPosition;
        transform.rotation = initialPlayerRotation;

        // 重置计时器
        timer = 0f;
        isCountingUp = true;

        // 重置UI
        resultUI.SetActive(false);
        resultTimeText.gameObject.SetActive(false);
        deathUI.SetActive(false);

        // 恢复游戏状态
        Time.timeScale = 1f;
        isPlayerDead = false;

        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }
    }
}
    