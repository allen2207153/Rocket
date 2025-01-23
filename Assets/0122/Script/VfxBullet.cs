using UnityEngine;

public class VfxBullet : MonoBehaviour
{
    [SerializeField] private string vfxTag; // VfxSetting�Ńt�@�C����ݒ肪�K�v�ł��B
    [SerializeField] private string seTag; // SoundSetting�Ńt�@�C����ݒ肪�K�v�ł��B

    private bool effectPlayed = false;
    void Update()
    {
        if (!effectPlayed)
        {
            var gravity = GetComponent<Gravity_Explosion_Effect>();
            if (gravity != null && gravity.hasExploded)
            {
                effectPlayed = true;
                PlayVFX();
            }
        }
    }

    void OnDestroy()
    {
        var bullet = GetComponent<Bullet_Explosion>();
        var sticky = GetComponent<StickyBulletExplosion>();

        if ((bullet != null && bullet.hasExploded) ||
            (sticky != null && sticky.hasExploded))
        {
            PlayVFX();
        }
    }
    private void PlayVFX()
    {
        VfxManager.Instance?.PlayVfx(vfxTag, transform.position);
        SoundManager.Instance?.PlaySE(seTag);
    }

}

