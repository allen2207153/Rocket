using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public GameObject blackHolePrefab;  // �ޥζ¬}�S�Ī��w�s��

    // �Ыض¬}�S��
    public void CreateBlackHole(Vector3 position)
    {
        // �b�l�u�����m�Ыض¬}�S��
        if (blackHolePrefab != null)
        {
            Instantiate(blackHolePrefab, position, Quaternion.identity);  // �b���w��m��Ҥƶ¬}�S��
            Debug.Log("Black hole effect triggered at: " + position);
        }
        else
        {
            Debug.LogWarning("Black hole prefab is not assigned!");
        }
    }
}
