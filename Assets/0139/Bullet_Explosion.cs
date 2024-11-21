using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Explosion : MonoBehaviour, IBullet
{
    private Vector3 direction;
    private float speed;

    public ExplosionEffect explosionEffect;  // 爆炸效果
    private bool hasExploded = false;

    // 弾丸の方向と速度を初期化する
    public void Initialize(Vector3 direction, float speed)
    {
        this.direction = direction.normalized;
        this.speed = speed;
    }

    // 弾丸の移動処理
    public void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    // 弾丸がターゲットに命中した際の処理
    public void OnHit(GameObject target)
    {
        if (target.CompareTag("Wall"))
        {
            Debug.Log("Enemy hit by FireBullet!");
            // 火炎のエフェクトや燃焼ダメージのロジックをここに追加
            Explode();
        }
        Destroy(gameObject); // 衝突後に弾丸を破壊する
    }

    // 毎フレーム、弾丸を移動させる
    private void Update()
    {
        Move(); // フレームごとに移動処理を実行
    }

    // 弾丸が他のオブジェクトに衝突した際の処理
    private void OnCollisionEnter(Collision other)
    {
        
        // 壁や敵に衝突したときの処理
        OnHit(other.gameObject);
    }

    public void Explode()
    {

        // 如果已?爆炸?，?不重??行
        if (hasExploded) return;

        hasExploded = true;

        // 触?爆炸效果
        if (explosionEffect != null)
        {
            explosionEffect.Explode(transform.position);
        }

        // 在??之前通知 BombSpawner 清除引用
        BombSpawner bombSpawner = FindObjectOfType<BombSpawner>();
        if (bombSpawner != null)
        {
            bombSpawner.ClearCurrentBomb();
        }

        // ??炸?物体
        Destroy(gameObject);
        Debug.Log("Bomb exploded and destroyed.");
    }
}
