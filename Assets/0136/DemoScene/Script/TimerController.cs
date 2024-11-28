using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 名前空間のインポート

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText;       // カウントダウンおよびカウントアップを表示するためのTextMeshPro UI要素
    public GameObject resultUI;             // 結果表示用のUIオブジェクト
    public TextMeshProUGUI resultTimeText;  // 結果画面で時間を表示するためのTextMeshPro UI要素

    public float countdownTime = 3f;  // カウントダウンの初期時間

    private float timer = 0f;         // カウントアップの初期値
    private bool isCountingUp = false; // 現在カウントアップ中かどうかのフラグ

    private void Start()
    {
        resultUI.SetActive(false);              // ゲーム開始時に結果表示UIを非表示にする
        resultTimeText.gameObject.SetActive(false); // 結果時間のテキストを非表示にする
        StartCoroutine(StartCountdown());       // カウントダウンを開始する
    }

    // カウントダウンのコルーチン
    private IEnumerator StartCountdown()
    {
        // countdownTime から 0 までカウントダウンする
        while (countdownTime > 0)
        {
            timerText.text = countdownTime.ToString("0"); // カウントダウンのテキストを更新
            yield return new WaitForSeconds(1f); // 1秒ごとに減少させる
            countdownTime--;
        }

        timerText.text = "0"; // "0" を表示
        yield return new WaitForSeconds(1f); // 1秒待機

        // カウントアップを開始
        isCountingUp = true;
    }

    // 毎フレーム更新される関数
    private void Update()
    {
        // カウントアップ中の場合
        if (isCountingUp)
        {
            timer += Time.deltaTime; // 経過時間を加算
            int seconds = Mathf.FloorToInt(timer); // 秒数を計算
            int milliseconds = Mathf.FloorToInt((timer - seconds) * 100); // ミリ秒を計算
            timerText.text = string.Format("{0:0}:{1:00}", seconds, milliseconds); // 時間を表示
        }
    }

    // プレイヤーが目標プラットフォームに到達したときに呼び出される関数
    public void OnReachGoal()
    {
        isCountingUp = false; // カウントアップを停止
        resultUI.SetActive(true); // 結果UIを表示
        resultTimeText.gameObject.SetActive(true); // 結果時間のテキストを表示

        // 結果UIに表示する時間を設定
        int seconds = Mathf.FloorToInt(timer);
        int milliseconds = Mathf.FloorToInt((timer - seconds) * 100);
        resultTimeText.text = string.Format("Time : {0:0}:{1:00}", seconds, milliseconds); // 結果時間を更新
    }

    // プレイヤーが目標プラットフォームに接触したときに検出する
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが目標プラットフォームに触れたかどうかを確認
        if (other.gameObject.CompareTag("GoalPlatform"))
        {
            OnReachGoal(); // 結果処理を呼び出す
        }
    }
}
