using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 命名空間の導入

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText; // タイマーを表示する TextMeshPro UI 要素
    public float countdownTime = 3f;  // カウントダウンの開始時間

    private float timer = 0f;         // 正のタイマーの初期値
    private bool isCountingUp = false; // 正のカウントが行われているかどうか

    private void Start()
    {
        StartCoroutine(StartCountdown()); // カウントダウンを開始する
    }

    // カウントダウンのコルーチン
    private IEnumerator StartCountdown()
    {
        // カウントダウンが0になるまでループする
        while (countdownTime > 0)
        {
            timerText.text = countdownTime.ToString("0"); // カウントダウンのテキストを更新する
            yield return new WaitForSeconds(1f); // 1秒待つ
            countdownTime--; // カウントダウンを減らす
        }

        timerText.text = "0"; // 0を表示する
        yield return new WaitForSeconds(1f); // 1秒待つ

        // 正のカウントを開始する
        isCountingUp = true;
    }

    // 毎フレーム更新される処理
    private void Update()
    {
        // 正のカウントが行われている場合
        if (isCountingUp)
        {
            timer += Time.deltaTime; // タイマーに経過時間を加算する
            int seconds = Mathf.FloorToInt(timer); // 秒数を計算する
            int milliseconds = Mathf.FloorToInt((timer - seconds) * 100); // ミリ秒を計算する（小数点部分を取り出して100倍）
            timerText.text = string.Format("{0:0}:{1:00}", seconds, milliseconds); // 秒とミリ秒をテキストに更新する
        }
    }
}
