using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  // 需要引入SceneManagement

public class goal_death_reset : MonoBehaviour
{
    public TextMeshProUGUI timerText;          // 計時表示
    public GameObject resultUI;               // 結果UI
    public TextMeshProUGUI resultTimeText;    // 結果時間表示
    public GameObject deathUI;                // 死亡UI

    private float timer = 0f;                 // タイマー
    private bool isCountingUp = false;        // カウントアップ中か
    private bool isPlayerDead = false;        // プレイヤーが死亡したか
    private Rigidbody playerRigidbody;        // プレイヤーのRigidbody

    private Vector3 initialPlayerPosition;    // プレイヤー初期位置
    private Quaternion initialPlayerRotation; // プレイヤー初期回転

    private void Start()
    {
        resultUI.SetActive(false);              // 結果UIを非表示
        deathUI.SetActive(false);               // 死亡UIを非表示
        resultTimeText.gameObject.SetActive(false); // 結果時間表示を非表示
        playerRigidbody = GetComponent<Rigidbody>(); // プレイヤーのRigidbodyを取得

        if (playerRigidbody == null)
        {
            Debug.LogError("プレイヤーオブジェクトにRigidbodyコンポーネントがありません！");
        }

        initialPlayerPosition = transform.position; // プレイヤーの初期位置を記録
        initialPlayerRotation = transform.rotation; // プレイヤーの初期回転を記録

        isCountingUp = true; // カウントアップ開始
    }

    private void Update()
    {
        // プレイヤーが死亡している場合、Enterキーでリセット
        if (isPlayerDead)
        {
            Time.timeScale = 1f;  // 確保遊戲時間恢復

            // 只檢查 Enter 鍵是否被按下
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Enter 鍵被按下，重置所有物件...");
                ResetAllObjects();  // 重置所有物件
            }
        }

        // 正常的計時器
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
        // プレイヤーがGoalPlatformに当たった場合
        if (collision.gameObject.CompareTag("GoalPlatform"))
        {
            OnReachGoal();
        }

        // プレイヤーがDeathPlatformに当たった場合
        if (collision.gameObject.CompareTag("DeathPlatform"))
        {
            Debug.Log("プレイヤーがDeathPlatformに当たりました");
            Die(); // 死亡処理
        }
    }

    private void OnReachGoal()
    {
        isCountingUp = false; // 停止計時
        resultUI.SetActive(true); // 顯示結果UI
        resultTimeText.gameObject.SetActive(true); // 顯示結果時間

        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        int milliseconds = Mathf.FloorToInt((timer * 100) % 100);

        resultTimeText.text = string.Format("Time : {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds); // 顯示結果時間
    }

    private void Die()
    {
        isCountingUp = false; // 停止計時
        isPlayerDead = true;  // 設定玩家死亡狀態
        deathUI.SetActive(true); // 顯示死亡UI
        Time.timeScale = 0f; // 暫停遊戲

        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero; // 重置速度
            playerRigidbody.angularVelocity = Vector3.zero; // 重置旋轉速度
        }
    }

    private void ResetAllObjects()
    {
        // 重設玩家位置和旋轉
        transform.position = initialPlayerPosition;
        transform.rotation = initialPlayerRotation;

        // 重設計時器
        timer = 0f;
        isCountingUp = true;

        // 重設UI
        resultUI.SetActive(false);
        resultTimeText.gameObject.SetActive(false);
        deathUI.SetActive(false);

        // 恢復遊戲狀態
        Time.timeScale = 1f; // 恢復遊戲
        isPlayerDead = false; // 清除死亡狀態
    }
}
