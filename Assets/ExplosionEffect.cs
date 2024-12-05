using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float explosionForce = 500f;   // 爆発の力（爆風の強さ）
    public float explosionRadius = 5f;    // 爆発の範囲
    public float upwardsModifier = 1f;    // 爆炸时的向上修正
    public float rotationForceFactor = 0.2f; // 调整为更小的系数

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
                // 计算爆炸中心到物体的方向
                Vector3 direction = (col.transform.position - explosionPosition).normalized;

                // 基于爆炸力与修正，施加爆炸的合力
                Vector3 force = direction * explosionForce + Vector3.up * upwardsModifier;

                // 添加力到物体上
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }

    public void Explode_Gravity(Vector3 explosionPosition)
    {
        // 开始协程，持续吸引5秒钟
        StartCoroutine(GravityPull(explosionPosition, 5f)); // 5秒的吸引效果
    }

    private IEnumerator GravityPull(Vector3 explosionPosition, float duration)
    {
        float elapsed = 0f;

        // 在指定的时间内持续施加吸引力
        while (elapsed < duration)
        {
            // 获取爆炸范围内的所有碰撞器
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

            // 对每个碰撞器添加吸引力
            foreach (Collider col in colliders)
            {
                // 如果有刚体，则施加吸引力
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // 计算物体到爆炸中心的方向（吸引力）
                    Vector3 directionToCenter = (explosionPosition - col.transform.position).normalized;

                    // 基于距离调整吸引力，使得越靠近中心力越大
                    float distance = Vector3.Distance(explosionPosition, col.transform.position);
                    float adjustedForce = Mathf.Lerp(explosionForce, 0, distance / explosionRadius);

                    // 减少旋转的离心力系数，防止物体被甩出
                    rotationForceFactor = 0.2f;  // 调整为更小的系数
                    Vector3 rotationAxis = Vector3.up; // 旋转轴设置为向上的方向
                    Vector3 perpendicularForce = Vector3.Cross(directionToCenter, rotationAxis).normalized * adjustedForce * rotationForceFactor;

                    // 将吸引力和绕中心旋转的力结合
                    Vector3 finalForce = directionToCenter * adjustedForce + perpendicularForce;

                    // 施加合力到物体
                    rb.AddForce(finalForce, ForceMode.Acceleration); // 使用加速度模式，力的效果更明显
                }
            }

            // 增加经过的时间
            elapsed += Time.fixedDeltaTime;

            // 等待下一帧物理更新（FixedUpdate）
            yield return new WaitForFixedUpdate();
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
