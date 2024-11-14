using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;  // ΰyη­Τ
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
            // vZΰyΚuC‘οέΰyα’ΆέθέΚu
            explosionEffect.Explode(transform.position);
            Destroy(gameObject);  // ΰyγηχΚθ
        }
    }
}
