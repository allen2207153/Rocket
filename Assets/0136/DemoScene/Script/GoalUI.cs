using System.Collections;
using UnityEngine;
using TMPro;

public class GoalUI : MonoBehaviour
{
    public GameObject endingUI; // 終了画面のUI
    public TextMeshProUGUI timerText; // ゲーム中のタイマー表示
    public TextMeshProUGUI resultTimeText; // 終了画面での結果時間表示

    private float timer; // ゲーム内の経過時間
    private bool isCounting = true; // タイマーが動いているかどうか

    private void Start()
    {
        endingUI.SetActive(false); // 初期化時に終了画面を非表示にする
        timer = 0f; // タイマーを初期化
    }

    private void Update()
    {
        if (isCounting)
        {
            timer += Time.deltaTime; // 経過時間を計算（Time.timeScaleの影響を受ける）
            UpdateTimerUI(timer);
        }
    }

    private void UpdateTimerUI(float time)
    {
        int seconds = Mathf.FloorToInt(time); // 秒数を計算
        int milliseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100f); // ミリ秒を計算
        timerText.text = $"{seconds:00}:{milliseconds:00}"; // タイマーをUIに表示
    }

    private void EndGame()
    {
        isCounting = false; // タイマーを停止

        // 結果画面に表示する時間を設定
        resultTimeText.text = $"Final Time: {Mathf.FloorToInt(timer)}:{Mathf.FloorToInt((timer - Mathf.FloorToInt(timer)) * 100):00}";

        // デバッグログで時間を表示
        Debug.Log($"Game Over! Final Time: {timer:F2} seconds");

        // 終了画面を表示
        endingUI.SetActive(true);

        // ゲーム内の時間を完全停止
        Time.timeScale = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoalPlatform"))
        {
            EndGame(); // プレイヤーが目標プラットフォームに触れるとゲーム終了
        }
    }
}
