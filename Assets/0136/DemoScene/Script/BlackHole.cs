using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public GameObject blackHolePrefab;  // 引用黑洞特效的預製物

    // 創建黑洞特效
    public void CreateBlackHole(Vector3 position)
    {
        // 在子彈停止的位置創建黑洞特效
        if (blackHolePrefab != null)
        {
            Instantiate(blackHolePrefab, position, Quaternion.identity);  // 在指定位置實例化黑洞特效
            Debug.Log("Black hole effect triggered at: " + position);
        }
        else
        {
            Debug.LogWarning("Black hole prefab is not assigned!");
        }
    }
}
