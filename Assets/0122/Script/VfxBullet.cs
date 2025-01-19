using UnityEngine;

public class VfxBullet : MonoBehaviour
{
    [SerializeField] private string vfxTag; // The tag should match your VFX in VfxSetting
    [SerializeField] private string seTag; // The tag should match your VFX in VfxSetting

    private void OnCollisionEnter(Collision collision)
    {
        // Play VFX at the collision point
        ContactPoint contact = collision.contacts[0];
        VfxManager.Instance.PlayVfx(vfxTag, contact.point);
        SoundManager.Instance.PlaySE(seTag);

        // Optionally destroy the bullet after collision
        // Destroy(gameObject);
    }
}