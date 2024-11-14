using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float delay = 3f;  // ���y��筎���
    public ExplosionEffect explosionEffect;
    private bool hasExploded = false;

    private void Start()
    {
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    public void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // �\ᢔ��y����
        if (explosionEffect != null)
        {
            explosionEffect.Explode(transform.position);
        }
        Destroy(gameObject);
    }
}
