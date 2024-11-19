using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmap : MonoBehaviour
{
    [Header("Speed Modification Settings")]
    public float speedMultiplier = 1.5f;  // �剗1�׉����C����1�׌���

    private FirstPersonController playerController;
    private float originalWalkSpeed;

    // ��`�p�����ʙ���ތ^�I�W��
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

                // �����n�`�I�W�܌��营�����Ґ�����
                if (gameObject.CompareTag(SPEED_BOOST_TAG))
                {
                    // ��������
                    playerController.walkSpeed *= speedMultiplier;
                    Debug.Log($"Speed boosted to: {playerController.walkSpeed}");
                }
                else if (gameObject.CompareTag(SPEED_SLOW_TAG))
                {
                    // ��������
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
                // �������n���x
                playerController.walkSpeed = originalWalkSpeed;
                Debug.Log($"Speed restored to: {originalWalkSpeed}");
            }
        }
    }

    // �ݕҏS�풆��������I���S��
    private void OnDrawGizmos()
    {
        // �����������������F
        if (gameObject.CompareTag(SPEED_BOOST_TAG))
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f); // ���������F
        }
        // �������������׍g�F
        else if (gameObject.CompareTag(SPEED_SLOW_TAG))
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // �������g�F
        }

        // ㉐�����͚�
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }
}