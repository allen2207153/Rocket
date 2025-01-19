using UnityEngine;

public class WeaponPickup : ItemBase
{
    public GameObject weaponHolder; // 有効化する武器ホルダーオブジェクト
    public GameObject playerObject; // Player_1128オブジェクトへの参照

    private BulletManager bulletManagerScript; // BulletManagerスクリプトへの参照

    private void Start()
    {
        // Player_1128オブジェクトからBulletManagerスクリプトを取得
        if (playerObject != null)
        {
            bulletManagerScript = playerObject.GetComponent<BulletManager>();
        }
        else
        {
            // Player_1128が設定されていない場合、エラーメッセージを表示
            Debug.LogError("Player_1128 オブジェクトが設定されていません！");
        }
    }

    protected override void OnPickup(Collider player)
    {
        // 武器ホルダーを有効化
        if (weaponHolder != null)
        {
            weaponHolder.SetActive(true);
        }

        // Player_1128上のBulletManagerスクリプトを有効化
        if (bulletManagerScript != null)
        {
            bulletManagerScript.enabled = true; // スクリプトを有効化
            Debug.Log("Player_1128 上の BulletManager が有効化されました！");
        }
        else
        {
            // BulletManagerスクリプトが見つからない場合、エラーメッセージを表示
            Debug.LogError("Player_1128 に BulletManager スクリプトが見つかりません！");
        }
    }
}
