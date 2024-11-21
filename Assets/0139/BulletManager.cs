using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletPrefab; // 弾丸のプレハブ
    public Transform firePoint; // 弾丸発射位置
    public float bulletSpeed = 10f; // 弾丸の速度

    // 弾丸を発射する処理
    public void ShootBullet()
    {
        // 弾丸の生成
        var bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Bullet_Explosionコンポーネントを取得して、初期化
        var bulletExplosion = bulletInstance.GetComponent<Bullet_Explosion>();
       
            // 弾丸の方向と速度を設定
        bulletExplosion.Initialize(firePoint.forward, bulletSpeed);
        bulletExplosion.Move();
        
    }

    // 毎フレーム更新される処理
    private void Update()
    {
        // 発射テスト用にマウスクリックで弾を撃つ
        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet();
            Debug.Log("Success");
        }
    }
}