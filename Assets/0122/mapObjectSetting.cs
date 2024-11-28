using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapObjectSetting : MonoBehaviour
{


    [Header("Speed Settings")]
    public float speedMultiplier = 1.5f;  // 1 より大きい場合は加速、1 より小さい場合は減速

    [Header("Dash Settings")]
    public float dashForce = 100f;         // ダッシュ 
    public float dashDuration = 0.2f;     // ダッシュ期間(時間)
    public bool continuousDash = false;   // ダッシュを続ける（ループ）：プレイヤーがオブジェクトに接触したら、ダッシュループする
    public float dashCooldown = 1f;       // ダッシュクールダウン(再発動時間) (連続ループモードのみ)

    private Player_Movement PlayerMovement;
    private float originalWalkSpeed;

    private Rigidbody playerRigidbody;  //Rigidbodyのコンポーネント
    private bool isDashing = false;
    private bool canDash = true;

    // ゾーンタイプを識別する
    private const string SPEED_BOOST_TAG = "SpeedBoost";    //加速
    private const string SPEED_SLOW_TAG = "SpeedSlow";      //減速
    private const string SPEED_DASH_TAG = "SpeedDash";      //ダッシュ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement = other.GetComponent<Player_Movement>();
            playerRigidbody = other.GetComponent<Rigidbody>();

            if (PlayerMovement != null && playerRigidbody != null)
            {
                originalWalkSpeed = PlayerMovement.walkSpeed;

                // 地形ラベルに基づいて加速するか減速するかを決定します
                if (gameObject.CompareTag(SPEED_BOOST_TAG))
                {
                    // 加速ゾーン
                    PlayerMovement.walkSpeed *= speedMultiplier;
                    Debug.Log($"プレイヤー速度加速: {PlayerMovement.walkSpeed}");
                }
                else if (gameObject.CompareTag(SPEED_SLOW_TAG))
                {
                    // 減速ゾーン
                    PlayerMovement.walkSpeed *= (1f / speedMultiplier);
                    Debug.Log($"プレイヤー速度減速: {PlayerMovement.walkSpeed}");
                }

                else if (gameObject.CompareTag(SPEED_DASH_TAG) && !isDashing && canDash)
                {
                    // ダッシュゾーン
                    StartCoroutine(SpeedDash());
                    Debug.Log("ダッシュ開始！");
                }
            }
            else
            {
                Debug.LogWarning("プレイヤー　コンポーネント　なし！"); // 
            }

        }

    }

    private void OnTriggerStay(Collider other)
    {
        // 連続ダッシュモードでプレイヤーがエリア内にいる場合
        if (continuousDash && other.CompareTag("Player") &&
            gameObject.CompareTag(SPEED_DASH_TAG) && !isDashing && canDash)
        {
            StartCoroutine(SpeedDash());
        }
    }


    private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (PlayerMovement != null)
                {
                // 元の速度に戻す
                PlayerMovement.walkSpeed = originalWalkSpeed;
                    Debug.Log($"元速度に戻す: {originalWalkSpeed}");
                }
            }
        }
    private IEnumerator SpeedDash()
    {
        if (PlayerMovement != null && playerRigidbody != null)
        {
            isDashing = true;
            canDash = false;

            // 今入力している移動値
            float inputH = Input.GetAxisRaw("Horizontal");
            float inputV = Input.GetAxisRaw("Vertical");

            // 今プレイヤーの方向
            Vector3 dashDirection = PlayerMovement.transform.forward;

            Debug.Log($"ダッシュの力: {dashForce}");



            // ダッシュ
            float elapsedTime = 0f;
            while (elapsedTime < dashDuration)
            {
                playerRigidbody.AddForce(dashDirection * dashForce, ForceMode.Impulse);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            isDashing = false;

            // ダッシュなら、クールダウンを利用する

              if (continuousDash)
                 {
                     yield return new WaitForSeconds(dashCooldown);
                      canDash = true;
                   }
                }
                else
               {
                   Debug.LogError(" ダッシュなからプレイヤー　コンポーネント　なし！");
                }
        }

    }
