using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapObjectSetting : MonoBehaviour
{

    // 加、減速
    [Header("Speed Settings")]  
    public float speedMultiplier = 1.5f;  // 1 より大きい場合は加速、1 より小さい場合は速

    // オートダッシュ
    [Header("Dash Settings")]　
    public float dashForce = 100f;         // ダッシュ度
    public float dashDuration = 0.2f;     // ダッシュ期間(時間)
    public bool continuousDash = false;   // ダッシュを続ける（ループ）：プレイヤーがオブジェクトに接触したら、ダッシュループする
    public float dashCooldown = 1f;       // ダッシュクールダウン(再発動時間) (連続ループモードのみ)

    /****************************************
    [Header("Moving Platform Settings")]
    public Vector3 moveDirection = Vector3.right;  // 移動方向
    public float moveDistance = 5f;               // 移動距離
    public float moveSpeed = 2f;                  // 移動速度
    */

    // 消えるプレート
    [Header("Disappearing Platform Settings")]
    public float visibleTime = 2f;                // 表示時間
    public float invisibleTime = 1f;              // 非表示時間
    public bool startVisible = true;              // 開始時に表示するか
    public float fadeSpeed = 2f;                  // フェード速度

    // 墜ちるプレート
    [Header("Fall Platform Settings")]
    public float timeToFall = 1.0f;        // プレイヤーが乗ってから消えるまでの時間
    public float timeToRespawn = 2.0f;     // プレイヤーが離れてから再表示までの時間
    private bool isFalling = false;        // 落下中かどうか
    private Coroutine fallCoroutine = null;

    //****************************************

  private Player_Movement PlayerMovement;
  private float originalWalkSpeed;

  private Rigidbody playerRigidbody;  //Rigidbodyのコンポーネント
  private bool isDashing = false;
  private bool canDash = true;

  // プレート関連変数
  private Vector3 startPosition;
  private bool movingForward = true;
  private MeshRenderer meshRenderer;
  private Collider platformCollider;
  private bool isDisappearing = false;


  // ゾーンタイプを識別する
  private const string SPEED_BOOST_TAG = "SpeedBoost";    //加速
  private const string SPEED_SLOW_TAG = "SpeedSlow";      //減速
  private const string SPEED_DASH_TAG = "SpeedDash";      //オートダッシュ 

  // トラップを識別する
  private const string TRAP_MOVE_TAG = "MovingPlate";         //ムービングプレート
  private const string TRAP_DISAPPEAR_TAG = "DisappearPlate"; //消えるプレート
  private const string TRAP_FALL_TAG = "FallPlate";           //墜ちるプレート

  private Material platformMaterial;
  private Color originalColor;
  private float currentAlpha = 1f;

  private void Start()
  {
      startPosition = transform.position;
      meshRenderer = GetComponent<MeshRenderer>();
      platformCollider = GetComponent<Collider>();

      // マテリアルを取得し、元の色を保存する
      if (meshRenderer)
      {
          platformMaterial = meshRenderer.material;
          originalColor = platformMaterial.color;
      }

      // 消えるプレート初期化
      if (gameObject.CompareTag(TRAP_DISAPPEAR_TAG))
      {
          if (startVisible)
              StartCoroutine(DisappearRoutine());
          else
          {
              SetPlatformAlpha(0f);
              if (platformCollider) platformCollider.enabled = false;
              StartCoroutine(DisappearRoutine());
          }
      }
  }

  // Helper function to set platform transparency
  private void SetPlatformAlpha(float alpha)
  {
      if (meshRenderer && platformMaterial)
      {
          Color newColor = originalColor;
          newColor.a = alpha;
          platformMaterial.color = newColor;
          currentAlpha = alpha;

          // When fully transparent, disable the MeshRenderer completely
          if (alpha <= 0)
          {
              meshRenderer.enabled = false;
          }
          else if (!meshRenderer.enabled)
          {
              meshRenderer.enabled = true;
          }
      }
  }

    /* private void Update()
     {
         // ムービングプレートの更新
         if (gameObject.CompareTag(TRAP_MOVE_TAG))
         {
             UpdateMovingPlatform();
         }
     }
    */

    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーが落ちるプレートに乗った時
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag(TRAP_FALL_TAG) && !isFalling)
        {
            if (fallCoroutine != null)
            {
                StopCoroutine(fallCoroutine);
            }
            fallCoroutine = StartCoroutine(FallPlatformRoutine());
        }
    }
    private IEnumerator FallPlatformRoutine()
    {
        isFalling = true;

        // プレイヤーが乗ってから指定時間待つ
        yield return new WaitForSeconds(timeToFall);

        // フェードアウト
        yield return StartCoroutine(FadeRoutine(1f, 0f));
        if (platformCollider) platformCollider.enabled = false;

        // プレイヤーが離れてから指定時間待つ
        yield return new WaitForSeconds(timeToRespawn);

        // プラットフォームを再表示
        if (platformCollider) platformCollider.enabled = true;
        yield return StartCoroutine(FadeRoutine(0f, 1f));

        isFalling = false;
        fallCoroutine = null;
    }


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

                else if (gameObject.CompareTag(TRAP_FALL_TAG))
                {
                    // ダッシュゾーン
                    StartCoroutine(SpeedDash());
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
                playerRigidbody.AddForce(dashDirection * dashForce, ForceMode.Force);
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
    // 消えるプレート

    private IEnumerator DisappearRoutine()
    {
        while (true)
        {
            // フェードイン
            if (platformCollider) platformCollider.enabled = true;
            yield return StartCoroutine(FadeRoutine(0f, 1f));
            yield return new WaitForSeconds(visibleTime);

            // フェードアウト
            yield return StartCoroutine(FadeRoutine(1f, 0f));
            if (platformCollider) platformCollider.enabled = false;
            yield return new WaitForSeconds(invisibleTime);
        }
    }

    private IEnumerator FadeRoutine(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        float startTime = Time.time;

        while (elapsedTime < 1f)
        {
            elapsedTime = (Time.time - startTime) * fadeSpeed;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime);
            SetPlatformAlpha(currentAlpha);
            yield return null;
        }

        SetPlatformAlpha(targetAlpha);
    }
}


