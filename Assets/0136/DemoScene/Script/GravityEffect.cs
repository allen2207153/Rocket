using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEffect : MonoBehaviour
{
    public float explosionForce = 500f;   // 爆発の力（爆風の強さ）
    public float explosionRadius = 5f;    // 爆発の範囲
    public float upwardsModifier = 1f;    // 爆炸时的向上修正

    // 爆発の処理
    public void Explode(Vector3 explosionPosition)
    {
        // 获取爆炸范围内的所有碰撞器
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        // 对每个碰撞器添加爆炸力
        foreach (Collider col in colliders)
        {
            // 如果有刚体，则施加爆炸力
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 计算爆炸中心到物体的方向（反向，吸引力）
                Vector3 direction = (explosionPosition - col.transform.position).normalized;

                // 基于爆炸力与修正，施加吸引的合力
                Vector3 force = direction * explosionForce + Vector3.up * upwardsModifier;

                // 添加力到物体上
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }

    // 在场景视图中绘制爆炸范围
    private void OnDrawGizmos()
    {
        // 将Gizmos的颜色设置为半透明的红色
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);

        // 绘制爆炸范围的球体
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
