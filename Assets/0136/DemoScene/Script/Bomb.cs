//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Bomb : MonoBehaviour
//{
//    public float delay = 3f;  // 爆炸延遲時間
//    public ExplosionEffect explosionEffect;
//    private bool hasExploded = false;

//    private void Start()
//    {
//        StartCoroutine(ExplodeAfterDelay());
//    }

//    private IEnumerator ExplodeAfterDelay()
//    {
//        yield return new WaitForSeconds(delay);
//        Explode();
//    }

//    public void Explode()
//    {
//        if (hasExploded) return;
//        hasExploded = true;

//        // 觸發爆炸效果
//        if (explosionEffect != null)
//        {
//            explosionEffect.Explode(transform.position);
//        }
//        Destroy(gameObject);
//    }
//}
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public ExplosionEffect explosionEffect;  // 爆炸效果
    private bool hasExploded = false;

    private void Update()
    {
        // 按下 F 键时触发爆炸
        if (Input.GetKeyDown(KeyCode.G))
        {
            Explode();
        }
    }

    public void Explode()
    {
        // 如果已经爆炸过，则不重复执行
        if (hasExploded) return;

        hasExploded = true;

        // 触发爆炸效果
        if (explosionEffect != null)
        {
            explosionEffect.Explode(transform.position);
        }

        // 销毁炸弹物体
        Destroy(gameObject);
        Debug.Log("Bomb exploded and destroyed.");
    }
}
