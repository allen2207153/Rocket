using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [Header("一般設定")]
    public float rotationSpeed = 50f; // 回転速度

    private void Update()
    {
        // 共通の回転ロジック
        Rotate();
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーと接触した場合
        if (other.CompareTag("Player"))
        {
            // 道具の効果（派生クラスで実装）
            OnPickup(other);

            // 道具を削除
            Destroy(gameObject);
        }
    }

    private void Rotate()
    {
        // オブジェクトをY軸で回転させる
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    // 抽象メソッド：各道具の効果をここで実装
    protected abstract void OnPickup(Collider player);
}
