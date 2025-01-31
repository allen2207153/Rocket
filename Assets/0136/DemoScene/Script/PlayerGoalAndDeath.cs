using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerGoalAndDeath : MonoBehaviour
{
    public TextMeshProUGUI timerText;          // 计时器 UI
    public GameObject resultUI;                // 结果 UI
    public TextMeshProUGUI resultTimeText;     // 结果时间 UI
    public GameObject deathUI;                 // 死亡 UI
    public Transform respawnPoint;             // 重生点

    public RawImage cubeRawImage1;             // 第一颗 Cube 的 UI 画面
    public RawImage cubeRawImage2;             // 第二颗 Cube 的 UI 画面

    private float timer = 0f;                  // 计时器
    private bool isCountingUp = true;          // 是否在计时
    private bool isPlayerDead = false;         // 玩家是否死亡
    private Rigidbody playerRigidbody;         // 玩家刚体

    [SerializeField] private int resulttime_gold;
    [SerializeField] private int resulttime_silver;
    [SerializeField] private int resulttime_bronze;

    private Vector3 initialPlayerPosition;     // 玩家初始位置
    private Quaternion initialPlayerRotation;  // 玩家初始旋转

    private void Start()
    {
        // 初始化 UI
        resultUI.SetActive(false);
        deathUI.SetActive(false);
        resultTimeText.gameObject.SetActive(false);

        // 隐藏 Cube 的 UI 画面
        if (cubeRawImage1 != null) cubeRawImage1.gameObject.SetActive(false);
        if (cubeRawImage2 != null) cubeRawImage2.gameObject.SetActive(false);

        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("玩家缺少 Rigidbody 组件！");
        }

        // 记录玩家初始位置和旋转
        if (respawnPoint != null)
        {
            initialPlayerPosition = respawnPoint.position;
            initialPlayerRotation = respawnPoint.rotation;
        }
        else
        {
            initialPlayerPosition = transform.position;
            initialPlayerRotation = transform.rotation;
        }
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

            UpdateCubesBasedOnTime();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void UpdateCubesBasedOnTime()
    {
        if (timer <= resulttime_gold)
        {
            if (cubeRawImage1 != null) cubeRawImage1.gameObject.SetActive(true);
            if (cubeRawImage2 != null) cubeRawImage2.gameObject.SetActive(false);
        }
        else
        {
            cubeRawImage1.gameObject.SetActive(false);
            cubeRawImage2.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("GoalPlatform"))
        {
            OnReachGoal();
        }
        if (collision.gameObject.CompareTag("DeathPlatform"))
        {
            Die();
        }
    }

    private void OnReachGoal()
    {
        isCountingUp = false;
        resultUI.SetActive(true);
        resultTimeText.gameObject.SetActive(true);

        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        int milliseconds = Mathf.FloorToInt((timer * 100) % 100);

        resultTimeText.text = string.Format("Time : {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var playerController = GetComponent<Player_Movement>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

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

    public void ResetAllObjects()
    {
        Debug.Log("重新加载场景");

        // **恢复 TimeScale，防止暂停状态影响新场景**
        Time.timeScale = 1f;

        // **重新加载当前场景**
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

   
    }

