//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BulletManager : MonoBehaviour
//{
//    public GameObject bulletPrefab; // 弾丸のプレハブ
//    public Transform firePoint; // 弾丸発射位置
//    public float bulletSpeed = 10f; // 弾丸の速度

//    // 弾丸を発射する処理
//    public void ShootBullet()
//    {
//        // 弾丸の生成
//        var bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

//        // Bullet_Explosionコンポーネントを取得して、初期化
//        var bulletExplosion = bulletInstance.GetComponent<Bullet_Explosion>();

//            // 弾丸の方向と速度を設定
//        bulletExplosion.Initialize(firePoint.forward, bulletSpeed);
//        bulletExplosion.Move();

//    }

//    // 毎フレーム更新される処理
//    private void Update()
//    {
//        // 発射テスト用にマウスクリックで弾を撃つ
//        if (Input.GetButtonDown("Fire1"))
//        {
//            ShootBullet();
//            Debug.Log("Success");
//        }
//    }
//}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro; 

//public class BulletManager : MonoBehaviour
//{
//    public GameObject bulletPrefab; // 弾丸のプレハブ\
//    public Transform firePoint; // 弾丸発射位置
//    public float bulletSpeed = 10f; // 弾丸の速度
//    public float fireRate = 0.5f; // 開火の間隔（秒）
//    public int maxBullets = 6; // 最大弾丸数

//    private int currentBullets; // 現在の残り弾丸数
//    private float nextFireTime = 0f; // 次に発射できる時間

//    public TextMeshProUGUI bulletTextTMP; // 子彈數量を表示する TextMeshPro UI 元素
//   //public Stick_Explosion gggl;

//    private void Start()
//    {
//        // 初期化時に弾丸数を最大値に設定
//        currentBullets = maxBullets;
//        UpdateBulletUI(); // 初期の子彈数を表示
//        //gggl = GetComponent<Stick_Explosion>();
//    }

//    // 弾丸を発射する処理
//    public void ShootBullet()
//    {
//        // 弾丸が残っていない場合、発射せずメッセージを表示する
//        if (currentBullets <= 0)
//        {
//            Debug.Log("弾丸がありません！リロードが必要です。");
//            return;
//        }

//        // 弾丸の生成
//        var bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

//        // Bullet_Explosionコンポーネントを取得して、初期化
//        var bulletExplosion = bulletInstance.GetComponent<Bullet_Explosion>();

//        // 弾丸の方向と速度を設定
//        bulletExplosion.Initialize(firePoint.forward, bulletSpeed);
//        bulletExplosion.Move();

//        // 発射後に弾丸数を減少
//        currentBullets--;
//        Debug.Log("残り弾丸数: " + currentBullets);

//        // 弾丸数の UI 表示を更新
//        UpdateBulletUI();
//    }

//    // リロード処理
//    private void Reload()
//    {
//        // 弾丸数を最大値にリセット
//        currentBullets = maxBullets;
//        Debug.Log("リロード完了。弾丸数がリセットされました: " + currentBullets);

//        // 弾丸数の UI 表示を更新
//        UpdateBulletUI();
//    }

//    // 毎フレーム更新される処理
//    private void Update()
//    {
//        // 現在の時間が次に発射できる時間を超えている場合に発射する
//        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
//        {
//            ShootBullet();
//            nextFireTime = Time.time + fireRate; // 次の発射可能時間を更新
//        }

//        // Rキーを押すとリロードを行う
//        if (Input.GetKeyDown(KeyCode.R))
//        {
//            Reload();
//        }
//    }

//    // 弾丸数の UI 表示を更新する処理
//    private void UpdateBulletUI()
//    {
//        // 弾丸数を UI に表示
//        bulletTextTMP.text = "Bullet : " + currentBullets + "/" + maxBullets;
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletPrefab; // 弾丸のプレハブ
    public GameObject stickyBulletPrefab; // 粘着弾丸のプレハブ
    public GameObject gravityBulletPrefab; // 重力弾丸のプレハブ
    public Transform firePoint; // 弾丸発射位置
    public float bulletSpeed = 10f; // 弾丸の速度
    public float fireRate = 0.5f; // 開火の間隔（秒）
    public int maxBullets = 6; // 最大弾丸数

    private int currentBullets; // 現在の残り弾丸数
    private float nextFireTime = 0f; // 次に発射できる時間
    private enum BulletType { Normal, Sticky, Gravity } // 弾丸の種類
    private BulletType currentBulletType = BulletType.Normal; // 現在の弾丸の種類

    public TextMeshProUGUI bulletTextTMP; // 子彈數量を表示する TextMeshPro UI 元素

    private void Start()
    {
        // 初期化時に弾丸数を最大値に設定
        currentBullets = maxBullets;
        UpdateBulletUI(); // 初期の子彈数を表示
    }

    // 弾丸を発射する処理
    public void ShootBullet()
    {
        // 弾丸が残っていない場合、発射せずメッセージを表示する
        if (currentBullets <= 0)
        {
            Debug.Log("弾丸がありません！リロードが必要です。");
            return;
        }

        // 使用する弾丸の種類を決定
        GameObject bulletToShoot = null;
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

        // 弾丸の生成
        var bulletInstance = Instantiate(bulletToShoot, firePoint.position, firePoint.rotation);

        // Bullet_ExplosionまたはStickyBulletExplosionまたはGravityBulletExplosionコンポーネントを取得して、初期化
        var bulletExplosion = bulletInstance.GetComponent<IBullet>();
        bulletExplosion.Initialize(firePoint.forward, bulletSpeed);

        // 発射後に弾丸数を減少
        currentBullets--;
        Debug.Log("残り弾丸数: " + currentBullets);

        // 弾丸数の UI 表示を更新
        UpdateBulletUI();
    }

    // リロード処理
    private void Reload()
    {
        // 弾丸数を最大値にリセット
        currentBullets = maxBullets;
        Debug.Log("リロード完了。弾丸数がリセットされました: " + currentBullets);

        // 弾丸数の UI 表示を更新
        UpdateBulletUI();
    }

    // 毎フレーム更新される処理
    private void Update()
    {
        // 現在の時間が次に発射できる時間を超えている場合に発射する
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            ShootBullet();
            nextFireTime = Time.time + fireRate; // 次の発射可能時間を更新
        }

        // Rキーを押すとリロードを行う
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        // Qキーを押すと弾丸の種類を切り替える
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchBulletType();
        }
    }

    // 弾丸の種類を切り替える処理
    private void SwitchBulletType()
    {
        // 弾丸の種類を順番に切り替える
        currentBulletType = (BulletType)(((int)currentBulletType + 1) % System.Enum.GetValues(typeof(BulletType)).Length);
        Debug.Log(currentBulletType + " に切り替えました。");
    }

    // 弾丸数の UI 表示を更新する処理
    private void UpdateBulletUI()
    {
        // 弾丸数を UI に表示
        bulletTextTMP.text = "Bullet : " + currentBullets + "/" + maxBullets;
    }
}

