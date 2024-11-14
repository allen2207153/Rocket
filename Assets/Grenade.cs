using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;  // ���y��筎���
    private float timer;

    public ExplosionEffect explosionEffect;

    private void Start()
    {
        timer = delay;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            // �v�Z���y�ʒu�C������ݔ��yᢐ��ݎ藋���݈ʒu
            explosionEffect.Explode(transform.position);
            Destroy(gameObject);  // ���y�����ʎ藋
        }
    }
}
