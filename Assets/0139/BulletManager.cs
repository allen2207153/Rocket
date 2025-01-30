using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletPrefab; // 通常弾プレハブ
    public GameObject stickyBulletPrefab; // 粘着弾プレハブ
    public GameObject gravityBulletPrefab; // 重力弾プレハブ
    public Transform firePoint; // 弾丸発射位置
    public float bulletSpeed = 10f; // 弾丸の速度
    public float fireRate = 0.5f; // 発射間隔（秒）
    //public int maxBullets = 6; // 最大弾丸数

    private int currentBullets; // 現在の残り弾丸数
    private float nextFireTime = 0f; // 次に発射できる時間
    private enum BulletType { Normal, Sticky, Gravity } // 弾丸の種類
    private BulletType currentBulletType = BulletType.Normal; // 現在の弾丸タイプ

    public Image bulletTypeImage; // 弾丸の種類を表示するUI画像
    public Sprite normalBulletSprite; // 通常弾のアイコン
    public Sprite stickyBulletSprite; // 粘着弾のアイコン
    public Sprite gravityBulletSprite; // 重力弾のアイコン

    public Light gunLight; // 銃の光（エディタで設定）

    private void Start()
    {
        //currentBullets = maxBullets; // 初期の弾丸数を設定
        UpdateBulletUI();

        if (gunLight == null)
        {
            gunLight = GetComponentInChildren<Light>();
        }
    }

    public void ShootBullet()
    {
        //if (currentBullets <= 0)
        //{
        //    Debug.Log("弾丸がありません！リロードしてください。");
        //    return;
        //}

        GameObject bulletToShoot = null;

        // 使用する弾丸の種類を決定
        switch (currentBulletType)
        {
            case BulletType.Normal:
                bulletToShoot = bulletPrefab;
                break;
            case BulletType.Sticky:
                bulletToShoot = stickyBulletPrefab;
                break;
            case BulletType.Gravity:
                bulletToShoot = gravityBulletPrefab;
                break;
        }

        var bulletInstance = Instantiate(bulletToShoot, firePoint.position, firePoint.rotation);
        var bulletExplosion = bulletInstance.GetComponent<IBullet>();
        bulletExplosion.Initialize(firePoint.forward, bulletSpeed);

        currentBullets--;
        Debug.Log("残り弾丸数: " + currentBullets);

        // UI更新
        UpdateBulletUI();
    }

    private void Reload()
    {
        //currentBullets = maxBullets;
        Debug.Log("リロード完了。弾丸数リセット: " + currentBullets);
        UpdateBulletUI();

        SoundManager.Instance?.PlaySE("bulletReload");　　　//SEを再生する
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            ShootBullet();
            nextFireTime = Time.time + fireRate;
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    Reload();
        //}

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchBulletType();
        }
    }

    private void SwitchBulletType()
    {
        // 弾丸タイプを切り替える
        currentBulletType = (BulletType)(((int)currentBulletType + 1) % System.Enum.GetValues(typeof(BulletType)).Length);
        Debug.Log(currentBulletType + " に切り替えました。");

        // UI更新
        UpdateBulletUI();

        SoundManager.Instance?.PlaySE("bulletSwitch");　　　//SEを再生する
    }

    private void UpdateBulletUI()
    {
        // 弾丸の種類に応じてアイコンを変更
        switch (currentBulletType)
        {
            case BulletType.Normal:
                bulletTypeImage.sprite = normalBulletSprite;
                if (gunLight != null) gunLight.color = Color.white;
                break;
            case BulletType.Sticky:
                bulletTypeImage.sprite = stickyBulletSprite;
                if (gunLight != null) gunLight.color = Color.green;
                break;
            case BulletType.Gravity:
                bulletTypeImage.sprite = gravityBulletSprite;
                if (gunLight != null) gunLight.color = Color.magenta;
                break;
        }
    }
}
