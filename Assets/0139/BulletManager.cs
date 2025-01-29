using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletPrefab; // 弾丸のプレハブ（通常の弾丸）
    public GameObject stickyBulletPrefab; // 粘着弾丸のプレハブ
    public GameObject gravityBulletPrefab; // 重力弾丸のプレハブ
    public Transform firePoint; // 弾丸発射位置
    public float bulletSpeed = 10f; // 弾丸の速度
    public float fireRate = 0.5f; // 発射間隔（秒）
    public int maxBullets = 6; // 最大弾丸数

    private int currentBullets; // 現在の残り弾丸数
    private float nextFireTime = 0f; // 次に発射できる時間
    private enum BulletType { Normal, Sticky, Gravity } // 弾丸の種類
    private BulletType currentBulletType = BulletType.Normal; // 現在選択されている弾丸の種類

    public TextMeshProUGUI bulletTextTMP; // 弾丸数を表示するための TextMeshPro UI

    public Light gunLight; // 銃に取り付けられた Light コンポーネント（Unity Editor で設定）

    private void Start()
    {
        currentBullets = maxBullets; // 初期弾丸数を設定
        UpdateBulletUI();

        if (gunLight == null)
        {
            gunLight = GetComponentInChildren<Light>();
        }
    }

    public void ShootBullet()
    {
        if (currentBullets <= 0)
        {
            Debug.Log("弾丸がありません！リロードが必要です。");
            return;
        }

        GameObject bulletToShoot = null;
        string description = ""; // 子彈描述文字

        // 使用する弾丸の種類を決定
        switch (currentBulletType)
        {
            case BulletType.Normal:
                bulletToShoot = bulletPrefab;
                description = "通常弾：標準的な弾丸です。";
                break;
            case BulletType.Sticky:
                bulletToShoot = stickyBulletPrefab;
                description = "粘着弾：ターゲットに粘着します。";
                break;
            case BulletType.Gravity:
                bulletToShoot = gravityBulletPrefab;
                description = "重力弾：重力フィールドを発生させます。";
                break;
        }

        var bulletInstance = Instantiate(bulletToShoot, firePoint.position, firePoint.rotation);
        var bulletExplosion = bulletInstance.GetComponent<IBullet>();
        bulletExplosion.Initialize(firePoint.forward, bulletSpeed);

        currentBullets--;
        Debug.Log("残り弾丸数: " + currentBullets);

        // 更新 UI，包括目前子彈數量和種類描述
        bulletTextTMP.text = "Bullet : " + currentBullets + "/" + maxBullets + "\n" + description;
    }

    private void Reload()
    {
        currentBullets = maxBullets;
        Debug.Log("リロード完了。弾丸数がリセットされました: " + currentBullets);
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchBulletType();
        }
    }

    private void SwitchBulletType()
    {
        currentBulletType = (BulletType)(((int)currentBulletType + 1) % System.Enum.GetValues(typeof(BulletType)).Length);
        Debug.Log(currentBulletType + " に切り替えました。");

        // 切換槍枝類型時自動更新光源顏色及 UI
        SwitchGunColor();
        UpdateBulletUI();

        SoundManager.Instance?.PlaySE("bulletSwitch");　　　//SEを再生する
    }

    private void SwitchGunColor()
    {
        switch (currentBulletType)
        {
            case BulletType.Normal:
                if (gunLight != null) gunLight.color = Color.white;
                break;
            case BulletType.Sticky:
                if (gunLight != null) gunLight.color = Color.green;
                break;
            case BulletType.Gravity:
                if (gunLight != null) gunLight.color = Color.magenta;
                break;
        }
    }

    private void UpdateBulletUI()
    {
        string description = "";

        // 現在の弾丸種類に応じて説明文を設定
        switch (currentBulletType)
        {
            case BulletType.Normal:
                description = "通常弾：標準的な弾丸です。";
                break;
            case BulletType.Sticky:
                description = "粘着弾：ターゲットに粘着します。";
                break;
            case BulletType.Gravity:
                description = "重力弾：重力フィールドを発生させます。";
                break;
        }

        bulletTextTMP.text = "Bullet : " + currentBullets + "/" + maxBullets + "\n" + description;
    }
}
