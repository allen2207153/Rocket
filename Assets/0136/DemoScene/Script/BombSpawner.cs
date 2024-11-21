using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject bombPrefab;  // 炸弹的预制体
    private GameObject currentBomb; // 当前生成的炸弹引用

    private void Update()
    {
        // 按下 F 键生成炸弹
        if (Input.GetKeyDown(KeyCode.F))
        {
            SpawnBomb();
        }
    }

    public void SpawnBomb()
    {
        // 检查当前炸弹是否存在，如果已经存在，则不再生成新的炸弹
        if (currentBomb != null)
        {
            Debug.Log("There is already a bomb in the scene.");
            return;
        }

        // 生成新的炸弹并保持引用
        currentBomb = Instantiate(bombPrefab, transform.position, transform.rotation);
    }

    // 清除当前炸弹引用
    public void ClearCurrentBomb()
    {
        currentBomb = null;
    }
}
