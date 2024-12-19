using UnityEngine;

public class VfxBullet : MonoBehaviour
{
    [SerializeField] private string vfxTag = "VFX1"; // The tag should match your VFX in VfxSetting

    private void OnCollisionEnter(Collision collision)
    {
        // Play VFX at the collision point
        ContactPoint contact = collision.contacts[0];
        VfxManager.Instance.PlayVfx(vfxTag, contact.point);

        // Optionally destroy the bullet after collision
       // Destroy(gameObject);
    }
}