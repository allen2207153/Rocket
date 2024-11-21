using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Explosion : MonoBehaviour, IBullet
{
    private Vector3 direction;
    private float speed;

    public ExplosionEffect explosionEffect;  // ���y����
    private bool hasExploded = false;

    // �e�ۂ̕����Ƒ��x������������
    public void Initialize(Vector3 direction, float speed)
    {
        this.direction = direction.normalized;
        this.speed = speed;
    }

    // �e�ۂ̈ړ�����
    public void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    // �e�ۂ��^�[�Q�b�g�ɖ��������ۂ̏���
    public void OnHit(GameObject target)
    {
        if (target.CompareTag("Wall"))
        {
            Debug.Log("Enemy hit by FireBullet!");
            // �Ή��̃G�t�F�N�g��R�ă_���[�W�̃��W�b�N�������ɒǉ�
            Explode();
        }
        Destroy(gameObject); // �Փˌ�ɒe�ۂ�j�󂷂�
    }

    // ���t���[���A�e�ۂ��ړ�������
    private void Update()
    {
        Move(); // �t���[�����ƂɈړ����������s
    }

    // �e�ۂ����̃I�u�W�F�N�g�ɏՓ˂����ۂ̏���
    private void OnCollisionEnter(Collision other)
    {
        
        // �ǂ�G�ɏՓ˂����Ƃ��̏���
        OnHit(other.gameObject);
    }

    public void Explode()
    {

        // �@�ʛ�?���y?�C?�s�d??�s
        if (hasExploded) return;

        hasExploded = true;

        // �G?���y����
        if (explosionEffect != null)
        {
            explosionEffect.Explode(transform.position);
        }

        // ��??�V�O�ʒm BombSpawner �������p
        BombSpawner bombSpawner = FindObjectOfType<BombSpawner>();
        if (bombSpawner != null)
        {
            bombSpawner.ClearCurrentBomb();
        }

        // ??�y?����
        Destroy(gameObject);
        Debug.Log("Bomb exploded and destroyed.");
    }
}
