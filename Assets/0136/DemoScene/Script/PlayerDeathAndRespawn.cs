using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerDeathAndRespawn : MonoBehaviour
{
    public Transform respawnPoint; // リスポーン地点の位置
    public GameObject deathUI; // 死亡時に表示する赤いUI画面
    public TextMeshProUGUI timerText; // 時間を表示するTextMeshProコンポーネント

    private float timer = 0f; // タイマー
    private bool isCounting = true; // 計時中かどうか

    private void Start()
    {
        deathUI.SetActive(false); // ゲーム開始時に死亡UIを非表示にする
        ResetTimer(); // タイマーを初期化
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
        }
    }

    // プレイヤーが特定の床に触れたときに死亡をトリガーする
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeathPlatform"))
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
    }

    // プレイヤーのリスポーン処理
    private void Respawn()
    {
        transform.position = respawnPoint.position; // プレイヤーをリスポーン地点に移動
        deathUI.SetActive(false); // 死亡UIを非表示
        Time.timeScale = 1f; // ゲームを再開
        ResetTimer(); // タイマーをリセット
    }

    // タイマーをリセット
    private void ResetTimer()
    {
        timer = 0f; // 時間を0にリセット
        isCounting = true; // 計時を開始
    }
}