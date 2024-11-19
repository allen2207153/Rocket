using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmap : MonoBehaviour
{
    [Header("Speed Modification Settings")]
    public float speedMultiplier = 1.5f;  // å1×Á¬C¬1×ž¬

    private FirstPersonController playerController;
    private float originalWalkSpeed;

    // è`p¯ÊœæÞ^IWâÜ
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

                // ªn`IWâÜè¥Á¬Ò¥ž¬
                if (gameObject.CompareTag(SPEED_BOOST_TAG))
                {
                    // Á¬œæ
                    playerController.walkSpeed *= speedMultiplier;
                    Debug.Log($"Speed boosted to: {playerController.walkSpeed}");
                }
                else if (gameObject.CompareTag(SPEED_SLOW_TAG))
                {
                    // ž¬œæ
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
                // øŽn¬x
                playerController.walkSpeed = originalWalkSpeed;
                Debug.Log($"Speed restored to: {originalWalkSpeed}");
            }
        }
    }

    // ÝÒSíèûŠœæIæSñŠ
    private void OnDrawGizmos()
    {
        // Á¬œæèûŠ×ûF
        if (gameObject.CompareTag(SPEED_BOOST_TAG))
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f); // Œ§ŸûF
        }
        // ž¬œæèûŠ×gF
        else if (gameObject.CompareTag(SPEED_SLOW_TAG))
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // Œ§ŸgF
        }

        // ã»œæÍ¡
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }
}