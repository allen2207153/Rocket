using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapObjectSetting : MonoBehaviour
{


    [Header("Speed Settings")]
    public float speedMultiplier = 1.5f;  // 1 より大きい場合は加速、1 より小さい場合は減速

    [Header("Dash Settings")]
    public float dashForce = 0.5f;         // ダッシュ 
    public float dashDuration = 0.5f;     // ダッシュ期間(時間)
    public bool continuousDash = false;   // ダッシュを続ける（ループ）：プレイヤーがオブジェクトに接触したら、ダッシュループする
    public float dashCooldown = 1f;       // ダッシュクールダウン(再発動時間) (連続ループモードのみ)

    private FirstPersonController playerController;
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
            playerController = other.GetComponent<FirstPersonController>();
            playerRigidbody = other.GetComponent<Rigidbody>();

            if (playerController != null && playerRigidbody != null)
            {
                originalWalkSpeed = playerController.walkSpeed;

                // 地形ラベルに基づいて加速するか減速するかを決定します
                if (gameObject.CompareTag(SPEED_BOOST_TAG))
                {
                    // 加速ゾーン
                    playerController.walkSpeed *= speedMultiplier;
                    Debug.Log($"Speed boosted to: {playerController.walkSpeed}");
                }
                else if (gameObject.CompareTag(SPEED_SLOW_TAG))
                {
                    // 減速ゾーン
                    playerController.walkSpeed *= (1f / speedMultiplier);
                    Debug.Log($"Speed reduced to: {playerController.walkSpeed}");
                }

                else if (gameObject.CompareTag(SPEED_DASH_TAG) && !isDashing && canDash)
                {
                    // ダッシュゾーン
                    StartCoroutine(SpeedDash());
                    Debug.Log("Starting dash boost!");
                }
            }
            else
            {
                Debug.LogWarning("Missing required components on player!"); // 
            }

        }

    }

    private void OnTriggerStay(Collider other)
    {
        // 連続スプリントモードでプレイヤーがエリア内にいる場合
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
                if (playerController != null)
                {
                    // 元の速度に戻す
                    playerController.walkSpeed = originalWalkSpeed;
                    Debug.Log($"Speed restored to: {originalWalkSpeed}");
                }
            }
        }

    private IEnumerator SpeedDash()
    {
        if (playerController != null && playerRigidbody != null)
        {
            isDashing = true;
            canDash = false;

            // 今プレイヤーの方向
            Vector3 dashDirection = playerController.transform.forward;

            // 移動禁止
            bool originalCanMove = playerController.playerCanMove;
            playerController.playerCanMove = false;
            Debug.Log($"Applying dash force: {dashForce}"); //

            // スプリント elapsed
            float elapsedTime = 0f;
            while (elapsedTime < dashDuration)
            {
                //  AddForce 
                playerRigidbody.AddForce(dashDirection * dashForce, ForceMode.Impulse);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 元に戻す
            playerController.playerCanMove = originalCanMove;
            isDashing = false;

            // cooldown
            if (continuousDash)
            {
                yield return new WaitForSeconds(dashCooldown);
                canDash = true;
            }
        }
        else
        {
            Debug.LogError("Missing player components during dash!"); // 
        }

    }


    //------------------TEMP---------------------
    // エディターで領域を表示するための視覚的なキュー
    private void OnDrawGizmos()
        {
            // 加速ゾーン: 緑
            if (gameObject.CompareTag(SPEED_BOOST_TAG))
            {
                Gizmos.color = new Color(0f, 1f, 0f, 0.3f); // 半透明の緑
            }
            // 減速ゾーン: 赤
            else if (gameObject.CompareTag(SPEED_SLOW_TAG))
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // 半透明の赤
            }
            // ダッシュゾーン: 赤
            else if (gameObject.CompareTag(SPEED_DASH_TAG)) 
            {
            Gizmos.color = new Color(0f, 0f, 1f, 0.3f); // 半透明の青
        }


        // 描画範囲
        Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(Vector3.zero, Vector3.one);
            }
        }
    }