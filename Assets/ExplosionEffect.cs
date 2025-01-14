//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ExplosionEffect : MonoBehaviour
//{
//    public float explosionForce = 7.5f;   // 爆発の力（爆風の強さ）
//    public float explosionRadius = 3.5f;    // 爆発の範囲
//    public float upwardsModifier = 1f;    // 爆炸时的向上修正
//    public float rotationForceFactor = 0.2f; // 调整为更小的系数

//    // 爆発の処理
//    public void Explode(Vector3 explosionPosition)
//    {
//        // 获取爆炸范围内的所有碰撞器
//        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

//        // 对每个碰撞器添加爆炸力
//        foreach (Collider col in colliders)
//        {
//            // 如果有刚体，则施加爆炸力
//            Rigidbody rb = col.GetComponent<Rigidbody>();
//            if (rb != null)
//            {
//                // 计算爆炸中心到物体的方向
//                Vector3 direction = (col.transform.position - explosionPosition).normalized;

//                // 基于爆炸力与修正，施加爆炸的合力
//                Vector3 force = direction * explosionForce + Vector3.up * upwardsModifier;

//                // 添加力到物体上
//                rb.AddForce(force, ForceMode.Impulse);
//            }
//        }
//    }

//    public void Explode_Gravity(Vector3 explosionPosition)
//    {
//        // 开始协程，持续吸引5秒钟
//        StartCoroutine(GravityPull(explosionPosition, 5f)); // 5秒的吸引效果
//    }

//    private IEnumerator GravityPull(Vector3 explosionPosition, float duration)
//    {
//        float elapsed = 0f;

//        // 指定した時間内に吸引力を継続的に適用
//        while (elapsed < duration)
//        {
//            Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

//            foreach (Collider col in colliders)
//            {
//                Rigidbody rb = col.GetComponent<Rigidbody>();
//                if (rb != null)
//                {
//                    // 中心方向の吸引力を計算
//                    Vector3 directionToCenter = (explosionPosition - col.transform.position).normalized;

//                    // 吸引力の大きさを距離に応じて調整
//                    float distance = Vector3.Distance(explosionPosition, col.transform.position);
//                    float adjustedForce = explosionForce * Mathf.Pow(1 - Mathf.Clamp01(distance / explosionRadius), 2);

//                    // 吸引力を適用
//                    rb.AddForce(directionToCenter * adjustedForce, ForceMode.Acceleration);
//                }
//            }

//            elapsed += Time.fixedDeltaTime;
//            yield return new WaitForFixedUpdate();
//        }

//        // 黒洞が消失する際に物体の速度をリセット
//        ResetObjectVelocities(explosionPosition);
//    }

//    // 吸引範囲内の物体の速度をリセットする
//    private void ResetObjectVelocities(Vector3 explosionPosition)
//    {
//        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

//        foreach (Collider col in colliders)
//        {
//            Rigidbody rb = col.GetComponent<Rigidbody>();
//            if (rb != null)
//            {
//                // 速度をゼロに設定
//                rb.velocity = Vector3.zero;
//                rb.angularVelocity = Vector3.zero;

//                // 必要に応じて物体を固定
//                rb.isKinematic = false; // 必要であれば true に変更して物体を固定化
//            }
//        }
//    }



//    // 在场景视图中绘制爆炸范围
//    private void OnDrawGizmos()
//    {
//        // 将Gizmos的颜色设置为半透明的红色
//        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);

//        // 绘制爆炸范围的球体
//        Gizmos.DrawSphere(transform.position, explosionRadius);
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float explosionForce = 7.5f;   // 爆发的力（爆炸的强度）
    public float explosionRadius = 3.5f;  // 爆炸的范围
    public float upwardsModifier = 1f;    // 爆炸时的向上修正
    public float rotationForceFactor = 0.2f; // 旋转力系数
    public ParticleSystem explosionEffect;  // 引用爆炸特效的粒子系统

    // 爆炸的处理
    public void Explode(Vector3 explosionPosition)
    {
        // 获取爆炸范围内的所有碰撞器
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        // 播放爆炸特效
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, explosionPosition, Quaternion.identity);
        }

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

        // 指定的时间内持续吸引力
        while (elapsed < duration)
        {
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

            foreach (Collider col in colliders)
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // 计算吸引力的方向
                    Vector3 directionToCenter = (explosionPosition - col.transform.position).normalized;

                    // 吸引力的强度随距离变化
                    float distance = Vector3.Distance(explosionPosition, col.transform.position);
                    float adjustedForce = explosionForce * Mathf.Pow(1 - Mathf.Clamp01(distance / explosionRadius), 2);

                    // 应用吸引力
                    rb.AddForce(directionToCenter * adjustedForce, ForceMode.Acceleration);
                }
            }

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // 吸引效果结束时重置物体速度
        ResetObjectVelocities(explosionPosition);
    }

    // 重置吸引范围内物体的速度
    private void ResetObjectVelocities(Vector3 explosionPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 将速度和角速度设为零
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // 必要时可以将物体设置为静态
                rb.isKinematic = false; // 如果需要静态设置为 true
            }
        }
    }

    // 在场景视图中绘制爆炸范围
    private void OnDrawGizmos()
    {
        // 设置 Gizmos 的颜色为半透明红色
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);

        // 绘制爆炸范围的球体
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
