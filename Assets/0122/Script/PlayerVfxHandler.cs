using UnityEngine;

public class PlayerVfxHandler : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    private void Start()
    {
        // Get the player camera if not assigned
        if (playerCamera == null)
        {
            // Try to find camera by name first
            playerCamera = GameObject.Find("Player_Camera")?.GetComponent<Camera>();

            // If not found, try to find as child
            if (playerCamera == null)
            {
                playerCamera = GetComponentInChildren<Camera>();
            }

            // If still not found, use main camera as fallback
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (VfxManager.Instance != null)
        {
            VfxManager.Instance.HandleCollisionVfx(collision, gameObject);
        }
    }
}
