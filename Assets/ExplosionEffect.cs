using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float explosionForce = 500f;    // 爆発の力（爆風の強さ）
    public float explosionRadius = 5f;     // 爆発の範囲
    public float upwardsModifier = 1f;     // 垂直方向への力の強化
    public float horizontalModifier = 1f;  // 水平方向への力の強化

    // 爆発の処理
    public void Explode(Vector3 explosionPosition)
    {
        // 爆発範囲内にある全てのコライダーを取得
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        // 各コライダーに対して爆発力を加える
        foreach (Collider col in colliders)
        {
            // Rigidbodyがあれば爆発力を加える
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 爆発位置から物体への方向を計算
                Vector3 direction = col.transform.position - explosionPosition;

                // 水平方向の推力の追加
                Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;

                // 物体と爆発源との距離を計算
                float distance = direction.magnitude;

                // 距離に基づいて爆発力を調整
                float force = Mathf.Lerp(explosionForce, 0, distance / explosionRadius);

                // 爆発力を物体に加える（垂直方向も考慮）
                rb.AddExplosionForce(force, explosionPosition, explosionRadius, upwardsModifier);

                // 水平方向の推力を別に加える
                rb.AddForce(horizontalDirection * force * horizontalModifier, ForceMode.Impulse);
            }
        }
    }

    // シーンビューで爆発範囲を描画
    private void OnDrawGizmos()
    {
        // Gizmosの色を半透明の赤に設定
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);

        // 物体の位置に爆発範囲を示す円を描画
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
