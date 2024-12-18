using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    private float lifetime;

    public void SetLifetime(float time)
    {
        lifetime = time;
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
