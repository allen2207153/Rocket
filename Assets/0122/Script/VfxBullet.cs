using UnityEngine;

public class VfxBullet : MonoBehaviour
{
    [SerializeField] private string vfxTag; // VfxSettingでファイルを設定が必要です。
    [SerializeField] private string seTag; // SoundSettingでファイルを設定が必要です。

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

