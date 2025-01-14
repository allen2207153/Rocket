using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity_Explosion_Effect : MonoBehaviour, IBullet
{
    private Vector3 direction;
    private float speed;
    private bool hasExploded = false;
    public bool isStuck;

    public ExplosionEffect explosionEffect;  // 引用 ExplosionEffect 類別
    private BombSpawner bombSpawner;
    private Rigidbody rb;

    private Vector3 initialPosition; // 初始位置
    public float maxDistance = 2f; // 最大射程

    public void Initialize(Vector3 direction, float speed)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        initialPosition = transform.position; // 設置初始位置
    }

    private void FixedUpdate()
    {
        if (!hasExploded)
        {
            Move();
            CheckDistance(); // 檢查距離
        }
    }

    public void Move()
    {
        rb.velocity = direction * speed;
    }

    private void CheckDistance()
    {
        float distanceTravelled = Vector3.Distance(initialPosition, transform.position);
        if (distanceTravelled >= maxDistance)
        {
            StopBullet();
            Explode();
        }
    }

    private void StopBullet()
    {
        rb.velocity = Vector3.zero;  // 停止子彈的移動
        rb.isKinematic = true;  // 設置為靜態，不受物理影響
        Debug.Log("Bullet stopped after reaching max distance.");

        // 在子彈停止的位置生成特效
        if (explosionEffect != null)
        {
            explosionEffect.Explode_Gravity(transform.position);  // 生成特效
        }
    }

    public void OnHit(GameObject target)
    {
        if (hasExploded) return;

        if (target.CompareTag("Wall"))
        {
            Debug.Log("Bullet hit a wall!");
            Explode();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!hasExploded)
        {
            OnHit(other.gameObject);
        }
    }

    public void Explode()
    {
        if (hasExploded) return;

        hasExploded = true;

        if (explosionEffect != null)
        {
            explosionEffect.Explode_Gravity(transform.position);  // 再次調用特效
        }

        if (bombSpawner != null)
        {
            bombSpawner.ClearCurrentBomb();
        }
        StickToWall();
        StartCoroutine(DestroyAfterDelay(5f));  // 開始延遲銷毀物體

        Debug.Log("Explosion occurred, bullet will be destroyed after 5 seconds.");
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void StickToWall()
    {
        isStuck = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        transform.parent = rb.transform;
    }

    private void Awake()
    {
        bombSpawner = FindObjectOfType<BombSpawner>();
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
}
