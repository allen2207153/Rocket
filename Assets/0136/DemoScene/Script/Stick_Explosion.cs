using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBulletExplosion : MonoBehaviour, IBullet
{
    private Vector3 direction;
    private float speed;
    private bool hasExploded = false;
    private bool isStuck = false;

    public ExplosionEffect explosionEffect;
    private BombSpawner bombSpawner;

    private Rigidbody rb;

    public void Initialize(Vector3 direction, float speed)
    {
        this.direction = direction.normalized;
        this.speed = speed;
    }

    private void FixedUpdate()
    {
        if (!hasExploded && !isStuck)
        {
            Move();
        }
    }

    public void Move()
    {
        rb.velocity = direction * speed;
    }

    public void OnHit(GameObject target)
    {
        if (hasExploded) return;

        if (target.CompareTag("Wall"))
        {
            Debug.Log("Bullet hit a wall and stuck!");
            StickToWall();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!hasExploded && !isStuck)
        {
            OnHit(other.gameObject);
        }
    }

    private void StickToWall()
    {
        isStuck = true; 
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        transform.parent = rb.transform;
    }

    public void Explode()
    {
        if (hasExploded) return;

        hasExploded = true;

        if (explosionEffect != null)
        {
            explosionEffect.Explode(transform.position);
        }

        if (bombSpawner != null)
        {
            bombSpawner.ClearCurrentBomb();
        }

        Destroy(gameObject);

        Debug.Log("Explosion occurred and bullet destroyed.");
    }

    private void Awake()
    {
        bombSpawner = FindObjectOfType<BombSpawner>();
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void Update()
    {
        if (isStuck && Input.GetKeyDown(KeyCode.G))
        {
            Explode();
        }
    }
}
