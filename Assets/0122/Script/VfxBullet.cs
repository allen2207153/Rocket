using System.Diagnostics;
using UnityEngine;

public class VfxBullet : MonoBehaviour
{
    [SerializeField] private string vfxTag; // The tag should match your VFX in VfxSetting
    [SerializeField] private string seTag; // The tag should match your VFX in VfxSetting
    private Gravity_Explosion_Effect _gravity;
    private bool _gravitybool;
    private void Update()
    {

        
        bool _gravitybool = _gravity.GetComponent<Gravity_Explosion_Effect>().hasExploded;
        UnityEngine.Debug.Log(_gravitybool);
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Play VFX at the collision point
        ContactPoint contact = collision.contacts[0];
        VfxManager.Instance.PlayVfx(vfxTag, contact.point);
        SoundManager.Instance.PlaySE(seTag);
        if(_gravitybool ==true)
        {
            VfxManager.Instance.PlayVfx(vfxTag, contact.point);
        }
        // Optionally destroy the bullet after collision
        // Destroy(gameObject);
    }


}