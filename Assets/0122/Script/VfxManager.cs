using UnityEngine;

public class VfxManager : MonoBehaviour
{

    public static VfxManager Instance { get; private set; }

    [SerializeField] private VfxSetting vfxSetting;

    [SerializeField] private Camera playerCamera;

    [SerializeField] private GameObject player;


    // VFX tags for different effects
    private const string WALL_COLLISION_VFX = "WallHit";
    private const string SPEED_DASH_VFX = "SpeedDash";
    private const string DASH_SKILL_VFX = "DashSkill";


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // カメラを探す
        if (playerCamera == null)
        {
            playerCamera = GameObject.Find("Player_Camera")?.GetComponent<Camera>();
            if (playerCamera == null)
            {
                Debug.LogWarning("Player_Camera 存在していません, main cameraに切り替えます");
                playerCamera = Camera.main;
            }
        }
    }


    //VFXを（一般的）表示する　
    public void PlayVfx(string tag, Vector3 position)
    {
        if (vfxSetting == null)
        {
            Debug.LogError("VfxSetting が割り当てられていません！");
            return;
        }

        VFX vfxToPlay = vfxSetting.Get_VFXList.Find(vfx => vfx.tag == tag);

        if (vfxToPlay.VfxPrefab != null)
        {

            GameObject instantiatedVfx = Instantiate(vfxToPlay.VfxPrefab, position, Quaternion.identity);
            ParticleSystem ps = instantiatedVfx.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
        }
        else
        {
            Debug.LogWarning($"タグ '{tag}' の VFX が見つかりませんでした。");
        }
    }

    // VFXを（カメラに）表示する　
    public void PlayCameraAlignedVfx(string tag, Vector3 position)
    {
        Debug.Log("1. Starting PlayCameraAlignedVfx"); // First step

        if (vfxSetting == null || playerCamera == null)
        {
            Debug.LogError("VfxSetting or PlayerCamera is not assigned!");
            return;
        }
        Debug.Log("2. VfxSetting and Camera OK"); // Check if we get past first check

        VFX vfxToPlay = vfxSetting.Get_VFXList.Find(vfx => vfx.tag == tag);
        // Debug.Log($"3. Found VFX: {vfxToPlay != null}"); // Check if VFX is found

        if (vfxToPlay.VfxPrefab != null)
        {
            Debug.Log("4. VfxPrefab found, trying to instantiate");
            GameObject instantiatedVfx = Instantiate(vfxToPlay.VfxPrefab, position, playerCamera.transform.rotation);
            Debug.Log($"5. VFX instantiated at {position}");

            // Add this debug check
            Debug.Log($"5.5. Does VFX have ParticleSystem? {instantiatedVfx.GetComponent<ParticleSystem>() != null}");


            ParticleSystem ps = instantiatedVfx.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                // Try to find ParticleSystem in children
                ps = instantiatedVfx.GetComponentInChildren<ParticleSystem>();
                Debug.Log("5.7. Searched for ParticleSystem in children");
            }

            if (ps != null)
            {
                Debug.Log("6. Found ParticleSystem, playing");
                ps.Play();
                Debug.Log("7. ParticleSystem started");
            }
            else
            {
                Debug.LogError("No ParticleSystem found in VFX prefab or its children!");
            }
        }
    }
    //  VFXを（キー押すとカメラに）表示する　
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Key pressed");
            if (player != null)
            {
                Debug.Log($"Player position: {player.transform.position}");
                PlayCameraAlignedVfx(DASH_SKILL_VFX, player.transform.position);
            }
            else
            {
                Debug.LogError("Player reference is null!");
            }
        }
    }


    //  VFXを（衝突判定とカメラに）表示する　
    public void HandleCollisionVfx(Collision collision, GameObject player)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            ContactPoint contact = collision.GetContact(0);
            PlayCameraAlignedVfx(WALL_COLLISION_VFX, contact.point);
        }

        if (collision.gameObject.CompareTag("SpeedDash"))
        {
            PlayCameraAlignedVfx(SPEED_DASH_VFX, player.transform.position);
        }
    }
}

