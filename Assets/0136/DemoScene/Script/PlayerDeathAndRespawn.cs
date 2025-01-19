using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // 引入場景管理命名空間

public class PlayerDeathAndRespawn : MonoBehaviour
{
    public Transform respawnPoint; // リスポーン地点の位置
    public GameObject deathUI; // 死亡時に表示する赤いUI画面
    public TextMeshProUGUI timerText; // 時間を表示するTextMeshProコンポーネント

    private float timer = 0f; // タイマー
    private bool isCounting = true; // 計時中かどうか
    private Rigidbody playerRigidbody; // プレイヤーのRigidbody

    private void Start()
    {
        deathUI.SetActive(false); // ゲーム開始時に死亡UIを非表示にする
        ResetTimer(); // タイマーを初期化
        playerRigidbody = GetComponent<Rigidbody>(); // Rigidbodyを取得

        if (playerRigidbody == null)
        {
            Debug.LogError("Rigidbody component is missing from the player object.");
        }
    }

    private void Update()
    {
        if (isCounting)
        {
            timer += Time.deltaTime; // 経過時間を計算
            int seconds = Mathf.FloorToInt(timer); // 秒数を計算
            int milliseconds = Mathf.FloorToInt((timer - seconds) * 100); // ミリ秒を計算
            timerText.text = string.Format("{0:0}:{1:00}", seconds, milliseconds); // 時間を表示
        }

        // プレイヤーが死亡している場合、Enterキーを押すとリスタート
        if (deathUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            Respawn();
            ResetScene(); // シーンのリセット処理を呼び出す
        }
    }

    // プレイヤーが特定のトリガーに触れたときに死亡をトリガーする
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathPlatform"))
        {
            Die();
        }
    }

    // プレイヤーの死亡処理
    private void Die()
    {
        isCounting = false; // 計時を停止
        deathUI.SetActive(true); // 死亡UIを表示
        Time.timeScale = 0f; // ゲームを一時停止

        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero; // プレイヤーの速度をリセット
            playerRigidbody.angularVelocity = Vector3.zero; // 回転速度もリセット
        }
    }

    // プレイヤーのリスポーン処理
    private void Respawn()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero; // プレイヤーの速度をリセット
            playerRigidbody.angularVelocity = Vector3.zero; // 回転速度もリセット
            playerRigidbody.MovePosition(respawnPoint.position); // Rigidbodyで位置を更新
        }
        else
        {
            // Rigidbodyがない場合、Transformで位置を更新
            transform.position = respawnPoint.position;
        }

        Debug.Log("Respawned at position: " + respawnPoint.position); // デバッグ用出力

        deathUI.SetActive(false); // 死亡UIを非表示
        Time.timeScale = 1f; // ゲームを再開
        ResetTimer(); // タイマーをリセット
    }

    // シーンをリセットする処理（全てのオブジェクトを元に戻す）
    private void ResetScene()
    {
        // シーンを再読み込みすることで全てのオブジェクトの状態をリセット
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // タイマーをリセット
    private void ResetTimer()
    {
        timer = 0f; // 時間を0にリセット
        isCounting = true; // 計時を開始
    }
}
