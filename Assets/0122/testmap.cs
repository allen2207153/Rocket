using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmap : MonoBehaviour
{
    [Header("Speed Modification Settings")]
    public float speedMultiplier = 1.5f;  // 大於1為加速，小於1為減速

    private FirstPersonController playerController;
    private float originalWalkSpeed;

    // 定義用於識別區域類型的標籤
    private const string SPEED_BOOST_TAG = "SpeedBoost";
    private const string SPEED_SLOW_TAG = "SpeedSlow";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<FirstPersonController>();

            if (playerController != null)
            {
                originalWalkSpeed = playerController.walkSpeed;

                // 根據地形的標籤決定是加速還是減速
                if (gameObject.CompareTag(SPEED_BOOST_TAG))
                {
                    // 加速區域
                    playerController.walkSpeed *= speedMultiplier;
                    Debug.Log($"Speed boosted to: {playerController.walkSpeed}");
                }
                else if (gameObject.CompareTag(SPEED_SLOW_TAG))
                {
                    // 減速區域
                    playerController.walkSpeed *= (1f / speedMultiplier);
                    Debug.Log($"Speed reduced to: {playerController.walkSpeed}");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerController != null)
            {
                // 恢復原始速度
                playerController.walkSpeed = originalWalkSpeed;
                Debug.Log($"Speed restored to: {originalWalkSpeed}");
            }
        }
    }

    // 在編輯器中顯示區域的視覺提示
    private void OnDrawGizmos()
    {
        // 加速區域顯示為綠色
        if (gameObject.CompareTag(SPEED_BOOST_TAG))
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f); // 半透明綠色
        }
        // 減速區域顯示為紅色
        else if (gameObject.CompareTag(SPEED_SLOW_TAG))
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // 半透明紅色
        }

        // 繪製區域範圍
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }
}