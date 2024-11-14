using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;  // ”šày‰„ç­ŽžŠÔ
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
            // ŒvŽZ”šàyˆÊ’uC”‡—¡˜ïÝ”šàyá¢¶ÝŽè—‹ŠÝˆÊ’u
            explosionEffect.Explode(transform.position);
            Destroy(gameObject);  // ”šàyŒãç÷šÊŽè—‹
        }
    }
}
