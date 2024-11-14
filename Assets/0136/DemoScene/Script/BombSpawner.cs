using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject bombPrefab; 
    public Transform spawnPoint; 
    public float throwForce = 10f; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            SpawnBomb();
        }
    }

    void SpawnBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(spawnPoint.forward * throwForce, ForceMode.Impulse);
        }
    }
}
